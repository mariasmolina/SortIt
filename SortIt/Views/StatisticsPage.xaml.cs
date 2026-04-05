using SortIt.ViewModels;

namespace SortIt.Views;

public partial class StatisticsPage : ContentPage
{
    private readonly StatisticsViewModel _viewModel;

    public StatisticsPage(StatisticsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.Load();
    }

    private async void BackToProfile(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProfilePage());
    }
}