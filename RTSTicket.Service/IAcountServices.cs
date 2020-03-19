namespace RTSTicket.Service
{
	using RTSTicket.Service.Models.Acount;
    using System.Threading.Tasks;
	using RTSTicket.Data.Models;
    public interface IAcountServices
	{
		Task<bool> RegisterUserAsync(RegisterBindingModel model , string securityStamp);

		Task<User> Login(LoginModel loginModel);

		string TokenProvider();

		Task<string> ConfirmEmail(string email, string token);
		Task<User> FindByEmailAsync(string email);

		string CreateRandomPassword();

		bool  IsEmailConfirmedAsync(User user);
		Task<string> SaveRandomPassword(string newRandomPasword, string userEmail);
		Task<string> SaveForgotPassword(ResetPasswordModel model);
		string SecurityStampTokenProvider();
	}
}
