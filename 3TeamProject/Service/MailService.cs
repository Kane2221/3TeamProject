using _3TeamProject.Models;
using System.Net.Mail;

namespace _3TeamProject.Service
{
    public class MailService
    {
        private readonly IConfiguration _config;

        public MailService(IConfiguration config)
        {
            _config = config;
        }

        public void SendMail(string Address, string Subject, string Body)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("dotnettgm102@gmail.com", "");
                mail.To.Add(Address);
                mail.Priority = MailPriority.Normal;
                mail.Subject = Subject;
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient MySmtp = new SmtpClient("smtp.gmail.com", 587);
                MySmtp.UseDefaultCredentials = false;
                MySmtp.Credentials = new System.Net.NetworkCredential(_config["mail:Account"], _config["mail:Password"]);
                MySmtp.EnableSsl = true;
                MySmtp.Send(mail);
                MySmtp = null;
            };
        }
    }
}
