using SortIt.ViewModels;

namespace SortIt.Views
{
    public partial class ProfilePage : ContentPage
    {
        private readonly ProfileViewModel _vm;

        public ProfilePage()
        {
            InitializeComponent();

            _vm = (ProfileViewModel)BindingContext;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Загружает актуальные данные профиля из базы
            _vm.Load();
        }

        // Редактирование профиля
        private async void OnEditProfile(object sender, TappedEventArgs e)
        {
            await Navigation.PushModalAsync(new EditProfilePage());
        }
    }
}