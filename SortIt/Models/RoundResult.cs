namespace SortIt.Models
{
    public class RoundResult
    {
        public int GainedXp { get; set; }
        public bool LevelUp { get; set; }
        public string Title { get; set; } = "";
        public string Message { get; set; } = "";
    }
}