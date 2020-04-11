using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.VisualStudio.Utilities;
using RTSTicket.Service;
using RTSTicket.Service.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RTSTicket.Web.Filters
{
	public class MyAuthorizationFilter : AuthorizeAttribute, IAuthorizationFilter///ActionFilterAttribute
	{
		public string Role { get; set; }


		public void OnAuthorization(AuthorizationFilterContext context)
		{
			var isAuthenticated = context.HttpContext.AuthenticateAsync().Result.Succeeded;

			if (isAuthenticated || String.IsNullOrEmpty(Role))
			{
				var theRole = context.HttpContext.Session.GetString(Role);
				if (theRole != null)
				{
					return;
				}
			}
			
			context.Result = new RedirectToActionResult ("AccessDenied", "Acount", 5);

			return;
		}

		
	}
}
