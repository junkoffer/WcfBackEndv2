using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using MailKit.Net.Smtp;
using MimeKit;

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
        public static void SendRegistrationConfirmation(ServiceCase serviceCase)
        {
            var subject = $"Ett seviceärende med ID [{serviceCase.CaseNr}] är skapat";
            var messageText = $"Ett seviceärende med ID {serviceCase.CaseNr} har nu skapats åt dig.\n\n"
                + "Det mottagna ärendet ser ut såhär:\n\n"
                + $"namn: {serviceCase.Name}\nlägenhetsnummer: {serviceCase.FlatNr}\n"
                + $"E-Post: {serviceCase.ContactEmail}\n\n"
                + $"Meddelande:\n{serviceCase.Posts.FirstOrDefault().Message}";

            SendMessage(messageText, subject, serviceCase.Name, serviceCase.ContactEmail);
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