using Match.Core.Utilities.Results;
using Match.Entities.Models.MachDbModel;

namespace Match.Business.Services
{
    public interface IUserService
    {
        IDataResult<User> GetUserById(int userId);
        IDataResult<User> GetUserByEmail(string Email);
        User GetByMail(string email);
        OperationClaim GetUserClaim(User user);
        List<OperationClaim> GetClaims();
        IDataResult<List<User>>GetAllUser();
        IResult Add(User user);
        IResult Delete(User user);
        IResult Update(User user);
    }
}
