using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SortIt.Models;
using SortIt.Models.Statistics;
using SortIt.Resources.Localization;

namespace SortIt.ViewModels
{
    /* Statistika vaate mudel (ViewModel), mis:
    - hoiab KPI tekste (täpsus, vastuste arv jne),
    - valmistab ette andmesarjad graafikutele,
    - laadib/filtreerib sessioonipõhised andmed (aasta/kuu),
    - ning teavitab UI-d muutustest läbi INotifyPropertyChanged */
    public class StatisticsViewModel : INotifyPropertyChanged
    {
        // ---- KPI: Täpsus (Accuracy) ----
        private string _accuracyText = "0%";
        public string AccuracyText
        {
            get { return _accuracyText; }
            set
            {
                _accuracyText = value;
                OnPropertyChanged();
            }
        }

        // ---- KPI: Vastuste koguarv ----
        private string _totalAnswersText = "0";
        public string TotalAnswersText
        {
            get { return _totalAnswersText; }
            set
            {
                _totalAnswersText = value;
                OnPropertyChanged();
            }
        }

        // ---- KPI: Õiged vastused ----
        private string _correctAnswersText = "0";
        public string CorrectAnswersText
        {
            get { return _correctAnswersText; }
            set
            {
                _correctAnswersText = value;
                OnPropertyChanged();
            }
        }

        // ---- KPI: Valed vastused ----
        private string _wrongAnswersText = "0";
        public string WrongAnswersText
        {
            get { return _wrongAnswersText; }
            set
            {
                _wrongAnswersText = value;
                OnPropertyChanged();
            }
        }

        // ---- Progressi graafiku pealkiri ----
        private string _progressTitle = AppResources.ProgressMonthsTitle;
        public string ProgressTitle
        {
            get { return _progressTitle; }
            set
            {
                _progressTitle = value;
                OnPropertyChanged();
            }
        }

        // ---- Filtrid: valitud aasta/kuu ----
        private int _selectedYear;
        public int SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                if (_selectedYear == value)
                    return;

