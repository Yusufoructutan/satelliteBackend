using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class LocationDto
{
    public double SouthWestLatitude { get; set; } // GüneyBatı Enlemi
    public double SouthWestLongitude { get; set; } // GüneyBatı Boylamı
    public double NorthEastLatitude { get; set; } // KuzeyDoğu Enlemi
    public double NorthEastLongitude { get; set; } // KuzeyDoğu Boylamı

    [DataType(DataType.DateTime)]
    [JsonConverter(typeof(DateTimeJsonConverter))]
    public DateTime StartDate { get; set; } // Başlangıç tarihi

    [DataType(DataType.DateTime)]
    [JsonConverter(typeof(DateTimeJsonConverter))]
    public DateTime EndDate { get; set; }   // Bitiş tarihi
}
