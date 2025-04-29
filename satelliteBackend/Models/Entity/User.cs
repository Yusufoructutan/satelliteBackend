public class User
{
    public int UserId { get; set; } // Kullanıcı ID
    public string Username { get; set; } // Kullanıcı adı
    public string PasswordHash { get; set; } // Şifre hash'i
    public string PasswordSalt { get; set; } // Anahtarı saklamak için

    public string Email { get; set; } // Email adresi

    public ICollection<Location> Locations { get; set; } = new List<Location>();

}
