using System.Net.Mail;
using System.Net;

namespace ECommerce.Services
{
    public static class EmailService
    {
        public static void SendSmtp(string to, MailMessage message)
        {
            var host = "smtp.gmail.com";
            var port = 587;

            using var client = new SmtpClient(host)
            {
                Port = port,
                EnableSsl = true,
                Credentials = new NetworkCredential("elisamilzade@gmail.com", "ihosihmulzwuyrpb")
            };

            message.From = new MailAddress("elisamilzade@gmail.com");
            message.To.Add(new MailAddress(to));

            client.Send(message);
        }
    }
}
