using System;
using System.ComponentModel.DataAnnotations;

namespace Match.Web.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Adı")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Soyadı")]
        public string Surname { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Telefon")]
        public string Phone { get; set; }

        [StringLength(10, ErrorMessage = "{0} alanı enaz 6 karakter uzunluğunda olmalıdır", MinimumLength = 6)]
        [Display(Name = "Parola")]
        [DataType(dataType: DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Parola")]
        [DataType(dataType: DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Parola eşleşmiyor")]
        public string ConfirmPassword { get; set; }
        [Required]
        public int OperationClaimId { get; set; }

        public List<ListItemModel>? Roles { get; set; }

    }
}

