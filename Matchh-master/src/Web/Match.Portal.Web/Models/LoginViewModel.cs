using System.ComponentModel.DataAnnotations;

namespace Match.Web.Models;

public class LoginViewModel
{
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }
    [StringLength(10, ErrorMessage = "{0} alanı enaz 6 karakter uzunluğunda olmalıdır", MinimumLength = 6)]
    [Display(Name = "Parola")]
    [DataType(dataType: DataType.Password)]
    public string Password { get; set; }
    public bool IsRememberMe { get; set; }
}
