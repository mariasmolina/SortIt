using SortIt.Services;
using Microsoft.Maui.Storage;

namespace SortIt
{
    public partial class App : Application
    {
        public static AudioService Audio { get; } = new AudioService();

        public const string DatabaseName = "sortit.db3";

        private static UserProfileRepository _userDb;
        public static UserProfileRepository UserDB
        {
            get
            {
                if (_userDb == null)
                {
                    string path = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        DatabaseName);

                    Directory.CreateDirectory(Path.GetDirectoryName(path)!);

                    _userDb = new UserProfileRepository(path);
                }

                return _userDb;
            }
        }

        public App()
        {
            InitializeComponent();

            var savedLang = Preferences.Get("AppLanguage", "en");
            LanguageService.ChangeLanguage(savedLang);

            // Проверка первого запуска
            Preferences.Remove("HasLaunched");
            var firstRun = !Preferences.Get("HasLaunched", false);
            if (firstRun)
            {
                Preferences.Set("HasLaunched", true);
                MainPage = new NavigationPage(new SortIt.Views.MainPage());
            }
            else
            {
                MainPage = new AppShell();
            }
        }
    }
}