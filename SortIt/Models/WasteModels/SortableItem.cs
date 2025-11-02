namespace SortIt.Models.WasteModels
{
    public class SortableItem
    {
        public string Key { get; init; } = string.Empty; // Localization key
        public string Image { get; init; } = string.Empty;
        public WasteType Type { get; init; }


        public SortableItem(string key, string image, WasteType type)
        {
            Key = key; 
            Image = image; 
            Type = type;
        }
    }
}