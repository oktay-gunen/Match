using Match.Business.Services;
using Match.Core.Utilities.Results;
using Match.DataAccess.Abstract;
using Match.Entities.Models.MachDbModel;

namespace Match.Business.Concrete
{
    public class UserManager : IUserService
    {
        private readonly IUserDal _userDal;

        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public IResult Add(User user)
        {
            _userDal.Add(user);
            return new SuccessResult();
        }

        public IResult Delete(User user)
        {
           _userDal.Delete(user);
            return new SuccessResult();
        }

        public IDataResult<List<User>> GetAllUser()
        {
            var result = _userDal.GetList().ToList();
            return new SuccessDataResult<List<User>>(result);
        }

        public OperationClaim GetUserClaim(User user)
        {
            var result=_userDal.GetUserClaim(user);
            return result;
        }
        public List<OperationClaim> GetClaims()
        {
            var result=_userDal.GetClaims();
            return result;
        }

        public IDataResult<User> GetUserByEmail(string Email)
        {
             var result=_userDal.Get(a=>a.Email==Email);
            return new SuccessDataResult<User>(result);
        }

        public IDataResult<User> GetUserById(int userId)
        {
            return new SuccessDataResult<User>(_userDal.Get(a => a.Id == userId));
        }

        public IResult Update(User user)
        {
            _userDal.Update(user);
            return new SuccessResult();
        }

        public User GetByMail(string email)
        {
            return _userDal.Get(u => u.Email == email);
        }
    }
}
