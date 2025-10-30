using SortIt.Views;

namespace SortIt.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private void OnStartClicked(object sender, EventArgs e)
    {
        Application.Current!.MainPage = new AppShell();
    }
}