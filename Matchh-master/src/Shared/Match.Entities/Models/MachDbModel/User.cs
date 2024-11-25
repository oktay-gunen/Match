using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Match.Entities.Models.MachDbModel
{
    [Table("Users")]
    public class User:IEntity
	{
		public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int OperationClaimId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string Phone { get; set; }
        public int Status { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
