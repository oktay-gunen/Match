using System;
using Match.Web.Models;
using Match.Web.Helper;
using Match.Business.Services;
using Match.Entities.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Match.Core.Extensions;
using Match.Core.Utilities.Results;
using Match.Business.Constants.Enums;
using Match.Business.Constants;

namespace Match.Web.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public AccountController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }



        [AllowAnonymous]
        [Route("login")]
        public ActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var loginDto = new UserLoginDto()
                {
                    Email = model.Email,
                    Password = model.Password
                };

                var result = _authService.Login(loginDto);

                if (result.Success)
                {
                    var claimIdentity = new ClaimsIdentity(result.Data, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperies = new AuthenticationProperties()
                    {
                        IsPersistent = model.IsRememberMe
                    };

                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimIdentity),
                        authProperies
                    );

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, result.Message);

            }
            return View(model);
        }
        public ActionResult Register()
        {
            var claimCategory = _userService.GetClaims();


            var model = new RegisterViewModel
            {
                Roles = GetListItemModel(claimCategory)
            };
            return View(model);
        }
        public ActionResult AccountList()
        {
            var result = _userService.GetAllUser();
            var claimCategory = _userService.GetClaims();
            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View();
            }

            var userList = result.Data.Select(b => new UserViewModel()
            {
                Id = b.Id,
                Name = b.Name,
                Surname = b.Surname,
                Email = b.Email,
                Phone = b.Phone,
                Status = b.Status,
                OperationClaimId = b.OperationClaimId,
                OperationClaimName = claimCategory.FirstOrDefault(a => a.Id == b.OperationClaimId)?.Name ?? "",
                StatusName = StringHelper.GetUserStatus(b.Status)
            }).ToList();

            return View(userList);
        }


        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userDto = new UserRegisterDto()
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    Email = model.Email,
                    Phone = model.Phone,
                    Password = model.Password,
                    OperationClaimId = model.OperationClaimId
                };

                var result = _authService.Register(userDto);



                if (result.Success)
                {
                    ViewData["InfoMessage"] = result.Message;
                    return RedirectToAction("Accountlist", "Account");
                }

                ModelState.AddModelError(string.Empty, result.Message);

            }
            var claimCategory = _userService.GetClaims();
            model.Roles = GetListItemModel(claimCategory);

            return View(model);
        }

        public ActionResult AccountDetail()
        {
            var userId = User.GetUserId();


            var result = _userService.GetUserById(userId);
            var claimCategory = _userService.GetClaims();
            var userInfo = new AccountDetailViewModel();

            if (result.Success)
            {
                userInfo.Name = result.Data.Name;
                userInfo.Surname = result.Data.Surname;
                userInfo.Email = result.Data.Email;
                userInfo.Phone = result.Data.Phone;
                userInfo.OperationClaimId = claimCategory.FirstOrDefault(a => a.Id == result.Data.OperationClaimId)?.Id ?? 0;
                userInfo.OperationClaimName = claimCategory.FirstOrDefault(a => a.Id == result.Data.OperationClaimId)?.Name ?? "";
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.Message);
            }

            return View(userInfo);
        }
        public ActionResult UpdateUser(int Id)
        {

            var result = _userService.GetUserById(Id);
            var claimCategory = _userService.GetClaims();
            var userInfo = new UpdateUserViewModel();

            if (result.Success)
            {
                userInfo.Id = result.Data.Id;
                userInfo.Name = result.Data.Name;
                userInfo.Surname = result.Data.Surname;
                userInfo.Email = result.Data.Email;
                userInfo.Phone = result.Data.Phone;
                userInfo.OperationClaimId = result.Data.OperationClaimId;
                userInfo.Roles = GetListItemModel(claimCategory);
                userInfo.IsDeleted = result.Data.Status == (int)UserStatus.Deleted ? true : false;

                ViewData["InfoMessage"] = result.Message;
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.Message);
            }

            return View(userInfo);
        }
        [HttpPost]
        public async Task<ActionResult> UpdateUser(UpdateUserViewModel userInfo)
        {
             var result = _userService.GetUserById(userInfo.Id);
            if (ModelState.IsValid)
            {
                if (result.Success)
                {
                    result.Data.Name = userInfo.Name;
                    result.Data.Surname = userInfo.Surname;
                    result.Data.OperationClaimId=userInfo.OperationClaimId;
                    result.Data.UpdateDate = DateTime.Now;
                    result.Data.Status = userInfo.IsDeleted ? (int)UserStatus.Deleted: (int)UserStatus.Active;
                  

                    var resultUpadete = _authService.UpdateUser(result.Data);

                    if (!resultUpadete.Success)
                    {
                        ModelState.AddModelError(string.Empty, resultUpadete.Message);
                    }


                    ViewData["InfoMessage"] = resultUpadete.Message;

                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                }


            }
            var claimCategory = _userService.GetClaims();
            userInfo.Roles = GetListItemModel(claimCategory);
            userInfo.Email=result?.Data?.Email;
            userInfo.Phone=result?.Data?.Phone;

            return View(userInfo);
        }

        [HttpPost]
        public async Task<ActionResult> AccountDetailAsync(AccountDetailViewModel userInfo)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(userInfo.Password) && userInfo.Password.Length < 6)
                {
                    ModelState.AddModelError(string.Empty, Messages.PassworMustLength);
                }
                if (!string.IsNullOrEmpty(userInfo.Password) && userInfo.Password != userInfo.ConfirmPassword)
                {
                    ModelState.AddModelError(string.Empty, Messages.PassworMustEqu);
                }
                var userId = User.GetUserId();
                var result = _userService.GetUserById(userId);

                if (result.Success)
                {
                    result.Data.Name = userInfo.Name;
                    result.Data.Surname = userInfo.Surname;
                    result.Data.Phone = userInfo.Phone;
                    result.Data.Email = userInfo.Email;

                    result.Data.UpdateDate = DateTime.Now;
                    result.Data.Status = userInfo.IsDeleted ? (int)UserStatus.Deleted : (int)UserStatus.Active;

                    if (!string.IsNullOrEmpty(userInfo.Password))
                    {
                        result.Data.Password = userInfo.Password;
                    }
                   

                    var resultUpadete = _authService.UpdateUser(result.Data);

                    if (!resultUpadete.Success)
                    {
                        ModelState.AddModelError(string.Empty, resultUpadete.Message);
                    }


                    ViewData["InfoMessage"] = resultUpadete.Message;

                    //deleted user redirect login page
                    if (userInfo.IsDeleted)
                    {
                        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        return Redirect("/login");
                    }

                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                }


            }

            return View(userInfo);
        }


        [AllowAnonymous]
        [Route("logout")]
        public async Task<ActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/login");
        }

        private List<ListItemModel> GetListItemModel(List<Entities.Models.MachDbModel.OperationClaim>? claimCategory)
        {
            var itemList = new List<ListItemModel>();
            foreach (var item in claimCategory)
            {
                itemList.Add(new ListItemModel() { Id = item.Id, Name = item.Name });
            }
            return itemList;
        }

    }
}

