using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SortIt.Services;
using SortIt.Resources.Localization;

namespace SortIt.ViewModels
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        // Cвойства профиля
        private string _name = "";
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        private string _avatar = "avatar_leaf.png";
        public string Avatar
        {
            get => _avatar;
            set { _avatar = value; OnPropertyChanged(); }
        }

        private string _levelText = "";
        public string LevelText
        {
            get => _levelText;
            set { _levelText = value; OnPropertyChanged(); }
        }

        private string _rankText = "";
        public string RankText
        {
            get => _rankText;
            set { _rankText = value; OnPropertyChanged(); }
        }

        private double _xpProgress;
        public double XpProgress
        {
            get => _xpProgress;
            set { _xpProgress = value; OnPropertyChanged(); }
        }

        private string _xpDisplay = "";
        public string XpDisplay
        {
            get => _xpDisplay;
            set { _xpDisplay = value; OnPropertyChanged(); }
        }

        private string _totalCorrect = "0";
        public string TotalCorrect
        {
            get => _totalCorrect;
            set { _totalCorrect = value; OnPropertyChanged(); }
        }

        private string _totalWrong = "0";
        public string TotalWrong
        {
            get => _totalWrong;
            set { _totalWrong = value; OnPropertyChanged(); }
        }

        private string _plantImage = "plant_rank0_seedling.png";
        public string PlantImage
        {
            get => _plantImage;
            set { _plantImage = value; OnPropertyChanged(); }
        }

        // Сброс статистики
        public ICommand ResetStatsCommand { get; }

        public ProfileViewModel()
        {
            ResetStatsCommand = new Command(ResetStats);
        }

        // Загрузка данных профиля из базы
        public void Load()
        {
            var p = App.UserDB.GetProfile();

            // имя и аватар
            Name = p.Name;
            Avatar = p.Avatar;

            // уровень и ранг
            int lvl = LevelService.GetLevel(p.Xp);
            LevelText = $"{AppResources.Label_Lvl} {lvl}";
            RankText = LevelService.GetRank(lvl);

            // опыт и прогресс
            XpProgress = LevelService.GetProgress(p.Xp);
            XpDisplay = $"{p.Xp} / {LevelService.NextLevelXp(p.Xp)} XP";

            // статистика
            TotalCorrect = p.TotalCorrect.ToString();
            TotalWrong = p.TotalWrong.ToString();

            // дерево
            PlantImage = LevelService.GetRankImage(lvl);
        }

        // Сброс статистики
        private void ResetStats()
        {
            App.UserDB.ResetStats(fullReset: true);
            Load();
        }

        // Вызывает событие изменения свойства
        public event PropertyChangedEventHandler? PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string? propName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}