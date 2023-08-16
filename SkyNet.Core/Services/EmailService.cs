using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SkyNet.Core.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmail(string toEmail, string subject, string body)
        {
            string FromEmail = _configuration["EmailSettings:User"];
            string password = _configuration["EmailSettings:Password"];
            string smtp = _configuration["EmailSettings:SMTP"];
            int port = Int32.Parse(_configuration["EmailSettings:PORT"]);

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(FromEmail));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;

            var bodybuilder = new BodyBuilder();
            bodybuilder.HtmlBody = body;
            email.Body = bodybuilder.ToMessageBody();
            
            using(var smtpCl = new MailKit.Net.Smtp.SmtpClient())
            {
                smtpCl.Connect(smtp, port, MailKit.Security.SecureSocketOptions.SslOnConnect);
                smtpCl.Authenticate(FromEmail, password);
                await smtpCl.SendAsync(email);
                smtpCl.Disconnect(true);
            }
        }
    }
}
