using System.ComponentModel.DataAnnotations;

namespace Models.ViewModel;

public class RegisterViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    [Required]
    [DataType(DataType.Text)]
    public string UserName { get; set; }
    
    [Required]
    [DataType(DataType.Text)]
    public string FullName { get; set; }

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }
}