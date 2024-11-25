using System.Security.Claims;
using Match.Core.Utilities.Results;
using Match.Entities.Dtos;
using Match.Entities.Models.MachDbModel;

namespace Match.Business.Services
{
    public interface IAuthService
    {
        IDataResult<User> Register(UserRegisterDto userForRegisterDto);
        IDataResult<List<Claim>> Login(UserLoginDto userForLoginDto);
        IDataResult<User> UpdateUser(User updateUser);
        IResult UserExists(string email);
    }
}

