using SQLite;

namespace SortIt.Models
{
    //Mängusessiooni kokkuvõtlik statistika, mis salvestatakse lokaalsesse SQLite tabelisse
    [Table("GameSessionStats")]
    public class GameSessionStat
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime PlayedAt { get; set; }

        public int CorrectAnswers { get; set; }

        public int WrongAnswers { get; set; }

        public int GainedXp { get; set; }
    }
}