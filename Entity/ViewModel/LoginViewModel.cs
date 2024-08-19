using System.ComponentModel.DataAnnotations;

namespace Models.ViewModel;

public class LoginViewModel
{
    [Required]
    [DataType(DataType.Text)]
    public string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }
}