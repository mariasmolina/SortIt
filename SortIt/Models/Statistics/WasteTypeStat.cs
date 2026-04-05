using SQLite;

namespace SortIt.Models
{
    /* Hoiab jäätmeliigi(WasteType) lõikes koondstatistikat
    Mitu korda kasutaja vastas selle liigi puhul õigesti ja mitu korda valesti */
    [Table("WasteTypeStats")]
    public class WasteTypeStat
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Unique]
        public string WasteType { get; set; } = string.Empty;

        public int CorrectCount { get; set; }

        public int WrongCount { get; set; }
    }
}