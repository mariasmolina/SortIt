using SortIt.Services;

namespace SortIt
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();

            var savedLang = Preferences.Get("AppLanguage", "en");
            LanguageService.ChangeLanguage(savedLang);
        }
    }
}