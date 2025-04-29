public class LocationDto
{
    public double SouthWestLatitude { get; set; } // GüneyBatı Enlemi
    public double SouthWestLongitude { get; set; } // GüneyBatı Boylamı
    public double NorthEastLatitude { get; set; } // KuzeyDoğu Enlemi
    public double NorthEastLongitude { get; set; } // KuzeyDoğu Boylamı

    public DateTime StartDate { get; set; } // Başlangıç tarihi
    public DateTime EndDate { get; set; }   // Bitiş tarihi
}
