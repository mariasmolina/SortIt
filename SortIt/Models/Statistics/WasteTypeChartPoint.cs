namespace SortIt.Models.Statistics
{
    // Andmepunkt jäätmeliigi statistika kuvamiseks tulpdiagrammil
    public class WasteTypeChartPoint
    {
        public string Label { get; set; } = "";
        public double Correct { get; set; }
        public double Wrong { get; set; }
    }
}
