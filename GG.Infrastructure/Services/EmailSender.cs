using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace GG.Infrastructure
{
    public class EmailSender : IEmailSender
    {
        readonly SendGridClient _sendGrid;

        public EmailSender(SendGridClient sendGridClient)
        {
            this._sendGrid = sendGridClient;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {


            return Execute(subject, htmlMessage, email);

        }


        public Task Execute(string subject, string message, string email)
        {

            var msg = new SendGridMessage()
            {
                From = new EmailAddress("NotReply@GloblaGetaways.com", "Account Confirmation"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message

            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            return _sendGrid.SendEmailAsync(msg);

        }
    }
}
