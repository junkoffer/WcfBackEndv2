using MailKit.Net.Smtp;
using MimeKit;
using System.Configuration;
using System.Linq;
using WcfBackEndv2.Model;

namespace WcfBackEndv2.MailKit
{
    /// <summary>
    /// SendMailSimple använder sig av MailKit och ett Googlekonto med sänkt säkerhet. 
    /// installera MailKit med Nuget package manager. skriv: 
    ///     Install-Package MailKit
    /// 
    /// I stället för Oath2 så räcker det här med användarnamn och lösenord
    /// I Googlekontot du använder så måste du först godkänna anslutning med sänkt säkerhet. 
    /// Gå till https://www.google.com/settings/security/lesssecureapps & välj "Turn On"
    /// </summary>
    public class SendMailSimple
    {
        public static bool SendRegistrationConfirmation(ServiceCase serviceCase)
        {
            // om inget meddelande finns, och ingen vettig e-postadress 
            // så kan inget meddelande skickas
            if (validateSendRequest(serviceCase))
            {
                return false;
            }
            var subject = $"Ett seviceärende med ID [{serviceCase.CaseNr}] är skapat";
            var messageText = $"Ett seviceärende med ID [{serviceCase.CaseNr}] har nu skapats åt dig.\n\n"
                + "Det mottagna ärendet ser ut såhär:\n\n"
                + $"namn: {serviceCase.Name}\nlägenhetsnummer: {serviceCase.FlatNr}\n"
                + $"E-Post: {serviceCase.ContactEmail}\n\n"
                + $"Meddelande:\n{serviceCase.Posts.FirstOrDefault().Message}";
            SendMessage(messageText, subject, serviceCase.Name, serviceCase.ContactEmail);
            return true;
        }

        public static bool SendLastPostToTennant(ServiceCase serviceCase)
        {
            // om inget meddelande finns, och ingen vettig e-postadress 
            // så kan inget meddelande skickas
            // om inägget är markerat som privat så ska det heller inte skickas!
            if (validateSendRequest(serviceCase) || serviceCase.Posts.LastOrDefault().Private)
            {
                return false;
            }
            var subject = $"Nytt inlägg i ditt serviceärende, nr. [{serviceCase.CaseNr}].";
            var messageText = $"Det finns ett nytt inlägg i serviceärende nr. [{serviceCase.CaseNr}]:\n\n"
                + $"{serviceCase.Posts.LastOrDefault().Message}\n\n"
                + $"--------------------------------------------------\n"
                + $"THN-AB\n{serviceCase.Posts.LastOrDefault().Name}\n";
            //+ $"E-Post: {serviceCase.Posts.LastOrDefault().ContactEmail}\n\n"
            SendMessage(messageText, subject, serviceCase.Name, serviceCase.ContactEmail);
            return true;
        }

        private static bool validateSendRequest(ServiceCase serviceCase)
        {
            // om inget meddelande finns, och ingen vettig e-postadress 
            // så kan inget meddelande skickas
            return serviceCase == null
                || serviceCase.Posts == null
                || serviceCase.Posts.Count() == 0
                || !InputValidator.ValidateEmail(serviceCase.ContactEmail);
        }

        private static void SendMessage(string messageText, string subject, string recieverName, string recieverMail)
        {
            var message = new MimeMessage();
            var caseRegisteringEmailAdress = ConfigurationManager.ConnectionStrings["CaseRegisteringEmailAdress"].ConnectionString;
            var from = ConfigurationManager.ConnectionStrings["EmailSenderEmailAdress"].ConnectionString;
            message.From.Add(new MailboxAddress("THN-AB", from));
            message.To.Add(new MailboxAddress(recieverName, recieverMail));
            message.Subject = subject;
            message.Body = new TextPart("plain")
            {
                Text = messageText
            };

            using (var client = new SmtpClient())
            {
                var emailSenderEmailAdress = ConfigurationManager.ConnectionStrings["EmailSenderEmailAdress"].ConnectionString;
                var emailSenderEmailPassword = ConfigurationManager.ConnectionStrings["EmailSenderEmailPassword"].ConnectionString;
                client.Connect("smtp.gmail.com", 587);
                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(emailSenderEmailAdress, emailSenderEmailPassword);
                client.Send(message);
                client.Disconnect(true);
            }
        }

    }
}