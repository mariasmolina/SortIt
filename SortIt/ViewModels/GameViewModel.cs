using SortIt.Models;
using SortIt.Models.WasteModels;
using SortIt.Resources.Localization;
using SortIt.Services;
using System.ComponentModel;
using System.Windows.Input;

namespace SortIt.ViewModels
{
    public class GameViewModel : INotifyPropertyChanged
    {
        // Библиотека всех урн и всех видов мусора
        private readonly Dictionary<WasteType, Bin> allBins = new()
        {
            [WasteType.Glass] = new Bin { Type = WasteType.Glass, Image = "a_klaaspakend.png" },
            [WasteType.Hazardous] = new Bin { Type = WasteType.Hazardous, Image = "b_ohtlikudjaatmed.png" },
            [WasteType.Deposit] = new Bin { Type = WasteType.Deposit, Image = "c_pandipakend.png" },
            [WasteType.PaperPackaging] = new Bin { Type = WasteType.PaperPackaging, Image = "d_pappjapaberpakend.png" },
            [WasteType.PMB_Carton] = new Bin { Type = WasteType.PMB_Carton, Image = "e_plastmetalljoogikartong.png" },
            [WasteType.Reusable] = new Bin { Type = WasteType.Reusable, Image = "f_ringlusnoud.png" },
            [WasteType.Mixed] = new Bin { Type = WasteType.Mixed, Image = "g_segaolmejaatmed.png" },
            [WasteType.Bio] = new Bin { Type = WasteType.Bio, Image = "h_biojaatmed.png" },
            [WasteType.ScrapPaper] = new Bin { Type = WasteType.ScrapPaper, Image = "h_vanapaber.png" },
        };

        // Библиотека всех видов мусора, разбитых по типам
        private readonly Dictionary<WasteType, List<SortableItem>> wasteByType = new()
        {
            [WasteType.Glass] = new List<SortableItem>
            {
                new SortableItem("GlassBootle", "a_glass_bottle.png", WasteType.Glass),
                new SortableItem("GlassCan", "a_glass_jar.png", WasteType.Glass),
            },
            [WasteType.Hazardous] = new List<SortableItem>
            {
                new SortableItem("Battery", "b_battery.png", WasteType.Hazardous),
                new SortableItem("Bulb", "b_lightbulb.png", WasteType.Hazardous),
            },
            [WasteType.Deposit] = new List<SortableItem>
            {
                new SortableItem("DepositBootle", "c_bottle_deposit.png", WasteType.Deposit),
                new SortableItem("DepositCan", "c_can_deposit.png", WasteType.Deposit),
            },
            [WasteType.PaperPackaging] = new List<SortableItem>
            {
                new SortableItem("Box", "d_box.png", WasteType.PaperPackaging),
                new SortableItem("Newspaper", "d_newspaper.png", WasteType.PaperPackaging),
            },
            [WasteType.PMB_Carton] = new List<SortableItem>
            {
                new SortableItem("FilmWrapping", "e_plastic_wrapper.png", WasteType.PMB_Carton),
                new SortableItem("MetalCan", "e_metal_can.png", WasteType.PMB_Carton),
            },
            [WasteType.Reusable] = new List<SortableItem>
            {
                new SortableItem("ReusableMug", "f_cup.png", WasteType.Reusable),
                new SortableItem("ReusablePlate", "f_plate.png", WasteType.Reusable),
            },
            [WasteType.Mixed] = new List<SortableItem>
            {
                new SortableItem("Napkin", "g_tissue.png", WasteType.Mixed),
                new SortableItem("MedicalMask", "g_mask.png", WasteType.Mixed),
            },
            [WasteType.Bio] = new List<SortableItem>
            {
                new SortableItem("AppleCore", "h_apple_core.png", WasteType.Bio),
                new SortableItem("BananaPeel", "h_banana_peel.png", WasteType.Bio),
            },
            [WasteType.ScrapPaper] = new List<SortableItem>
            {
                new SortableItem("PaperNewspaper", "d_newspaper.png", WasteType.ScrapPaper),
            },
        };

        // активные 4 бака на экране
        private Bin bin0 = new Bin { Image = "idle_bin.png" };
        private Bin bin1 = new Bin { Image = "idle_bin.png" };
        private Bin bin2 = new Bin { Image = "idle_bin.png" };
        private Bin bin3 = new Bin { Image = "idle_bin.png" };

        // текущий предмет, который нужно отсортировать
        private SortableItem? currentItem;

        // очки раунда
        private int score = 0;
        private int correct = 0;
        private int wrong = 0;

        // звук и таймер
        private readonly AudioService audio;
        private IDispatcherTimer? timer;
        private bool isRunning;
        private int secs = 30;

