using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Match.Entities.Models.MachDbModel
{
    [Table("UserOperationClaims")]
    public class UserOperationClaim:IEntity
	{
        public int UserId { get; set; }
        public int Id { get; set; }
        public int OperationClaimId { get; set; }
    }
}

