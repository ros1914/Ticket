using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RTSTicket.Web.Services
{
	public class SendGridEmailSender : IEmailSender
	{
		private SendGridOptions options;

		public SendGridEmailSender(IOptions<SendGridOptions> options)
		{
			this.options = options.Value;
		}
		public  async Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			var client = new SendGridClient(this.options.SendGridApiKey);
			var from = new EmailAddress("ros1914@outlook.com", "RosAdmin");
			var to = new EmailAddress(email, email);
			var msg = MailHelper.CreateSingleEmail(from, to, subject, htmlMessage, htmlMessage);
			var response = await client.SendEmailAsync(msg);
			var body = await response.Body.ReadAsStringAsync();
			var statusCode = response.StatusCode;
		}
	}
}
