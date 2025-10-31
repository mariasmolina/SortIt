using SQLite;

namespace SortIt.Models
{
    public class Slide
    {
        public string Image { get; set; } = ""; 
        public string Title { get; set; } = "";  
        public List<string> Items { get; set; } = new();
    }
}
