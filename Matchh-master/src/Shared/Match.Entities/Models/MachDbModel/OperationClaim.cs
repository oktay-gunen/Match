using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Match.Entities.Models.MachDbModel
{
    [Table("OperationClaims")]
    public class OperationClaim:IEntity
	{

		public int Id { get; set; }
		public string Name { get; set; }
	}
}

