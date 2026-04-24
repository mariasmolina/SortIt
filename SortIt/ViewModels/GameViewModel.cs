using SortIt.Models;
using SortIt.Models.WasteModels;
using SortIt.Resources.Localization;
using SortIt.Services;
using System.ComponentModel;
using System.Windows.Input;
using SortIt.Resources.Constants;

namespace SortIt.ViewModels
{
    public class GameViewModel : INotifyPropertyChanged
    {
        // Вспомогательная функция для получения картинки по ключу
        static string BinImage(string keyOrFile) => T(keyOrFile);

        // Библиотека всех урн и всех видов мусора
        private readonly Dictionary<WasteType, Bin> allBins = new()
        {
            [WasteType.Glass] = new Bin { Type = WasteType.Glass, Image = "a_klaaspakend" },
            [WasteType.Hazardous] = new Bin { Type = WasteType.Hazardous, Image = "b_ohtlikudjaatmed" },
            [WasteType.Deposit] = new Bin { Type = WasteType.Deposit, Image = "c_pandipakend" },
            [WasteType.PaperPackaging] = new Bin { Type = WasteType.PaperPackaging, Image = "d_pappjapaberpakend" },
            [WasteType.PMB_Carton] = new Bin { Type = WasteType.PMB_Carton, Image = "e_plastmetalljoogikartong" },
            [WasteType.Reusable] = new Bin { Type = WasteType.Reusable, Image = "f_ringlusnoud" },
            [WasteType.Mixed] = new Bin { Type = WasteType.Mixed, Image = "g_segaolmejaatmed" },
            [WasteType.Bio] = new Bin { Type = WasteType.Bio, Image = "h_biojaatmed" },
            [WasteType.ScrapPaper] = new Bin { Type = WasteType.ScrapPaper, Image = "h_vanapaber" },
        };

        // Библиотека всех видов мусора, разбитых по типам
        private readonly Dictionary<WasteType, List<SortableItem>> wasteByType = new()
        {
            [WasteType.Glass] = new List<SortableItem>
            {
                new SortableItem("GlassBootle", ImageResources.a_glass_bottle, WasteType.Glass),
                new SortableItem("GlassCan", ImageResources.a_glass_jar, WasteType.Glass),
            },
            [WasteType.Hazardous] = new List<SortableItem>
            {
                new SortableItem("Battery", ImageResources.b_battery, WasteType.Hazardous),
                new SortableItem("Bulb", ImageResources.b_lightbulb, WasteType.Hazardous),
            },
            [WasteType.Deposit] = new List<SortableItem>
            {
                new SortableItem("DepositBootle", ImageResources.c_bottle_deposit, WasteType.Deposit),
                new SortableItem("DepositCan", ImageResources.c_can_deposit, WasteType.Deposit),
            },
            [WasteType.PaperPackaging] = new List<SortableItem>
            {
                new SortableItem("Box", ImageResources.d_box, WasteType.PaperPackaging),
                new SortableItem("Newspaper", ImageResources.d_newspaper, WasteType.PaperPackaging),
            },
            [WasteType.PMB_Carton] = new List<SortableItem>
            {
                new SortableItem("FilmWrapping", ImageResources.e_plastic_wrapper, WasteType.PMB_Carton),
                new SortableItem("MetalCan", ImageResources.e_metal_can, WasteType.PMB_Carton),
            },
            [WasteType.Reusable] = new List<SortableItem>
            {
                new SortableItem("ReusableMug", ImageResources.f_cup, WasteType.Reusable),
                new SortableItem("ReusablePlate", ImageResources.f_plate, WasteType.Reusable),
            },
            [WasteType.Mixed] = new List<SortableItem>
            {
                new SortableItem("Napkin", ImageResources.g_tissue, WasteType.Mixed),
                new SortableItem("MedicalMask", ImageResources.g_mask, WasteType.Mixed),
            },
            [WasteType.Bio] = new List<SortableItem>
            {
                new SortableItem("AppleCore", ImageResources.h_apple_core, WasteType.Bio),
                new SortableItem("BananaPeel", ImageResources.h_banana_peel, WasteType.Bio),
            },
            [WasteType.ScrapPaper] = new List<SortableItem>
            {
                new SortableItem("PaperNewspaper", ImageResources.d_newspaper, WasteType.ScrapPaper),
            },
        };

        // активные 4 бака на экране
        private Bin bin0 = new Bin { Image = ImageResources.idle_bin };
        private Bin bin1 = new Bin { Image = ImageResources.idle_bin };
        private Bin bin2 = new Bin { Image = ImageResources.idle_bin };
        private Bin bin3 = new Bin { Image = ImageResources.idle_bin };

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
        private string trashImageSource = ImageResources.trash_question;
        public string TrashImageSource
        {
            get => trashImageSource;
            set { trashImageSource = value; OnPropertyChanged("TrashImageSource"); }
        }

        // картинки 4 баков в состоянии паузы
        private string bin0Image = ImageResources.idle_bin;
        public string Bin0Image
        {
            get => bin0Image;
            set { bin0Image = value; OnPropertyChanged("Bin0Image"); }
        }

        private string bin1Image = ImageResources.idle_bin;
        public string Bin1Image
        {
            get => bin1Image;
            set { bin1Image = value; OnPropertyChanged("Bin1Image"); }
        }

        private string bin2Image = ImageResources.idle_bin;
        public string Bin2Image
        {
            get => bin2Image;
            set { bin2Image = value; OnPropertyChanged("Bin2Image"); }
        }

        private string bin3Image = ImageResources.idle_bin;
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

            App.UserDB.UpdateWasteTypeStat(currentItem.Type.ToString(), correctAnswer);

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
            App.UserDB.SaveGameSession(correct, wrong, result.GainedXp);

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
            Bin0Image = BinImage(bin0.Image);
            Bin1Image = BinImage(bin1.Image);
            Bin2Image = BinImage(bin2.Image);
            Bin3Image = BinImage(bin3.Image);

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

            Bin0Image = ImageResources.idle_bin;
            Bin1Image = ImageResources.idle_bin;
            Bin2Image = ImageResources.idle_bin;
            Bin3Image = ImageResources.idle_bin;

            TrashImageSource = ImageResources.trash_question;
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
                UpdateScreenFromGame();
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
