﻿using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Mailjet.Client;
using Mailjet.Client.Resources;

namespace Rocky_Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public MailJetSettings _mailJetSettings { get; set; }

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }

        public async Task Execute(string email, string subject, string body)
        {
            _mailJetSettings = _configuration.GetSection("MailJet").Get<MailJetSettings>();

            MailjetClient client = new MailjetClient(_mailJetSettings.ApiKey, _mailJetSettings.SecretKey);
            
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
            .Property(Send.Messages, new JArray {
                new JObject {
                    {
                        "From",
                            new JObject 
                            {
                                {"Email", "himanshupanwar001@gmail.com"},
                                {"Name", "Himanshu"}
                            }
                    },        
                    {
                        "To",
                        new JArray {
                        new JObject {
                        {
                            "Email",
                            email
                        }, 
                        {
                            "Name",
                            "Udemy Course"
                        }
                    }
                }
            }, 
            {
                "Subject",
                subject
            }, 
            {
                "HTMLPart",
                body
            }
            }});
            await client.PostAsync(request);
        }
    }
}
