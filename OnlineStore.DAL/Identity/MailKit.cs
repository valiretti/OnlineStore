using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNet.Identity;
using MimeKit;

namespace OnlineStore.DAL.Identity
{
    public class MailKit : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            var myMessage = new MimeMessage();
            myMessage.From.Add(new MailboxAddress("Administrator", "admin@exemple.com"));
            myMessage.To.Add(new MailboxAddress(message.Destination));
            myMessage.Subject = message.Subject;

            myMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Body };

            using (var client = new SmtpClient())
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                
                client.Connect("localhost", 25, false);

                // Note: only needed if the SMTP server requires authentication
                //client.Authenticate("admin", "admin");

                await client.SendAsync(myMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
    
}
