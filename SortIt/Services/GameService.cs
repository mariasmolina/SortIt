using SortIt.Models;
using SortIt.Models.WasteModels;
using SortIt.Resources.Constants;

namespace SortIt.Services
{
    // Mängu loogika ja statistika haldamise teenus
    public class GameService
    {
        // Kõik võimalikud prügikastid ja nende omadused
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

        // Kõik võimalikud prügiobjektid, mis võivad mängus ette tulla, ja nende omadused
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

        // Mängu hetkeseis - aktiivsed prügikastid, praegune sorteeritav objekt, skoor ja statistika
        public List<Bin> ActiveBins { get; private set; } = new();
        public SortableItem? CurrentItem { get; private set; }
        public int Score { get; private set; }
        public int Correct { get; private set; }
        public int Wrong { get; private set; }
        public int SecondsLeft { get; private set; } = 30;
        public bool IsRunning { get; private set; }

        // Käivitab uue mänguringi ja lähtestab mängu statistika
        public void StartRound(int seconds = 30)
        {
            IsRunning = true;
            SecondsLeft = seconds;
            Score = 0;
            Correct = 0;
            Wrong = 0;

            PickBinsForThisTurn();
            PickNextTrashItem();
        }

        // Vähendab taimerit ühe sekundi võrra
        public bool Tick()
        {
            if (!IsRunning) return false;

            SecondsLeft--;

            if (SecondsLeft <= 0)
            {
                IsRunning = false;
                return true;
            }

            return false;
        }

        // Kontrollib, kas mängija valitud prügikast on õige
        public bool CheckAnswer(int tappedIndex)
        {
            if (!IsRunning || CurrentItem == null) return false;
            if (tappedIndex < 0 || tappedIndex >= ActiveBins.Count) return false;

            Bin tappedBin = ActiveBins[tappedIndex];
            bool correctAnswer = tappedBin.Type == CurrentItem.Type;

            if (correctAnswer)
            {
                Score += 3;
                Correct++;
            }
            else
            {
                Score -= 3;
                Wrong++;
            }

            App.UserDB.UpdateWasteTypeStat(CurrentItem.Type.ToString(), correctAnswer);

            PickBinsForThisTurn();
            PickNextTrashItem();

            return correctAnswer;
        }

        // Lõpetab mänguringi, arvutab kogutud kogemuspunktid ja salvestab tulemused andmebaasi
        public RoundResult FinishRound(bool saveResult = true)
        {
            IsRunning = false;

            RoundResult result = new RoundResult
            {
                GainedXp = Score > 0 ? Score : 0
            };

            if (saveResult)
            {
                result.LevelUp = App.UserDB.AddXp(result.GainedXp);
                App.UserDB.AddGameResult(Correct, Wrong);
                App.UserDB.SaveGameSession(Correct, Wrong, result.GainedXp);
            }

            return result;
        }

        // Lähtestab mängu algolekusse
        public void Reset()
        {
            IsRunning = false;
            SecondsLeft = 30;
            Score = 0;
            Correct = 0;
            Wrong = 0;
            CurrentItem = null;
            ActiveBins.Clear();
        }

        // Valib juhuslikult 4 prügikasti, mida selle mänguringi jooksul kasutada
        private void PickBinsForThisTurn()
        {
            ActiveBins = allBins.Values
                .OrderBy(_ => Random.Shared.Next())
                .Take(4)
                .ToList();
        }

        // Valib juhuslikult ühe prügiobjekti, mida mängija peab sorteerima
        private void PickNextTrashItem()
        {
            WasteType chosenType = ActiveBins[Random.Shared.Next(ActiveBins.Count)].Type;
            List<SortableItem> pool = wasteByType[chosenType];
            CurrentItem = pool[Random.Shared.Next(pool.Count)];
        }
    }
}