        // текст таймера
        private string timerText = "30";
        public string TimerText
        {
            get => timerText;
            set { timerText = value; OnPropertyChanged("TimerText"); }
        }

        // текст очков
        private string scoreText = "0";
        public string ScoreText
        {
            get => scoreText;
            set { scoreText = value; OnPropertyChanged("ScoreText"); }
        }

        // подпись под мусором
        private string itemName = AppResources.TapStartHint;
        public string ItemName
        {
            get => itemName;
            set { itemName = value; OnPropertyChanged("ItemName"); }
        }

        // картинка мусора в состоянии паузы
        private string trashImageSource = "trash_question.png";
        public string TrashImageSource
        {
            get => trashImageSource;
            set { trashImageSource = value; OnPropertyChanged("TrashImageSource"); }
        }

        // картинки 4 баков в состоянии паузы
        private string bin0Image = "idle_bin.png";
        public string Bin0Image
        {
            get => bin0Image;
            set { bin0Image = value; OnPropertyChanged("Bin0Image"); }
        }

        private string bin1Image = "idle_bin.png";
        public string Bin1Image
        {
            get => bin1Image;
            set { bin1Image = value; OnPropertyChanged("Bin1Image"); }
        }

        private string bin2Image = "idle_bin.png";
        public string Bin2Image
        {
            get => bin2Image;
            set { bin2Image = value; OnPropertyChanged("Bin2Image"); }
        }

        private string bin3Image = "idle_bin.png";
        public string Bin3Image
        {
            get => bin3Image;
            set { bin3Image = value; OnPropertyChanged("Bin3Image"); }
        }

        // команды для View
        public ICommand StartCommand { get; }
        public ICommand TapBin0Command { get; }
        public ICommand TapBin1Command { get; }
        public ICommand TapBin2Command { get; }
        public ICommand TapBin3Command { get; }

        // события для View
        public event EventHandler<int>? BinCorrectSelected;
        public event EventHandler<int>? BinWrongSelected;
        public event EventHandler? ScorePunch;
        public event EventHandler<(int addXp, bool levelUp, string title, string message)>? RoundFinished;
        public event EventHandler? GameStartedVisual;

        public event PropertyChangedEventHandler? PropertyChanged;

        // локализация (ищет по ключу)
        static string T(string key) => AppResources.ResourceManager.GetString(key, AppResources.Culture);

        // ====== КОНСТРУКТОР ======
        public GameViewModel()
        {
            // звук
            audio = App.Audio;
            audio.PrepareSounds();

            // включение звука по настройке
            bool soundOn = Preferences.Get("SoundEnabled", true);
            audio.SetEnabled(soundOn);

            // команды
            StartCommand = new Command(OnStart);
            TapBin0Command = new Command(() => OnBinTapped(0));
            TapBin1Command = new Command(() => OnBinTapped(1));
            TapBin2Command = new Command(() => OnBinTapped(2));
            TapBin3Command = new Command(() => OnBinTapped(3));

            // смена языка
            LanguageService.LanguageChanged += OnLanguageChanged;

            // стартовое состояние (до игры)
            ShowPauseGameScreen();
        }


        // ====== ЛОГИКА ИГРЫ ======

        // старт нового раунда
        private void OnStart()
        {
            isRunning = true;

            secs = 30;
            TimerText = secs.ToString();

            score = 0;
            correct = 0;
            wrong = 0;

            // выбираем новые 4 бака
            PickBinsForThisTurn();

            // выбираем первый предмет
            PickNextTrashItem();

            // передаем View событие, чтобы запустить анимацию появления
            GameStartedVisual?.Invoke(this, EventArgs.Empty);

            // запускаем таймер
            timer?.Stop();
            timer = Application.Current.Dispatcher.CreateTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (_, __) =>
            {
                secs--;
                TimerText = secs.ToString();
                if (secs <= 0)
                {
                    timer.Stop();
                    EndRound();
                }
            };
            timer.Start();

            // обновление экрана перед стартом
            UpdateScreenFromGame();
        }

        // окончание раунда
        private void EndRound(bool fromNavigation = false)
        {
            isRunning = false;
            timer?.Stop();

            if (!fromNavigation)
            {
                // подсчёт результата + XP
                RoundResult result = FinishRound();

                RoundFinished?.Invoke(
                    this,
                    (result.GainedXp, result.LevelUp, result.Title, result.Message)
                );
            }

            // состояние паузы
            ShowPauseGameScreen();
        }

