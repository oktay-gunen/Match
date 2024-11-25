using System.ComponentModel.DataAnnotations;

namespace Match.Web.Models
{
    public class AccountDetailViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Ad")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Soyad")]
        public string Surname { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name="Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name="Telefon")]        
        public string Phone { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? StatusName { get; set; }
        public bool IsDeleted { get; set; }
        public int OperationClaimId { get; set; }
        public string OperationClaimName { get; set; }
    }
}