using SortIt.Services;
using SortIt.ViewModels;

namespace SortIt.Views;

public partial class GuidePage : ContentPage
{
    private readonly GuideViewModel _vm = new();
    public GuidePage()
	{
        InitializeComponent();
        BindingContext = _vm;
        LanguageService.LanguageChanged += OnLanguageChanged;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _vm.LoadSlides();
    }

    // Обновляет слайды при изменении языка
    private void OnLanguageChanged()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            _vm.LoadSlides();
        });
    }
}