using System.ComponentModel.DataAnnotations;

namespace Models.DTOs;

public class LoginDto
{
    [Required]
    [DataType(DataType.Text)]
    [Display(Name = "Kullanıcı Adı")]
    public string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Parola")]
    public string Password { get; set; }

    [Display(Name = "Beni Hatırla")]
    public bool RememberMe { get; set; }
}