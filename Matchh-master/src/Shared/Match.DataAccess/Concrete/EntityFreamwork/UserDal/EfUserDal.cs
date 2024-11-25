using Match.Core.DataAccess.EntityFramework;
using Match.DataAccess.Abstract;
using Match.DataAccess.Concrete.EntityFreamwork.Contexts;
using Match.Entities.Models.MachDbModel;


namespace Match.DataAccess.Concrete.EntityFreamwork.UserDal
{
    public class EfUserDal : EfEntityRepositoryBase<User, MatchContext>, IUserDal
    {

        public OperationClaim GetUserClaim(User user)
        {
            using var context = new MatchContext();
            var result = from operationClaim in context.OperationClaims
                         where operationClaim.Id == user.OperationClaimId
                         select new OperationClaim { Id = operationClaim.Id, Name = operationClaim.Name };
            return result.FirstOrDefault();
        }

        public List<OperationClaim> GetClaims()
        {
            using var context = new MatchContext();

            return context.OperationClaims.ToList();
        }
    }
}
