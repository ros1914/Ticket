namespace RTSTicket.Web.Controllers
{
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using RTSTicket.Data;
	using RTSTicket.Service;
	using RTSTicket.Service.Models.Acount;
	using RTSTicket.Web.Models.ViewModels.Acount;
	using RTSTicket.Web.Services;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.Linq;
	using System.Text.Encodings.Web;
	using System.Threading.Tasks;

	public class AcountController : Controller
	{
		private readonly IAcountServices acountServices;
		private readonly RTSTicketDbContext dbContext;
		private readonly HttpContext httpContext;
		private readonly IEmailSender emailSender;

		public AcountController(IAcountServices acountServices, RTSTicketDbContext dbContext, IHttpContextAccessor contextAccessor, IEmailSender emailSender)
		{
			this.acountServices = acountServices;
			this.dbContext = dbContext;
			this.httpContext = contextAccessor.HttpContext;
			this.emailSender = emailSender;
		}

		[HttpGet]
		public IActionResult Register()
		{

			return View();
		}

		[HttpPost]
		//[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterBindingModel model)
		{

			if (!ModelState.IsValid)
			{
				return View();
			}
			var userEmail = this.dbContext.Users.Where(p => p.Email == model.Email).FirstOrDefault();
			if (userEmail != null)
			{
				this.ViewBag.Error = $"User with this email {model.Email} have acount !!!";
				return View(); ;
			}

			var securityStamp = this.acountServices.SecurityStampTokenProvider();

			var cod = acountServices.TokenProvider();
			bool sucsses = await acountServices.RegisterUserAsync(model, securityStamp);


			if (sucsses)
			{
				var callbackUrl = Url.ActionLink(
				action: "ConfirmEmail",
				controller: "Acount",
				values: new { Email = model.Email, code = cod },
				protocol: Request.Scheme

				);
				await emailSender.SendEmailAsync(model.Email, "Confirm your email", $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

				HttpContext.Session.SetString("username", model.UserName);
				return RedirectToAction("Index", "Home");
			}

			this.ViewBag.Error = "User is not registration";
			return View();
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]

		[ValidateAntiForgeryToken]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Login(LoginModel loginModel)
		{

			if (!ModelState.IsValid)
			{
				return BadRequest();
			}


			var acount = await acountServices.Login(loginModel);
			if (acount == null)
			{
				this.ViewBag.Error = "Invalid user ";
				return View();
			}
			else
			{
				var userToken = this.acountServices.TokenProvider();
				httpContext.Session.SetString("JWToken", userToken);
				HttpContext.Session.SetString("username", acount.UserName);

				return RedirectToAction("Index", "Home");
			}

		}

		[HttpPost]
		public IActionResult Logout()
		{
			HttpContext.Session.Remove("username");
			HttpContext.Session.Remove("JWToken");

			return View();
			//return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public IActionResult ForgotPassword()
		{

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
		{

			if (!ModelState.IsValid)
			{
				return BadRequest();
			}


			var user = await this.acountServices.FindByEmailAsync(model.Email);
			if (user == null)
			{

				return RedirectToAction("ForgotPasswordConfirmation");
			}

			var cod = user.SecurityStamp;
			var newRandomPasword = this.acountServices.CreateRandomPassword();
			var saveNewPassword = await this.acountServices.SaveRandomPassword(newRandomPasword, user.Email);

			if (saveNewPassword == "sucsses")
			{
				//var cod = acountServices.TokenProvider();
				var callbackUrl = Url.ActionLink(
					action: "ResetPassword",
					controller: "Acount",
					values: new { code = cod },
					protocol: Request.Scheme

					);
				await emailSender.SendEmailAsync(model.Email, "Reset Password",
						$"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>. This is a new password \"{newRandomPasword}\" ");

				return RedirectToAction("ForgotPasswordConfirmation");
			}


			return BadRequest();
		}

		[HttpGet]
		public async Task<IActionResult> ConfirmEmail(ConfirmEmailModel model)
		{
			if (!ModelState.IsValid)
			{
				this.TempData["Error"] = $"Unable to load user with email '{model.Email}'.";
				return RedirectToAction("Index", "Home");
			}
			var result = await this.acountServices.ConfirmEmail(model.Email, model.Code);
			if (result == "succses")
			{
				return View();
			}

			this.TempData["Error"] = "The email is not conferm";
			return RedirectToAction("Index", "Home");

		}

		[HttpGet]
		public IActionResult ForgotPasswordConfirmation()
		{
			return View();
		}

		[HttpGet]
		public IActionResult ResetPassword(string code)
		{
			if (String.IsNullOrEmpty(code))
			{
				return BadRequest();
			}

			var model = new ResetPasswordModel() { Code = code };
			return View(model);
		}

		public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			var resultSavePassword = await this.acountServices.SaveForgotPassword(model);
			if (resultSavePassword == "sucsses")
			{
				return RedirectToAction("ResetPasswortConfirmation");
			}


			this.TempData["Error"] = "The password is not change !!!";
			return RedirectToAction("ForgotPassword");
		}

		public IActionResult ResetPasswortConfirmation()
		{
			return View();
		}
	}
}