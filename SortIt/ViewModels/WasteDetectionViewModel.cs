using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SortIt.Resources.Localization;
using SortIt.Services;

namespace SortIt.ViewModels
{
    public partial class WasteDetectionViewModel : ObservableObject
    {
        // Teenus, mis suhtleb Google Vision API-ga
        private readonly CloudVisionAPIService _visionService;

        // Teenus, mis suhtleb Google Vision API-ga
        private string? lastResultResourceKey = nameof(AppResources.NoResult);
        private string? lastStatusResourceKey = nameof(AppResources.TakePhotoAnalysis);
        private string? lastContainerResourceKey = null;

        // Pildi asukoht seadmes
        [ObservableProperty]
        private string? imagePath;

        // Tuvastuse tulemus
        [ObservableProperty]
        private string resultText = AppResources.NoResult;

        // Tuvastuse tulemus
        [ObservableProperty]
        private string containerText = "";

        // Kas rakendus on hetkel hõivatud (nt tuvastab jäätmeid)
        [ObservableProperty]
        private bool isBusy;

        public bool IsNotBusy => !IsBusy;

        public bool HasImage => !string.IsNullOrWhiteSpace(ImagePath);

        public bool CanDetect => HasImage && !IsBusy;

        [ObservableProperty]
        private string statusText = AppResources.TakePhotoAnalysis;

        // Leitud konteineri resource key
        [ObservableProperty]
        private string? detectedContainerKey;

        public WasteDetectionViewModel(CloudVisionAPIService visionService)
        {
            _visionService = visionService;
            // Kui keel muutub, uuendame tekste
            LanguageService.LanguageChanged += OnLanguageChanged;
        }

        // Käsk foto tegemiseks
        [RelayCommand]
        public async Task TakePhotoAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                // Küsi kaamera luba
                var status = await Permissions.RequestAsync<Permissions.Camera>();

                if (status != PermissionStatus.Granted)
                {
                    ResultText = AppResources.CameraPermissionDenied;
                    lastResultResourceKey = nameof(AppResources.CameraPermissionDenied);

                    StatusText = AppResources.CameraPermissionRequired;
                    lastStatusResourceKey = nameof(AppResources.CameraPermissionRequired);
                    return;
                }

                // Kontrolli, kas kaamera on seadmes olemas
                if (!MediaPicker.Default.IsCaptureSupported)
                {
                    ResultText = AppResources.CameraNotAvailable;
                    lastResultResourceKey = nameof(AppResources.CameraNotAvailable);

                    StatusText = AppResources.CameraNotSupportedOnDevice;
                    lastStatusResourceKey = nameof(AppResources.CameraNotSupportedOnDevice);
                    return;
                }

                // Foto tegemine
                var photo = await MediaPicker.Default.CapturePhotoAsync();
                if (photo == null)
                {
                    StatusText = AppResources.PhotoCaptureCancelled;
                    lastStatusResourceKey = nameof(AppResources.PhotoCaptureCancelled);
                    return;
                }

                // Salvesta foto ajutisse kausta
                string localPath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                await using var sourceStream = await photo.OpenReadAsync();
                await using var localFileStream = File.OpenWrite(localPath);
                await sourceStream.CopyToAsync(localFileStream);

                ImagePath = localPath;

                // Uuenda tekste
                ResultText = AppResources.ImageSelected_ResultText;
                lastResultResourceKey = nameof(AppResources.ImageSelected_ResultText);

                ContainerText = "";
                lastContainerResourceKey = null;

                StatusText = AppResources.ImageSelected_StatusText;
                lastStatusResourceKey = nameof(AppResources.ImageSelected_StatusText);
            }
            finally
            {
                IsBusy = false;
            }
        }

        // Käsk jäätme tuvastamiseks
        [RelayCommand]
        public async Task DetectWasteAsync()
        {
            // Kui pilti pole, ei saa tuvastada
            if (string.IsNullOrWhiteSpace(ImagePath))
            {
                ResultText = AppResources.TakePhotoFirst;
                detectedContainerKey = null;
                return;
            }

            try
            {
                IsBusy = true;
                StatusText = AppResources.AnalysisInProgress;
                lastStatusResourceKey = nameof(AppResources.AnalysisInProgress);

                // Saadame pildi Google Vision API-le
                var result = await _visionService.DetectObjectAsync(ImagePath);

                if (string.IsNullOrWhiteSpace(result.label))
                {
                    ResultText = AppResources.DetectionFailed;
                    lastResultResourceKey = nameof(AppResources.DetectionFailed);

                    ContainerText = "";
                    detectedContainerKey = null;

                    StatusText = AppResources.ObjectNotDetected;
                    lastStatusResourceKey = nameof(AppResources.ObjectNotDetected);
                    return;
                }

                // Leiame sobiva konteineri
                var containerKey = WasteCategoryMapper.MapLabelToContainerKey(result.label);
                detectedContainerKey = containerKey;

                ContainerText = AppResources.ResourceManager.GetString(containerKey, AppResources.Culture) ?? containerKey;

                // Kuvame tuvastatud objekti ja kindluse protsendi
                ResultText = $"{result.label} ({result.confidence:P0})";
                lastResultResourceKey = null;

                StatusText = AppResources.DetectionSucceeded;
                lastStatusResourceKey = nameof(AppResources.DetectionSucceeded);
            }
            finally
            {
                IsBusy = false;
            }
        }

        // Kui pildi tee muutub, uuendame nupu olekut
        partial void OnImagePathChanged(string? value)
        {
            OnPropertyChanged(nameof(HasImage));
            OnPropertyChanged(nameof(CanDetect));
        }

        // Kui IsBusy muutub, uuendame nupu olekut
        partial void OnIsBusyChanged(bool value)
        {
            OnPropertyChanged(nameof(IsNotBusy));
            OnPropertyChanged(nameof(CanDetect));
        }

        // Keele muutmisel uuendame tekste
        public void OnDisappearing()
        {
            LanguageService.LanguageChanged -= OnLanguageChanged;
        }

        // Keele muutmisel uuendame tekste
        private void OnLanguageChanged()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (!string.IsNullOrWhiteSpace(lastStatusResourceKey))
                    StatusText = AppResources.ResourceManager.GetString(lastStatusResourceKey, AppResources.Culture) ?? StatusText;

                if (!string.IsNullOrWhiteSpace(lastResultResourceKey))
                    ResultText = AppResources.ResourceManager.GetString(lastResultResourceKey, AppResources.Culture) ?? ResultText;

                if (!string.IsNullOrWhiteSpace(DetectedContainerKey))
                {
                    ContainerText = AppResources.ResourceManager.GetString(DetectedContainerKey, AppResources.Culture)
                                   ?? DetectedContainerKey;
                }
            });
        }
    }
}