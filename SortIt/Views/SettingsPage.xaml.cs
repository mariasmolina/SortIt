using SortIt.Models;
using SortIt.Resources.Localization;
using SortIt.Services;
using SortIt.ViewModels;

namespace SortIt.Views;

public partial class SettingsPage : ContentPage
{
    private readonly AudioService _audio;
    private readonly ProfileViewModel vm = new();
    public SettingsPage(AudioService audio)
	{
        InitializeComponent();
        _audio = audio;

        SoundSwitch.IsToggled = _audio.IsEnabled;
        SoundSwitch.Toggled += OnSoundToggled;

        ThemePicker.ItemsSource = new List<Theme>
        {
            new Theme
            (
                $"{AppResources.Theme_Light}", "#CFE6D5", "#FFFBD7", 
                Colors.White, "#2E7D32", "#1B5E20", 
                "Titillium", "#8BC34A",
                "#66BB6A", "#FFFFFF", "#212121"
            ),
            new Theme
            (
                $"{AppResources.Theme_Dark}", "#1E2723", "#71996A",
                Colors.DarkSeaGreen, "#1E3B16", "#FFFFFF",
                "Titillium", "#FFAB00",
                "#66BB6A", "#131A17", "#8F8D8D"
            )
        };
        ThemePicker.SelectedIndexChanged += ThemePicker_SelectedIndexChanged;
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
}