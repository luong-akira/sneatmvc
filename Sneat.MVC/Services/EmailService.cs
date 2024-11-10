using Sneat.MVC.Common;
using System.Net;
using System.Net.Mail;

namespace Sneat.MVC.Services
{
    public class EmailService
    {
        public void configClient(string emailRecieVe, string title, string content)
        {
            var fromAddress = new MailAddress(SystemParam.EMAIL_CONFIG);
            var toAddress = new MailAddress(emailRecieVe);
            string fromPassword = SystemParam.PASS_CONFIG;

            var smtp = new SmtpClient
            {
                Host = SystemParam.HOST_DEFAUL,
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = title,
                Body = content,
                IsBodyHtml = true
            })
            {
                smtp.Send(message);
            }
        }
    }
}