using System.ComponentModel.DataAnnotations;

namespace Models.DTOs;

public class LoginDto
{
    [Required]
    [DataType(DataType.Text)]
    [Display(Name = "Kullanýcý Adý")]
    public string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Parola")]
    public string Password { get; set; }

    [Display(Name = "Beni Hatýrla?")]
    public bool RememberMe { get; set; }
}