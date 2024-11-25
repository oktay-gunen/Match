using System.ComponentModel.DataAnnotations;

namespace Match.Web.Models
{
    public class UpdateUserViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Ad")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Soyad")]
        public string Surname { get; set; }
       
        [Display(Name="Email")]
        public string? Email { get; set; }
       
        [Display(Name="Telefon")]        
        public string? Phone { get; set; }
        public bool IsDeleted { get; set; }
        public int OperationClaimId { get; set; }

        public List<ListItemModel>? Roles { get; set; }
    }
}