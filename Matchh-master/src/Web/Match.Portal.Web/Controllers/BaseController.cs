using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Match.Web.Controllers
{
	[Authorize]
	public class BaseController : Controller
    {
		
	}
}

