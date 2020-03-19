using System.ComponentModel.DataAnnotations;

namespace RTSTicket.Service.Models.Acount
{
	public class LoginModel
	{
		[Required]
		[MinLength(2)]
		[MaxLengthAttribute(20)]
		[Display(Name = "User Name")]
		public string UserName { get; set; }

		[Required]
		[MinLength(3, ErrorMessage = "Password length must be more 2 symbols !!!")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Display(Name ="Remember Me")]
		public bool RememberMe { get; set; }
	}
}