        // когда пользователь тапнул по баку
        private void OnBinTapped(int index)
        {
            if (!isRunning) return;
            if (timer == null || !timer.IsRunning) return;

            bool wasCorrect = CheckAnswer(index);

            if (wasCorrect)
            {
                audio.PlayCorrect();

                BinCorrectSelected?.Invoke(this, index);
                ScorePunch?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                audio.PlayWrong();

                BinWrongSelected?.Invoke(this, index);
            }

            UpdateScreenFromGame();
        }

        // выбираем 4 случайных бака для текущего хода
        private void PickBinsForThisTurn()
        {
            // берём все баки
            var allList = allBins.Values.ToList();

            // перемешиваем
            var randomBin = allList
                .OrderBy(_ => Random.Shared.Next()) // случайный порядок
                .Take(4) // берём первые 4
                .ToList(); // в список

            bin0 = randomBin[0];
            bin1 = randomBin[1];
            bin2 = randomBin[2];
            bin3 = randomBin[3];
        }

        // выбираем новый предмет мусора, который надо сортировать
        private void PickNextTrashItem()
        {
            // случайно выбираем один из 4 активных баков
            int pick = Random.Shared.Next(4);
            WasteType chosenType = bin0.Type;

            if (pick == 1) chosenType = bin1.Type;
            else if (pick == 2) chosenType = bin2.Type;
            else if (pick == 3) chosenType = bin3.Type;

            // берём пул предметов для этого типа мусора
            var pool = wasteByType[chosenType];

            // берём случайный предмет из пула
            int itemIndex = Random.Shared.Next(pool.Count);
            currentItem = pool[itemIndex];
        }

        // проверяем, правильно ли игрок выбрал бак
        private bool CheckAnswer(int tappedIndex)
        {
            if (currentItem == null)
            {
                return false;
            }

            // какой бак он нажал
            Bin tappedBin = new Bin();
            if (tappedIndex == 0)
            {
                tappedBin = bin0;
            }
            else if (tappedIndex == 1)
            {
                tappedBin = bin1;
            }
            else if (tappedIndex == 2)
            {
                tappedBin = bin2;
            }
            else
            {
                tappedBin = bin3;
            }

            bool correctAnswer = false;
            if (tappedBin.Type == currentItem.Type)
            {
                correctAnswer = true;
                score += 3;
                correct++;
            }
            else
            {
                score -= 3;
                wrong++;
            }

            // готовим следующие баки и следующий предмет
            PickBinsForThisTurn();
            PickNextTrashItem();

            return correctAnswer;
        }

        // итоги раунда
        private RoundResult FinishRound()
        {
            RoundResult result = new RoundResult();

            // опыт не может быть отрицательным
            if (score > 0)
            {
                result.GainedXp = score;
            }
            else
            {
                result.GainedXp = 0;
            }

            // добавляем опыт пользователю
            bool leveled = App.UserDB.AddXp(result.GainedXp);
            App.UserDB.AddGameResult(correct, wrong);

            result.LevelUp = leveled;

            // текст уведомления
            result.Title = AppResources.RoundOver;
            result.Message = AppResources.XPGet + ": +" + result.GainedXp;

            if (result.LevelUp)
            {
                result.Message += "\n" + AppResources.LvlUp;
            }

            return result;
        }

        // перерисовывает экран, чтобы картинки и подписи совпадали с текущим состоянием игры
        private void UpdateScreenFromGame()
        {
            // картинки 4 баков
            Bin0Image = bin0.Image;
            Bin1Image = bin1.Image;
            Bin2Image = bin2.Image;
            Bin3Image = bin3.Image;

            // какой мусор сейчас в центре
            if (currentItem != null)
            {
                TrashImageSource = currentItem.Image;
                ItemName = T(currentItem.Key);
            }

            // счёт
            ScoreText = score.ToString();
        }

        // состояние паузы / до старта
        public void ShowPauseGameScreen()
        {
            isRunning = false;
            timer?.Stop();
            timer = null;

            Bin0Image = "idle_bin.png";
            Bin1Image = "idle_bin.png";
            Bin2Image = "idle_bin.png";
            Bin3Image = "idle_bin.png";

            TrashImageSource = "trash_question.png";
            ItemName = AppResources.TapStartHint;

            TimerText = "30";
            ScoreText = "0";
        }

        // при уходе со страницы
        public void OnDisappearing()
        {
            LanguageService.LanguageChanged -= OnLanguageChanged;

            if (isRunning)
            {
                // если игра была запущена, завершаем раунд
                EndRound(fromNavigation: true);
            }
        }

        // локализация
        private void OnLanguageChanged()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (isRunning && currentItem != null)
                {
                    ItemName = T(currentItem.Key);
                }
                else
                {
                    ItemName = AppResources.TapStartHint;
                }
            });
        }

        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