                _selectedYear = value;
                OnPropertyChanged();
                LoadDailyProgress();
            }
        }

        // Valitud kuu nimetus (lokaliseeritud), default: "Kõik kuud".
        private string _selectedMonth = AppResources.AllMonthsText;
        public string SelectedMonth
        {
            get { return _selectedMonth; }
            set
            {
                if (_selectedMonth == value)
                    return;

                _selectedMonth = value;
                OnPropertyChanged();
                LoadDailyProgress();
            }
        }

        // ---- Filtri valikud UI jaoks ----
        public ObservableCollection<int> Years { get; set; } = new();
        public ObservableCollection<string> Months { get; set; } = new();

        public ObservableCollection<ChartPoint> OverallPerformanceSeries { get; set; } = new();
        public ObservableCollection<WasteTypeChartPoint> WasteTypeSeries { get; set; } = new();
        public ObservableCollection<ChartPoint> DailyXpSeries { get; set; } = new();

        // Konstruktor, mis laadib kõik vajalikud andmed vaate jaoks
        public void Load()
        {
            LoadOverallStats();
            LoadWasteTypeStats();
            LoadFilters();
            LoadDailyProgress();
        }

        // Laeb üldised statistilised näitajad (KPI-d) ja valmistab ette andmed üldise soorituse graafikuks
        private void LoadOverallStats()
        {
            var profile = App.UserDB.GetProfile();

            int correct = profile.TotalCorrect;
            int wrong = profile.TotalWrong;
            int total = correct + wrong;

            CorrectAnswersText = correct.ToString();
            WrongAnswersText = wrong.ToString();
            TotalAnswersText = total.ToString();

            if (total == 0)
                AccuracyText = "0%";
            else
                AccuracyText = $"{(double)correct / total * 100:F1}%";

            OverallPerformanceSeries.Clear();

            OverallPerformanceSeries.Add(new ChartPoint
            {
                Label = AppResources.LabelStats_Correct,
                Value = correct
            });

            OverallPerformanceSeries.Add(new ChartPoint
            {
                Label = AppResources.LabelStats_Wrong,
                Value = wrong
            });
        }

        // Laeb jäätmeliikide kaupa statistilised näitajad ja valmistab ette andmed jäätmeliikide soorituse graafikuks
        private void LoadWasteTypeStats()
        {
            WasteTypeSeries.Clear();

            var stats = App.UserDB.GetWasteTypeStats();

            foreach (var stat in stats)
            {
                if (stat.CorrectCount == 0 && stat.WrongCount == 0)
                    continue;

                WasteTypeSeries.Add(new WasteTypeChartPoint
                {
                    Label = GetWasteTypeName(stat.WasteType),
                    Correct = stat.CorrectCount,
                    Wrong = stat.WrongCount
                });
            }
        }

        // Laeb sessioonipõhised andmed, et täita filtrite valikud (aasta/kuu) ja valmistab ette andmed progressi graafikuks vastavalt valitud filtritele
        private void LoadFilters()
        {
            var sessions = App.UserDB.GetGameSessions();

            Years.Clear();
            Months.Clear();

            var yearsFromData = sessions
                .Select(x => x.PlayedAt.Year)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            foreach (var year in yearsFromData)
            {
                Years.Add(year);
            }

            if (Years.Count == 0)
            {
                int currentYear = DateTime.Now.Year;
                Years.Add(currentYear);
                SelectedYear = currentYear;
            }
            else if (!Years.Contains(SelectedYear))
            {
                SelectedYear = Years.Last();
            }

            Months.Add(AppResources.AllMonthsText);
            Months.Add(AppResources.JanuaryText);
            Months.Add(AppResources.FebruaryText);
            Months.Add(AppResources.MarchText);
            Months.Add(AppResources.AprilText);
            Months.Add(AppResources.MayText);
            Months.Add(AppResources.JuneText);
            Months.Add(AppResources.JulyText);
            Months.Add(AppResources.AugustText);
            Months.Add(AppResources.SeptemberText);
            Months.Add(AppResources.OctoberText);
            Months.Add(AppResources.NovemberText);
            Months.Add(AppResources.DecemberText);

            if (string.IsNullOrWhiteSpace(SelectedMonth))
            {
                SelectedMonth = AppResources.AllMonthsText;
            }
        }

        // Laeb sessioonipõhised andmed ja valmistab ette andmed progressi graafikuks vastavalt valitud aastale ja kuule
        private void LoadDailyProgress()
        {
            DailyXpSeries.Clear();

            var sessions = App.UserDB.GetGameSessions()
                .Where(x => x.PlayedAt.Year == SelectedYear)
                .ToList();

            if (SelectedMonth == AppResources.AllMonthsText)
            {
                ProgressTitle = AppResources.ProgressMonthsTitle + $" ({SelectedYear})";

                var groupedByMonth = sessions
                    .GroupBy(x => x.PlayedAt.Month)
                    .OrderBy(x => x.Key)
                    .ToList();

                foreach (var month in groupedByMonth)
                {
                    DailyXpSeries.Add(new ChartPoint
                    {
                        Label = GetShortMonthName(month.Key),
                        Value = month.Sum(x => x.GainedXp)
                    });
                }
            }
            else
            {
                int monthNumber = GetMonthNumber(SelectedMonth);

                ProgressTitle = AppResources.ProgressDaysTitle + $" ({SelectedMonth} {SelectedYear})";

                var groupedByDay = sessions
                    .Where(x => x.PlayedAt.Month == monthNumber)
                    .GroupBy(x => x.PlayedAt.Day)
                    .OrderBy(x => x.Key)
                    .ToList();

                foreach (var day in groupedByDay)
                {
                    DailyXpSeries.Add(new ChartPoint
                    {
                        Label = day.Key.ToString("00"),
                        Value = day.Sum(x => x.GainedXp)
                    });
                }
            }
        }

        // Abimeetod, mis tõlgib jäätmeliigi tüübi nime lokaliseeritud tekstiks
        private string GetWasteTypeName(string type)
        {
            switch (type)
            {
                case "Glass":
                    return AppResources.GlassText;
                case "Hazardous":
                    return AppResources.HazardousText;
                case "Deposit":
                    return AppResources.DepositText;
                case "PaperPackaging":
                    return AppResources.PaperPackagingText;
                case "PMB_Carton":
                    return AppResources.PMB_CartonText;
                case "Reusable":
                    return AppResources.ReusableText;
                case "Mixed":
                    return AppResources.MixedText;
                case "Bio":
                    return AppResources.BioText;
                case "ScrapPaper":
                    return AppResources.ScrapPaperText;
                default:
                    return type;
            }
        }

        // Abimeetod, mis tagastab kuu numbri põhjal selle kuu lühendatud nime (nt "Jan", "Feb" jne) vastavalt kasutaja keelele
        private string GetShortMonthName(int month)
        {
            if (month is < 1 or > 12)
                return string.Empty;

            // Kasutab keelekujulisi lühendit
            return new DateTime(2000, month, 1)
                .ToString("MMM", System.Globalization.CultureInfo.CurrentUICulture);
        }

        // Abimeetod, mis tagastab kuu nime põhjal selle kuu numbri (1-12), toetades lokaliseeritud kuunimesid
        private int GetMonthNumber(string monthName)
        {
            if (string.IsNullOrWhiteSpace(monthName))
                return 0;

            if (string.Equals(monthName, AppResources.JanuaryText, StringComparison.CurrentCulture)) return 1;
            if (string.Equals(monthName, AppResources.FebruaryText, StringComparison.CurrentCulture)) return 2;
            if (string.Equals(monthName, AppResources.MarchText, StringComparison.CurrentCulture)) return 3;
            if (string.Equals(monthName, AppResources.AprilText, StringComparison.CurrentCulture)) return 4;
            if (string.Equals(monthName, AppResources.MayText, StringComparison.CurrentCulture)) return 5;
            if (string.Equals(monthName, AppResources.JuneText, StringComparison.CurrentCulture)) return 6;
            if (string.Equals(monthName, AppResources.JulyText, StringComparison.CurrentCulture)) return 7;
            if (string.Equals(monthName, AppResources.AugustText, StringComparison.CurrentCulture)) return 8;
            if (string.Equals(monthName, AppResources.SeptemberText, StringComparison.CurrentCulture)) return 9;
            if (string.Equals(monthName, AppResources.OctoberText, StringComparison.CurrentCulture)) return 10;
            if (string.Equals(monthName, AppResources.NovemberText, StringComparison.CurrentCulture)) return 11;
            if (string.Equals(monthName, AppResources.DecemberText, StringComparison.CurrentCulture)) return 12;

            return 0;
        }

        // INotifyPropertyChanged implementatsioon, mis võimaldab UI-l reageerida ViewModeli omaduste muutustele
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}