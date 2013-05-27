using SendGridMail;
using SendGridMail.Transport;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.Blog.Email
{
    public class EmailSender : ISendEmail
    {
        public void Send(IEmailMessage message)
        {
            SendGrid myMessage = SendGrid.GetInstance();
            myMessage.AddTo(message.To);
            myMessage.From = new MailAddress(message.From, message.FromName);
            myMessage.Subject = message.Subject;
            myMessage.Html = message.Body;

            // Create credentials, specifying your user name and password.
            var credentials = new NetworkCredential(ConfigurationManager.AppSettings["SendGridUsername"], ConfigurationManager.AppSettings["SendGridPassword"]);

            // Create an SMTP transport for sending email.
            var transportSMTP = SMTP.GetInstance(credentials);

            // Send the email.
            transportSMTP.Deliver(myMessage);
        }
    }
}
