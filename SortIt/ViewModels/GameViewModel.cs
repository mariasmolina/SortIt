using SortIt.Models;
using SortIt.Models.WasteModels;
using SortIt.Resources.Constants;
using SortIt.Resources.Localization;
using SortIt.Services;
using System.ComponentModel;
using System.Windows.Input;

namespace SortIt.ViewModels
{
    // Mänguvaate mudel, mis haldab mängu olekut, kasutajaliidest ja suhtlust teenustega
    public class GameViewModel : INotifyPropertyChanged
    {
        private readonly GameService gameService; // Teenus mänguloogika haldamiseks
        private readonly AudioService audio; // Teenus heliefektide esitamiseks
        private IDispatcherTimer? timer; // Mängu taimer vooru kestuse jälgimiseks

        public ICommand StartCommand { get; }
        public ICommand TapBin0Command { get; }
        public ICommand TapBin1Command { get; }
        public ICommand TapBin2Command { get; }
        public ICommand TapBin3Command { get; }

        // Sündmused, mida vaade saab kuulata mängu erinevate sündmuste puhul
        public event EventHandler<int>? BinCorrectSelected;
        public event EventHandler<int>? BinWrongSelected;
        public event EventHandler? ScorePunch;
        public event EventHandler<(int addXp, bool levelUp, string title, string message)>? RoundFinished;
        public event EventHandler? GameStartedVisual;
        public event PropertyChangedEventHandler? PropertyChanged;

        private string timerText = "30";
        public string TimerText { get => timerText; set { timerText = value; OnPropertyChanged(nameof(TimerText)); } }

        private string scoreText = "0";
        public string ScoreText { get => scoreText; set { scoreText = value; OnPropertyChanged(nameof(ScoreText)); } }

        private string itemName = AppResources.TapStartHint;
        public string ItemName { get => itemName; set { itemName = value; OnPropertyChanged(nameof(ItemName)); } }

        private string trashImageSource = ImageResources.trash_question;
        public string TrashImageSource { get => trashImageSource; set { trashImageSource = value; OnPropertyChanged(nameof(TrashImageSource)); } }

        private string bin0Image = ImageResources.idle_bin;
        public string Bin0Image { get => bin0Image; set { bin0Image = value; OnPropertyChanged(nameof(Bin0Image)); } }

        private string bin1Image = ImageResources.idle_bin;
        public string Bin1Image { get => bin1Image; set { bin1Image = value; OnPropertyChanged(nameof(Bin1Image)); } }

        private string bin2Image = ImageResources.idle_bin;
        public string Bin2Image { get => bin2Image; set { bin2Image = value; OnPropertyChanged(nameof(Bin2Image)); } }

        private string bin3Image = ImageResources.idle_bin;
        public string Bin3Image { get => bin3Image; set { bin3Image = value; OnPropertyChanged(nameof(Bin3Image)); } }

        // Initsialiseerib mänguvaate mudeli, vajalikud teenused ja käsud
        public GameViewModel()
        {
            gameService = new GameService();

            audio = App.Audio;
            audio.PrepareSounds();
            audio.SetEnabled(Preferences.Get("SoundEnabled", true));

            StartCommand = new Command(OnStart);
            TapBin0Command = new Command(() => OnBinTapped(0));
            TapBin1Command = new Command(() => OnBinTapped(1));
            TapBin2Command = new Command(() => OnBinTapped(2));
            TapBin3Command = new Command(() => OnBinTapped(3));

            LanguageService.LanguageChanged += OnLanguageChanged;
            ShowPauseGameScreen();
        }

        // Käivitab uue mänguringi ja alustab taimerit
        private void OnStart()
        {
            gameService.StartRound(30);
            GameStartedVisual?.Invoke(this, EventArgs.Empty);

            timer?.Stop();
            timer = Application.Current.Dispatcher.CreateTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (_, __) =>
            {
                bool timeIsOver = gameService.Tick();
                TimerText = gameService.SecondsLeft.ToString();

                if (timeIsOver)
                {
                    timer.Stop();
                    EndRound();
                }
            };
            timer.Start();

            UpdateScreenFromGame();
        }

        // Lõpetab mänguringi ja salvestab tulemused vajadusel andmebaasi
        private void EndRound(bool fromNavigation = false)
        {
            timer?.Stop();

            if (!fromNavigation)
            {
                RoundResult result = gameService.FinishRound();
                result.Title = AppResources.RoundOver;
                result.Message = AppResources.XPGet + ": +" + result.GainedXp;

                if (result.LevelUp)
                {
                    result.Message += "\n" + AppResources.LvlUp;
                }

                RoundFinished?.Invoke(this, (result.GainedXp, result.LevelUp, result.Title, result.Message));
            }
            else
            {
                gameService.FinishRound(saveResult: false);
            }

            ShowPauseGameScreen();
        }

        // Töötleb mängija prügikasti valiku ja kontrollib vastuse õigsust
        private void OnBinTapped(int index)
        {
            if (!gameService.IsRunning) return;
            if (timer == null || !timer.IsRunning) return;

            bool wasCorrect = gameService.CheckAnswer(index);

            if (wasCorrect)
            {
                audio.PlayCorrect();
                BinCorrectSelected?.Invoke(this, index);
                ScorePunch?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                audio.PlayWrong();
                BinWrongSelected?.Invoke(this, index);
            }

            UpdateScreenFromGame();
        }

        // Värskendab kasutajaliidese andmed vastavalt mängu hetkeseisule
        private void UpdateScreenFromGame()
        {
            List<Bin> bins = gameService.ActiveBins;

            if (bins.Count == 4)
            {
                Bin0Image = T(bins[0].Image);
                Bin1Image = T(bins[1].Image);
                Bin2Image = T(bins[2].Image);
                Bin3Image = T(bins[3].Image);
            }

            SortableItem? currentItem = gameService.CurrentItem;
            if (currentItem != null)
            {
                TrashImageSource = currentItem.Image;
                ItemName = T(currentItem.Key);
            }

            ScoreText = gameService.Score.ToString();
            TimerText = gameService.SecondsLeft.ToString();
        }

        // Kuvab pausiseisundi ning lähtestab mänguvaate algolekusse
        public void ShowPauseGameScreen()
        {
            timer?.Stop();
            timer = null;
            gameService.Reset();

            Bin0Image = ImageResources.idle_bin;
            Bin1Image = ImageResources.idle_bin;
            Bin2Image = ImageResources.idle_bin;
            Bin3Image = ImageResources.idle_bin;
            TrashImageSource = ImageResources.trash_question;
            ItemName = AppResources.TapStartHint;
            TimerText = "30";
            ScoreText = "0";
        }

        // Käivitatakse vaate sulgemisel või lehelt lahkumisel
        public void OnDisappearing()
        {
            LanguageService.LanguageChanged -= OnLanguageChanged;

            if (gameService.IsRunning)
            {
                EndRound(fromNavigation: true);
            }
        }

        // Värskendab kasutajaliidest keele muutmisel
        private void OnLanguageChanged()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (gameService.IsRunning && gameService.CurrentItem != null)
                {
                    UpdateScreenFromGame();
                }
                else
                {
                    ItemName = AppResources.TapStartHint;
                }
            });
        }

        // Tagastab lokaliseeritud teksti vastavalt ressursivõtmele
        private static string T(string key)
        {
            return AppResources.ResourceManager.GetString(key, AppResources.Culture) ?? key;
        }

        // Teavitab kasutajaliidest omaduse muutumisest
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
