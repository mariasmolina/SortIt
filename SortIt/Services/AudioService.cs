using Plugin.Maui.Audio;

namespace SortIt.Services;

public sealed class AudioService
{
    private readonly IAudioManager audioManager = AudioManager.Current;
    private IAudioPlayer? okSound;
    private IAudioPlayer? errorSound;
    private bool _isEnabled = true;

    public async Task PrepareSounds()
    {
        okSound = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("success.mp3"));
        errorSound = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("error.mp3"));
        okSound.Volume = 1.0;
        errorSound.Volume = 1.0;
    }

    public void SetEnabled(bool enabled)
    {
        _isEnabled = enabled;
    }

    public bool IsEnabled => _isEnabled;

    public void PlayCorrect()
    {
        if (!_isEnabled || okSound is null) return;
        okSound.Stop();
        okSound.Play();
    }

    public void PlayWrong()
    {
        if (!_isEnabled || errorSound is null) return;
        errorSound.Stop();
        errorSound.Play();
    }
}