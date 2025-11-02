using SortIt.Resources.Localization;
using SortIt.Services;
using SortIt.ViewModels;

namespace SortIt.Views
{
    public partial class GamePage : ContentPage
    {
        private GameViewModel vm;

        public GamePage()
        {
            InitializeComponent();
            vm = new GameViewModel();
            BindingContext = vm;

            vm.BinCorrectSelected += BinCorrectSelected;
            vm.BinWrongSelected += BinWrongSelected;
            vm.ScorePunch += ShowScoreAnimation;
            vm.RoundFinished += RoundFinished;
            vm.GameStartedVisual += GameStartedVisual;

            LanguageService.LanguageChanged += OnLanguageChanged;
            Title = AppResources.Game_Title;

            ResetGameScreen();
        }

        // ===== Обработка событий от ViewModel =====
        private void BinCorrectSelected(object? sender, int index)
        {
            if (index == 0)
            {
                CorrectAnswerAnimation(Bin0Host);
            }
            else if (index == 1)
            {
                CorrectAnswerAnimation(Bin1Host);
            }
            else if (index == 2)
            {
                CorrectAnswerAnimation(Bin2Host);
            }
            else if (index == 3)
            {
                CorrectAnswerAnimation(Bin3Host);
            }
        }

        private void BinWrongSelected(object? sender, int index)
        {
            if (index == 0)
            {
                WrongAnswerAnimation(Bin0Host);
            }
            else if (index == 1)
            {
                WrongAnswerAnimation(Bin1Host);
            }
            else if (index == 2)
            {
                WrongAnswerAnimation(Bin2Host);
            }
            else if (index == 3)
            {
                WrongAnswerAnimation(Bin3Host);
            }
        }

        private void RoundFinished(object? sender, (int addXp, bool levelUp, string title, string message) result)
        {
            DisplayAlert(result.title, result.message, "OK");

            ItemName.IsVisible = false;
            TrashBlock.Spacing = 0;
            ItemName.Opacity = 1;
            ItemName.TranslationY = 0;
        }

        private void GameStartedVisual(object? sender, EventArgs e)
        {
            ItemName.IsVisible = true;
            TrashBlock.Spacing = 6;
            ItemName.Opacity = 0;
            ItemName.FadeTo(1, 200, Easing.CubicOut);
        }

        private void ResetGameScreen()
        {
            Bin0Host.BackgroundColor = Colors.White;
            Bin1Host.BackgroundColor = Colors.White;
            Bin2Host.BackgroundColor = Colors.White;
            Bin3Host.BackgroundColor = Colors.White;

            Bin0Host.TranslationX = 0;
            Bin1Host.TranslationX = 0;
            Bin2Host.TranslationX = 0;
            Bin3Host.TranslationX = 0;

            Bin0Host.Scale = 1.0;
            Bin1Host.Scale = 1.0;
            Bin2Host.Scale = 1.0;
            Bin3Host.Scale = 1.0;

            TrashImage.Opacity = 1;
            TrashImage.TranslationY = 0;

            ItemName.IsVisible = false;

            TrashBlock.Spacing = 0;
        }

        // ===== Анимации =====
        private async void ShowScoreAnimation(object? sender, EventArgs e)
        {
            await ScoreLabel.ScaleTo(1.15, 90, Easing.CubicOut);
            await ScoreLabel.ScaleTo(1.00, 90, Easing.CubicIn);
        }

        private async void CorrectAnswerAnimation(Border bin)
        {
            await bin.ScaleXTo(0.0, 120, Easing.CubicIn);
            bin.BackgroundColor = Color.FromArgb("#E7F5ED");
            await bin.ScaleXTo(1.0, 140, Easing.CubicOut);

            await bin.ScaleTo(0.96, 90);
            await bin.ScaleTo(1.00, 90);

            bin.BackgroundColor = Colors.White;
        }

        private async void WrongAnswerAnimation(Border bin)
        {
            bin.BackgroundColor = Color.FromArgb("#FFE9E9");

            Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(35));

            uint dur = 55;
            await bin.TranslateTo(-6, 0, dur);
            await bin.TranslateTo(6, 0, dur);
            await bin.TranslateTo(-4, 0, dur);
            await bin.TranslateTo(4, 0, dur);
            await bin.TranslateTo(0, 0, dur);

            bin.BackgroundColor = Colors.White;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            LanguageService.LanguageChanged -= OnLanguageChanged;

            vm.OnDisappearing();
        }

        private void OnLanguageChanged()
        {
            Title = AppResources.Game_Title;
        }
    }
}