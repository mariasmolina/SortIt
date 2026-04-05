using SQLite;
using SortIt.Models;

namespace SortIt.Services
{
    public class DatabaseService
    {
        private readonly SQLiteConnection _db;

        public DatabaseService(string dbPath)
        {
            _db = new SQLiteConnection(dbPath);

            _db.CreateTable<UserProfile>();
            _db.CreateTable<GameSessionStat>();
            _db.CreateTable<WasteTypeStat>();
        }

        public UserProfile GetProfile()
        {
            var profile = _db.Table<UserProfile>().FirstOrDefault();
            if (profile == null)
            {
                profile = new UserProfile();
                _db.Insert(profile);
            }
            return profile;
        }

        public void SaveProfile(UserProfile profile)
        {
            _db.Update(profile);
        }

        public void SetName(string newName)
        {
            var p = GetProfile();
            p.Name = string.IsNullOrWhiteSpace(newName) ? "Eco Hero" : newName.Trim();
            _db.Update(p);
        }

        public void SetAvatar(string fileName)
        {
            var p = GetProfile();
            p.Avatar = fileName;
            _db.Update(p);
        }

        public bool AddXp(int amount)
        {
            if (amount <= 0)
                return false;

            var p = GetProfile();

            int before = LevelService.GetLevel(p.Xp);

            p.Xp += amount;
            _db.Update(p);

            int after = LevelService.GetLevel(p.Xp);

            return after > before;
        }

        public void AddGameResult(int correct, int wrong)
        {
            if (correct < 0) correct = 0;
            if (wrong < 0) wrong = 0;

            var p = GetProfile();

            p.TotalCorrect += correct;
            p.TotalWrong += wrong;

            _db.Update(p);
        }

        public (int correct, int wrong) GetTotals()
        {
            var p = GetProfile();
            return (p.TotalCorrect, p.TotalWrong);
        }

        public void ResetStats(bool fullReset = false)
        {
            var p = GetProfile();

            p.TotalCorrect = 0;
            p.TotalWrong = 0;

            if (fullReset)
            {
                p.Xp = 0;
                _db.DeleteAll<GameSessionStat>();
            }

            _db.Update(p);
        }

        // Salvestab mängusessiooni statistika, mida saab hiljem vaadata laiendatud statistikast
        public void SaveGameSession(int correctAnswers, int wrongAnswers, int gainedXp)
        {
            var session = new GameSessionStat
            {
                PlayedAt = DateTime.Now,
                CorrectAnswers = correctAnswers,
                WrongAnswers = wrongAnswers,
                GainedXp = gainedXp
            };

            _db.Insert(session);
        }

        // Uuendab konkreetse jäätmeliigi statistikat, mida saab hiljem vaadata laiendatud statistikast
        public void UpdateWasteTypeStat(string wasteType, bool isCorrect)
        {
            if (string.IsNullOrWhiteSpace(wasteType))
                return;

            var stat = _db.Table<WasteTypeStat>()
                          .FirstOrDefault(x => x.WasteType == wasteType);

            if (stat == null)
            {
                stat = new WasteTypeStat
                {
                    WasteType = wasteType,
                    CorrectCount = 0,
                    WrongCount = 0
                };
                _db.Insert(stat);
            }

            if (isCorrect)
                stat.CorrectCount++;
            else
                stat.WrongCount++;

            _db.Update(stat);
        }

        // Tagastab kõik jäätmeliikide statistika kirjed, mida saab kuvada laiendatud statistikavaates
        public List<WasteTypeStat> GetWasteTypeStats()
        {
            return _db.Table<WasteTypeStat>()
                      .OrderBy(x => x.WasteType)
                      .ToList();
        }

        // Tagastab kõik mängusessioonide statistika kirjed, mida saab kuvada laiendatud statistikavaates
        public List<GameSessionStat> GetGameSessions()
        {
            return _db.Table<GameSessionStat>()
                      .OrderBy(x => x.PlayedAt)
                      .ToList();
        }

        // Kustutab kõik laiendatud statistika kirjed, kuid jätab kasutajaprofiili puutumatuks
        public void ResetExtendedStatistics()
        {
            _db.DeleteAll<GameSessionStat>();
            _db.DeleteAll<WasteTypeStat>();
        }
    }
}