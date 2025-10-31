using SQLite;

namespace SortIt.Models
{
    [Table("UserProfile")]
    public class UserProfile
    {
        [PrimaryKey, AutoIncrement] public int Id { get; set; }
        public string Name { get; set; } = "Eco Hero";
        public string Avatar { get; set; } = "avatar_leaf.png";
        public int Xp { get; set; } = 0;
        public int TotalCorrect { get; set; } = 0;
        public int TotalWrong { get; set; } = 0;
    }
}
