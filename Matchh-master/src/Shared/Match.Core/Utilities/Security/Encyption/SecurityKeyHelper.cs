using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Match.Core.Utilities.Security.Encyption
{
	public class SecurityKeyHelper
	{
        public static SecurityKey CreateSecurityKey(string securityKey) => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

    }
}

