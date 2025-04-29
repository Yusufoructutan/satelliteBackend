public class DailyData
{
    public List<string> Time { get; set; }
    public List<double?> Temperature_2m_Max { get; set; }
    public List<double?> Temperature_2m_Min { get; set; }
    public List<double?> Wind_Speed_10m_Max { get; set; }
    public List<double?> Precipitation_Sum { get; set; } // Yağış
    public List<double?> Relative_Humidity_2m_Max { get; set; } // Nem
    public List<double?> Uv_Index_Max { get; set; } // UV Endeksi
}
