using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RTSTicket.Web.Filters
{
	public class MyAuthorizationFilter : Attribute, IAsyncAuthorizationFilter
	{
		//private readonly IEnumerable<string> _roles;
		//
		//public MyAuthorizationFilter(params string[] roles) => _roles = roles;
		//
		//public MyAuthorizationFilter(string role) => _roles = new List<string> { role };
		public Task OnAuthorizationAsync(AuthorizationFilterContext context )
		{
			var cr = context.HttpContext.User.Claims;
			//var res = featureAuth;
			
			throw new NotImplementedException();
		}
	}
}
