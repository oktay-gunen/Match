using System;
using Match.Core.DataAccess;
using Match.Entities.Models.MachDbModel;

namespace Match.DataAccess.Abstract
{
	public interface IUserDal:IEntityRepository<User>
	{
		OperationClaim GetUserClaim(User  user);
		List<OperationClaim> GetClaims();

	}
}

