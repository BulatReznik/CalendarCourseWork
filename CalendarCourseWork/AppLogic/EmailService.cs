using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace CalendarCourseWork
{
    public interface IEmailService
    {
        bool SendEmail(string to, string subject, string html, string from);
    }

    public class EmailService : IEmailService
    {
        public EmailService()
        {
        }

        public bool SendEmail(string to, string subject, string html, string from)
        {
            try
            {
                // create message
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(from));
                email.To.Add(MailboxAddress.Parse(to));
                email.Subject = subject;
                email.Body = new TextPart(TextFormat.Html) { Text = html };

                // send email using
                var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate("7777bulat7777", "llla zkts uyym rbhh");
                smtp.Send(email);
                smtp.Disconnect(true);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}