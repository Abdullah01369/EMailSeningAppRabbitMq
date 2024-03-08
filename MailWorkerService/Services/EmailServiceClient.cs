using MailWorkerService.Models;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace MailWorkerService.Services
{

    public class EmailServiceClient : IEmailSenderService
    {

        private readonly EmailSettings _settings;

        public EmailServiceClient(IOptions<EmailSettings> options)
        {
            _settings = options.Value;

        }
        public async Task SendMailAsync(string emailcontent, string to)
        {
            var smtpclient = new SmtpClient();
            to = "abdullahaarici14@gmail.com";
            smtpclient.Host = _settings.host;
            smtpclient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpclient.UseDefaultCredentials = false;
            smtpclient.Port = 587;
            smtpclient.Credentials = new NetworkCredential(_settings.email, _settings.password);
            smtpclient.EnableSsl = true;

            var mailmessage = new MailMessage();

            mailmessage.From = new MailAddress(_settings.email);
            mailmessage.To.Add(to);
            mailmessage.Subject = "rabbitmqdeneme mesajı";
            mailmessage.Body = $"<h4> Merhabalar size gonderdiğimiz mesaj su sekildedir: <h4><p>{emailcontent}<p>";

            mailmessage.IsBodyHtml = true;

            await smtpclient.SendMailAsync(mailmessage);

        }
    }
}
