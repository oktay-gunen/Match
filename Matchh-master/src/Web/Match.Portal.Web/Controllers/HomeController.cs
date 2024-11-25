using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Match.Web.Models;
using Match.Business.Services;
using System.Security.Claims;
using Match.Core.Extensions;

namespace Match.Web.Controllers;

public class HomeController : BaseController
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUserService _userService;
    private readonly IReportService _repoReportService;

    public HomeController(ILogger<HomeController> logger, IUserService userService, IReportService repoReportService)
    {
        _logger = logger;
        _userService = userService;
        _repoReportService = repoReportService;
    }

    public async Task<IActionResult> IndexAsync()
    {
        var claims = User.Claims;
        var asdasd = User.GetUserName();
        var sad = User.Identity.IsAuthenticated;
        var asd = User.Identity.Name;
        var kk = User.GetUserId();
        var kl = User.GetNameFirstLetters();
        var dd = await _repoReportService.GetReportFP_01_01Async();

        return View();
    }

    public IActionResult Privacy()
    {
        var aa = _userService.GetUserById(1);
        return View(aa);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

