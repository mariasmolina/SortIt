using SortIt.Models;
using SortIt.Resources.Localization;
using SortIt.Services;
using SortIt.ViewModels;

namespace SortIt.Views;

public partial class SettingsPage : ContentPage
{
    private readonly AudioService _audio = App.Audio;
    private readonly ProfileViewModel vm = new();
    private Theme _lightTheme;
    private Theme _darkTheme;
    private bool _isDarkTheme;

    public SettingsPage()
	{
        InitializeComponent();

        SetupThemes();
        ApplyTheme(false, false);

        SoundSwitch.IsToggled = _audio.IsEnabled;
        SoundSwitch.Toggled += OnSoundToggled;

        LanguageService.LanguageChanged += OnLanguageChanged;
    }

    private void SetupThemes()
    {
        _lightTheme = new Theme(
            AppResources.Theme_Light,
            "#CFE6D5", "#FFFBD7",
            Colors.White, "#2E7D32", "#1B5E20",
            "Titillium", "#8BC34A",
            "#66BB6A", "#FFFFFF", "#212121", "#6B7280"
        );

        _darkTheme = new Theme(
            AppResources.Theme_Dark,
            "#1E2723", "#71996A",
            Colors.DarkSeaGreen, "#1E3B16", "#FFFFFF",
            "Titillium", "#FFAB00",
            "#66BB6A", "#131A17", "#8F8D8D", "#FFFFFF"
        );
    }


    private void OnSoundToggled(object sender, ToggledEventArgs e)
    {
        _audio.SetEnabled(e.Value);
    }

    private async void OnThemeToggleTapped(object sender, EventArgs e)
    {
        ApplyTheme(!_isDarkTheme, true);
    }

    private async void ApplyTheme(bool dark, bool animate)
    {
        _isDarkTheme = dark;

        Theme selectedTheme;

        if (dark == true)
        {
            selectedTheme = _darkTheme;
        }
        else
        {
            selectedTheme = _lightTheme;
        }

        selectedTheme.Apply(this);

        if (dark == true)
        {
            Grid.SetColumn(ThemeSelectedBackground, 1);

            LightThemeLabel.TextColor = Color.FromArgb("#333333");
            DarkThemeLabel.TextColor = Color.FromArgb("#1B5E20");
        }
        else
        {
            Grid.SetColumn(ThemeSelectedBackground, 0);

            LightThemeLabel.TextColor = Color.FromArgb("#1B5E20");
            DarkThemeLabel.TextColor = Color.FromArgb("#333333");
        }

        if (animate == true)
        {
            ThemeSelectedBackground.Scale = 0.95;
            await ThemeSelectedBackground.ScaleTo(1, 120, Easing.CubicOut);
        }
    }

    private void OnTestLevelUpClicked(object sender, EventArgs e)
    {
        App.UserDB.AddXp(100);
    }

    private void Exit(object sender, EventArgs e)
    {
    #if ANDROID
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
    #endif
            return;
    }

    private void ResetStats(object sender, EventArgs e)
    {
        vm.ResetStatsCommand.Execute(null);
    }

    private void OnLanguageChanged()
    {
        SetupThemes();

        LightThemeLabel.Text = AppResources.Theme_Light;
        DarkThemeLabel.Text = AppResources.Theme_Dark;

        ApplyTheme(_isDarkTheme, false);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        LanguageService.LanguageChanged -= OnLanguageChanged;
    }
}