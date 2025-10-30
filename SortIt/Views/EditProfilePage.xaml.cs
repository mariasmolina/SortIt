namespace SortIt.Views
{
    public partial class EditProfilePage : ContentPage
    {
        private string _selectedAvatar = "avatar_leaf.png";

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
            switch (avatarFile)
            {
                case "avatar_leaf.png":
                    A1.StrokeThickness = 3;
                    break;

                case "avatar_earth.png":
                    A2.StrokeThickness = 3;
                    break;

                case "avatar_panda.png":
                    A3.StrokeThickness = 3;
                    break;

                case "avatar_drop.png":
                    A4.StrokeThickness = 3;
                    break;
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