using System.ComponentModel.DataAnnotations;

namespace Models.DTOs;

public class LoginDto
{
    [Required]
    [DataType(DataType.Text)]
    [Display(Name = "Kullan�c� Ad�")]
    public string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Parola")]
    public string Password { get; set; }

    [Display(Name = "Beni Hat�rla?")]
    public bool RememberMe { get; set; }
}