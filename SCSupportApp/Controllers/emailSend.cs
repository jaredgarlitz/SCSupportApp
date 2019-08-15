using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace SCSupportApp.Controllers
{
    public class emailSend
    {

        public static void sendMail(string toEmail)
        {
            Execute(toEmail).Wait();
        }

        static async Task Execute(string toEmail)
        {
            var apiKey = Environment.GetEnvironmentVariable("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");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("jaredxgarlitz@gmail.com", "Jared");
            var subject = "This is a giant test";
            var to = new EmailAddress(toEmail);
            var plainTextContent = "Jared, check email";
            var htmlContent = "<strong> kaboom</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}