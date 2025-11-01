using SortIt.Resources.Localization;
using SortIt.Services;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace SortIt.ViewModels
{
    public class LanguageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public string SelectedLanguage { get; private set; } =
            Preferences.Get("AppLanguage", CultureInfo.CurrentUICulture.TwoLetterISOLanguageName);
        public string Game_Title => AppResources.Game_Title;
        public string Greeting_Text => AppResources.Greeting_Text;
        public string Guide_Title => AppResources.Guide_Title;
        public string LabelRank_EcoScout => AppResources.LabelRank_EcoScout;
        public string LabelRank_GreenHero => AppResources.LabelRank_GreenHero;
        public string LabelRank_PlanetGuardian => AppResources.LabelRank_PlanetGuardian;
        public string LabelRank_Recycler => AppResources.LabelRank_Recycler;
        public string LabelRank_Seedling => AppResources.LabelRank_Seedling;
        public string LabelStats_Correct => AppResources.LabelStats_Correct;
        public string LabelStats_Wrong => AppResources.LabelStats_Wrong;
        public string Label_EcoLevel => AppResources.Label_EcoLevel;
        public string Label_Lvl => AppResources.Label_Lvl;
        public string Label_Statistics => AppResources.Label_Statistics;
        public string Profile_Title => AppResources.Profile_Title;
        public string Settings_EnableSound => AppResources.Settings_EnableSound;
        public string Settings_Language => AppResources.Settings_Language;
        public string Settings_Theme => AppResources.Settings_Theme;
        public string Settings_Title => AppResources.Settings_Title;
        public string Theme_Dark => AppResources.Theme_Dark;
        public string Theme_Light => AppResources.Theme_Light;
        public string Button_Cancel => AppResources.Button_Cancel;
        public string Button_Save => AppResources.Button_Save;
        public string Label_ChooseAvatar => AppResources.Label_ChooseAvatar;
        public string Label_PlayerName => AppResources.Label_PlayerName;
        public string Start => AppResources.Start;
        public string LvlUp => AppResources.LvlUp;
        public string RoundOver => AppResources.RoundOver;
        public string XPGet => AppResources.XPGet;
        public string DepositBootle => AppResources.DepositBootle;
        public string DepositCan => AppResources.DepositCan;
        public string GlassBootle => AppResources.GlassBootle;
        public string GlassCan => AppResources.GlassCan;
        public string Battery => AppResources.Battery;
        public string Bulb => AppResources.Bulb;
        public string Box => AppResources.Box;
        public string Newspaper => AppResources.Newspaper;
        public string FilmWrapping => AppResources.FilmWrapping;
        public string MetalCan => AppResources.MetalCan;
        public string ReusableMug => AppResources.ReusableMug;
        public string ReusablePlate => AppResources.ReusablePlate;
        public string Napkin => AppResources.Napkin;
        public string MedicalMask => AppResources.MedicalMask;
        public string AppleCore => AppResources.AppleCore;
        public string BananaPeel => AppResources.BananaPeel;
        public string PaperNewspaper => AppResources.PaperNewspaper;
        public string Settings_ChooseTheme => AppResources.Settings_ChooseTheme;
        public string Welcome_Title => AppResources.Welcome_Title;
        public string Welcome_Message => AppResources.Welcome_Message;
        public string Welcome_Start => AppResources.Welcome_Start;
        public string PerfumeBottle => AppResources.PerfumeBottle;
        public string GlassJarLid => AppResources.GlassJarLid;
        public string PaintCan => AppResources.PaintCan;
        public string Thermometer => AppResources.Thermometer;
        public string DepositGlass => AppResources.DepositGlass;
        public string PlasticBottleCap => AppResources.PlasticBottleCap;
        public string PaperBag => AppResources.PaperBag;
        public string CerealBox => AppResources.CerealBox;
        public string PlasticBottle => AppResources.PlasticBottle;
        public string JuiceCarton => AppResources.JuiceCarton;
        public string TinLid => AppResources.TinLid;
        public string ReusableCutlery => AppResources.ReusableCutlery;
        public string ReusableBottle => AppResources.ReusableBottle;
        public string Toothbrush => AppResources.Toothbrush;
        public string CeramicShard => AppResources.CeramicShard;
        public string CigaretteButt => AppResources.CigaretteButt;
        public string TeaBag => AppResources.TeaBag;
        public string VegetablePeels => AppResources.VegetablePeels;
        public string EggShells => AppResources.EggShells;
        public string Magazine => AppResources.Magazine;
        public string OldBook => AppResources.OldBook;
        public string Notebook => AppResources.Notebook;
        public string Envelope => AppResources.Envelope;
        public string Game_Challenge_Title => AppResources.Game_Challenge_Title;
        public string Game_Challenge_Message => AppResources.Game_Challenge_Message;
        public string Exit_Button => AppResources.Exit_Button;
        public string Reset_Button => AppResources.Reset_Button;

        public IEnumerable<string> ThemeOptions => new[] { Theme_Light, Theme_Dark };

        public ICommand SetEnglishCommand { get; }
        public ICommand SetRussianCommand { get; }
        public ICommand SetEstonianCommand { get; }

        public LanguageViewModel()
        {
            SetEnglishCommand = new Command(() => ChangeLanguage("en"));
            SetRussianCommand = new Command(() => ChangeLanguage("ru"));
            SetEstonianCommand = new Command(() => ChangeLanguage("et"));

            LanguageService.LanguageChanged += OnLanguageChanged;
        }

        private void ChangeLanguage(string code)
        {
            SelectedLanguage = code;                     
            OnPropertyChanged(nameof(SelectedLanguage)); 
            LanguageService.ChangeLanguage(code);
        }

        private void OnLanguageChanged()
        {
            OnPropertyChanged(nameof(Game_Title));
            OnPropertyChanged(nameof(Greeting_Text));
            OnPropertyChanged(nameof(Guide_Title));
            OnPropertyChanged(nameof(LabelRank_EcoScout));
            OnPropertyChanged(nameof(LabelRank_GreenHero));
            OnPropertyChanged(nameof(LabelRank_PlanetGuardian));
            OnPropertyChanged(nameof(LabelRank_Recycler));
            OnPropertyChanged(nameof(LabelRank_Seedling));
            OnPropertyChanged(nameof(LabelStats_Correct));
            OnPropertyChanged(nameof(LabelStats_Wrong));
            OnPropertyChanged(nameof(Label_EcoLevel));
            OnPropertyChanged(nameof(Label_Lvl));
            OnPropertyChanged(nameof(Label_Statistics));
            OnPropertyChanged(nameof(Profile_Title));
            OnPropertyChanged(nameof(Settings_EnableSound));
            OnPropertyChanged(nameof(Settings_Language));
            OnPropertyChanged(nameof(Settings_Theme));
            OnPropertyChanged(nameof(Settings_Title));
            OnPropertyChanged(nameof(Theme_Dark));
            OnPropertyChanged(nameof(Theme_Light));
            OnPropertyChanged(nameof(Settings_EnableSound));
            OnPropertyChanged(nameof(ThemeOptions));
            OnPropertyChanged(nameof(Button_Cancel));
            OnPropertyChanged(nameof(Button_Save));
            OnPropertyChanged(nameof(Label_ChooseAvatar));
            OnPropertyChanged(nameof(Label_PlayerName));
            OnPropertyChanged(nameof(Start));
            OnPropertyChanged(nameof(LvlUp));
            OnPropertyChanged(nameof(RoundOver));
            OnPropertyChanged(nameof(XPGet));
            OnPropertyChanged(nameof(DepositBootle));
            OnPropertyChanged(nameof(DepositCan));
            OnPropertyChanged(nameof(GlassBootle));
            OnPropertyChanged(nameof(GlassCan));
            OnPropertyChanged(nameof(Battery));
            OnPropertyChanged(nameof(Bulb));
            OnPropertyChanged(nameof(Box));
            OnPropertyChanged(nameof(Newspaper));
            OnPropertyChanged(nameof(FilmWrapping));
            OnPropertyChanged(nameof(MetalCan));
            OnPropertyChanged(nameof(ReusableMug));
            OnPropertyChanged(nameof(ReusablePlate));
            OnPropertyChanged(nameof(Napkin));
            OnPropertyChanged(nameof(MedicalMask));
            OnPropertyChanged(nameof(AppleCore));
            OnPropertyChanged(nameof(BananaPeel));
            OnPropertyChanged(nameof(PaperNewspaper));
            OnPropertyChanged(nameof(Settings_ChooseTheme));
            OnPropertyChanged(nameof(Welcome_Title));
            OnPropertyChanged(nameof(Welcome_Message));
            OnPropertyChanged(nameof(Welcome_Start));
            OnPropertyChanged(nameof(PerfumeBottle));
            OnPropertyChanged(nameof(GlassJarLid));
            OnPropertyChanged(nameof(PaintCan));
            OnPropertyChanged(nameof(Thermometer));
            OnPropertyChanged(nameof(DepositGlass));
            OnPropertyChanged(nameof(PlasticBottleCap));
            OnPropertyChanged(nameof(PaperBag));
            OnPropertyChanged(nameof(CerealBox));
            OnPropertyChanged(nameof(PlasticBottle));
            OnPropertyChanged(nameof(JuiceCarton));
            OnPropertyChanged(nameof(TinLid));
            OnPropertyChanged(nameof(ReusableCutlery));
            OnPropertyChanged(nameof(ReusableBottle));
            OnPropertyChanged(nameof(Toothbrush));
            OnPropertyChanged(nameof(CeramicShard));
            OnPropertyChanged(nameof(CigaretteButt));
            OnPropertyChanged(nameof(TeaBag));
            OnPropertyChanged(nameof(VegetablePeels));
            OnPropertyChanged(nameof(EggShells));
            OnPropertyChanged(nameof(Magazine));
            OnPropertyChanged(nameof(OldBook));
            OnPropertyChanged(nameof(Notebook));
            OnPropertyChanged(nameof(Envelope));
            OnPropertyChanged(nameof(Game_Challenge_Title));
            OnPropertyChanged(nameof(Game_Challenge_Message));
            OnPropertyChanged(nameof(Exit_Button));
            OnPropertyChanged(nameof(Reset_Button));
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
