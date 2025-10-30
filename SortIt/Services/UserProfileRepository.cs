using SQLite;
using SortIt.Models;

namespace SortIt.Services
{
    public class UserProfileRepository
    {
        private readonly SQLiteConnection _db;

        public UserProfileRepository(string dbPath)
        {
            _db = new SQLiteConnection(dbPath);

            _db.CreateTable<UserProfile>();
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
            }

            _db.Update(p);
        }
    }
}