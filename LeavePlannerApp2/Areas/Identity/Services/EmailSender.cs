using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace LeavePlannerApp2.Areas.Identity.Services
{
    //public class EmailSender : IEmailSender
    //{
    //    // Our private configuration variables
    //    private string host;
    //    private int port;
    //    private bool enableSSL;
    //    private string userName;
    //    private string password;

    //    // Get our parameterized configuration
    //    public EmailSender(string host, int port, bool enableSSL, string userName, string password)
    //    {
    //        this.host = host;
    //        this.port = port;
    //        this.enableSSL = enableSSL;
    //        this.userName = userName;
    //        this.password = password;
    //    }

    //    // Use our configuration to send the email by using SmtpClient
    //    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    //    {
    //        var client = new SmtpClient(host, port)
    //        {
    //            Credentials = new NetworkCredential(userName, password),
    //            EnableSsl = enableSSL
    //        };
    //        return client.SendMailAsync(
    //            new MailMessage(userName, email, subject, htmlMessage) { IsBodyHtml = true }
    //        );
    //    }
    //}
    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor, IConfiguration configuration)
        {
            Options = optionsAccessor.Value;
            _configuration = configuration;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        private IConfiguration _configuration;

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(subject, message, email);
        }

        public Task Execute(string subject, string message, string email)
        {
            var client = new SendGridClient(_configuration["EmailSender:UserName"]);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("davidzagi93@gmail.com", "Joe Smith"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.TrackingSettings = new TrackingSettings
            {
                ClickTracking = new ClickTracking { Enable = false }
            };

            return client.SendEmailAsync(msg);
        }
    }
}
