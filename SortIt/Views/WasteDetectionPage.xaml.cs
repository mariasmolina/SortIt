using SortIt.ViewModels;

namespace SortIt.Views;

public partial class WasteDetectionPage : ContentPage
{
    public WasteDetectionPage(WasteDetectionViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}