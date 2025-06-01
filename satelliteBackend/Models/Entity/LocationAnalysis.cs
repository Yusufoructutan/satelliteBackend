public class LocationAnalysis
{
    public int Id { get; set; }
    public int LocationId { get; set; }
    public DateTime AnalysisDate { get; set; }
    public double AverageNDVI { get; set; }
    public string Comment { get; set; }
    public string DetailsJson { get; set; }

    public string Recommendation { get; set; } // ← Eksik olan bu


    public Location Location { get; set; }
}
