using SortIt.Resources.Constants;

namespace SortIt.Views
{
    public partial class EditProfilePage : ContentPage
    {
        private string _selectedAvatar = ImageResources.avatar_leaf;

        public EditProfilePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var profile = App.UserDB.GetProfile();
            // заполняет текущее имя пользователя
            NameEntry.Text = profile.Name;
            // текущий аватар
            _selectedAvatar = profile.Avatar;
            HighlightSelected(_selectedAvatar);
        }

        void OnPickAvatar(object sender, TappedEventArgs e) 
        {
            if (e.Parameter is string avatarFile) // CommandParameter
            {
                _selectedAvatar = avatarFile;
                HighlightSelected(avatarFile);
            }
        }

        // Подсветка выбранного аватара
        void HighlightSelected(string avatarFile)
        {
            // сбрасывает обводку у всех
            A1.StrokeThickness = 0;
            A2.StrokeThickness = 0;
            A3.StrokeThickness = 0;
            A4.StrokeThickness = 0;

            // обводка только у выбранного аватара
            if (avatarFile == ImageResources.avatar_leaf)
            {
                A1.StrokeThickness = 3;
            }
            else if (avatarFile == ImageResources.avatar_earth)
            {
                A2.StrokeThickness = 3;
            }
            else if (avatarFile == ImageResources.avatar_panda)
            {
                A3.StrokeThickness = 3;
            }
            else if (avatarFile == ImageResources.avatar_drop)
            {
                A4.StrokeThickness = 3;
            }
        }

        // Сохранить
        async void OnSave(object sender, EventArgs e)
        {
            string newName = "Eco Hero";
            if (NameEntry.Text != null)
            {
                newName = NameEntry.Text;
            }
            // сохраняет имя и аватар в базу
            App.UserDB.SetName(newName);
            App.UserDB.SetAvatar(_selectedAvatar);

            await Navigation.PopModalAsync();
        }

        // Отмена
        async void OnCancel(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}