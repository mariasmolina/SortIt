using SortIt.Models;
using SortIt.Resources.Localization;
using SortIt.Services;
using SortIt.ViewModels;

namespace SortIt.Views;

public partial class SettingsPage : ContentPage
{
    private readonly AudioService _audio = App.Audio;
    private readonly ProfileViewModel vm = new();
    public SettingsPage()
	{
        InitializeComponent();

        SoundSwitch.IsToggled = _audio.IsEnabled;
        SoundSwitch.Toggled += OnSoundToggled;

        ThemePicker.SelectedIndexChanged += ThemePicker_SelectedIndexChanged;
        SetupThemePicker();

        LanguageService.LanguageChanged += OnLanguageChanged;
    }

    private void SetupThemePicker()
    {
        ThemePicker.ItemsSource = new List<Theme>
        {
            new Theme(
                AppResources.Theme_Light,
                "#CFE6D5", "#FFFBD7",
                Colors.White, "#2E7D32", "#1B5E20",
                "Titillium", "#8BC34A",
                "#66BB6A", "#FFFFFF", "#212121"
            ),
            new Theme(
                AppResources.Theme_Dark,
                "#1E2723", "#71996A",
                Colors.DarkSeaGreen, "#1E3B16", "#FFFFFF",
                "Titillium", "#FFAB00",
                "#66BB6A", "#131A17", "#8F8D8D"
            )
        };

        ThemePicker.SelectedIndex = 0;
    }


    private void OnSoundToggled(object sender, ToggledEventArgs e)
    {
        _audio.SetEnabled(e.Value);
    }

    private void ThemePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ThemePicker.SelectedItem is Theme selectedTheme)
        {
            selectedTheme.Apply(this);
        }
    }

    private void OnTestLevelUpClicked(object sender, EventArgs e)
    {
        App.UserDB.AddXp(300);
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
        int oldIndex = ThemePicker.SelectedIndex;
        SetupThemePicker();

        if (oldIndex >= 0 && oldIndex < ThemePicker.ItemsSource.Count)
            ThemePicker.SelectedIndex = oldIndex;
        else
            ThemePicker.SelectedIndex = 0;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        LanguageService.LanguageChanged -= OnLanguageChanged;
    }
}