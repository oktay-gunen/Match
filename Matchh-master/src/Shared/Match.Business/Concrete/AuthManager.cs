using System;
using System.Security.Claims;
using Match.Business.Constants;
using Match.Business.Constants.Enums;
using Match.Business.Services;
using Match.Core.Extensions;
using Match.Core.Utilities.Results;
using Match.Core.Utilities.Security.Hashing;
using Match.Entities.Dtos;
using Match.Entities.Models.MachDbModel;

namespace Match.Business.Concrete
{ 
    public class AuthManager : IAuthService
    {
        private IUserService _userService;
       

        public AuthManager(IUserService userService)
        {
            _userService = userService;
        }

      
        public IDataResult<User> Register(UserRegisterDto userRegisterDto)
        {
            var userEmailToCheck = _userService.GetByMail(userRegisterDto.Email);
            if (userEmailToCheck!=null)
            {
                return new ErrorDataResult<User>(Messages.UserAlreadyExists);
            }
            if (userEmailToCheck?.Phone== userRegisterDto.Phone)
            {
                return new ErrorDataResult<User>(Messages.UserUsesPhone);
            }
            string passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(userRegisterDto.Password, out passwordHash, out passwordSalt);
            var user = new User
            {
                Email = userRegisterDto.Email,
                Name = userRegisterDto.Name,
                Surname = userRegisterDto.Surname,
                Password = passwordHash,
                PasswordSalt = passwordSalt,
                Phone= userRegisterDto.Phone,
                UpdateDate=DateTime.Now,
                CreateDate=DateTime.Now,
                Status = (int)UserStatus.Active
            };
            _userService.Add(user);
            return new SuccessDataResult<User>(user, Messages.UserRegistered);
        }
        public IDataResult<User> UpdateUser(User updateUser)
        {
            var userEmailToCheck = _userService.GetByMail(updateUser.Email);
            if (userEmailToCheck==null)
            {
                return new ErrorDataResult<User>(Messages.UserNoExists);
            }
           
                if(userEmailToCheck?.Password!=updateUser.Password)
            {   string passwordHash, passwordSalt;
                HashingHelper.CreatePasswordHash(updateUser.Password, out passwordHash, out passwordSalt);
                updateUser.Password = passwordHash;
                updateUser.PasswordSalt = passwordSalt;
            }
            
            
            _userService.Update(updateUser);
            return new SuccessDataResult<User>(updateUser, Messages.SuccesInfo);
        }

        public IDataResult<List<Claim>> Login(UserLoginDto userLoginDto)
        {
            var userToCheck = _userService.GetByMail(userLoginDto.Email);
            if (userToCheck == null || userToCheck.Status!=1)
            {
                return new ErrorDataResult<List<Claim>>(Messages.UserNotFound);
            }

            if (!HashingHelper.VerifyPasswordHash(userLoginDto.Password, userToCheck.Password, userToCheck.PasswordSalt))
            {
                return new ErrorDataResult<List<Claim>>(Messages.PasswordError);
            }

             var userClaims=SetClaims(userToCheck);


            return new SuccessDataResult<List<Claim>>(userClaims.ToList(), Messages.SuccessfulLogin);
        }

        public IResult UserExists(string email)
        {
            if (_userService.GetByMail(email) != null)
            {
                return new ErrorResult(Messages.UserAlreadyExists);
            }
            return new SuccessResult();
        }
         private IEnumerable<Claim> SetClaims(User user)
        {   
            var claims = new List<Claim>();
            var userRoles = _userService.GetUserClaim(user);
            claims.AddNameIdentifier(user.Id.ToString());
            claims.AddEmail(user.Email);
            claims.AddName($"{user.Name} {user.Surname}");
            claims.AddNameFirstLetters($"{user.Name.Substring(0,1).ToUpper()}{user.Surname.Substring(0,1).ToUpper()}");
            claims.AddRoles(new string[]{userRoles.Name});
            
            return claims;
        }
    }
    
}

