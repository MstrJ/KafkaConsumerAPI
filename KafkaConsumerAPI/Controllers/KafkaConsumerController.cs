using Confluent.Kafka;
//using FluentEmail.Core;
//using FluentEmail.Razor;
//using FluentEmail.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.Net.Mail;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;
using MailKit.Net.Smtp;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using KafkaConsumerAPI.Services.EmailService;
using KafkaConsumerAPI.Models;

namespace KafkaConsumerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KafkaConsumerController : ControllerBase
    {

        private  IEmailService _emailService;
        private static IPostRepository _posts = new PostRepository();

        
        public KafkaConsumerController(IEmailService emailService)
        {
            _posts.GetAll();
            _emailService = emailService;
        }

        // Pierwsze uruchomienie ~ delay 30 sec
        [HttpGet("GetALL")]
        public async Task<IActionResult> GetALL()
        {
            return Ok(_posts.GetAll());
        }


        //[HttpPost("SendInflueEmail")]
        //public async Task<IActionResult> Send(string AbsolutePath,string mailAdress,string mailTitle)
        //{
        //    //string path = @"C:\Users\Intern\source\repos\restApiProjects\restApi1Folder\KafkaConsumerAPI\KafkaConsumerAPI\mails";
        //    string path = AbsolutePath;
        //    string body = "";

        //    foreach (var item in _posts.GetAll())
        //    {
        //        body += $"<p>{item.Id}\t{item.Title}\t{item.Content}</p>";
        //    }
        //    var sender = new SmtpSender(() => new SmtpClient("localhost")
        //    {
        //        EnableSsl = false,
        //        DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
        //        PickupDirectoryLocation = path
        //    });

        //    StringBuilder template = new();
        //    template.AppendLine("Dear @Model.Mail");
        //    template.AppendLine("<p>Kafka Consumer Log</p>");
        //    template.AppendLine(body);
        //    template.AppendLine("<p>- Blogger Team</p>");

        //    Email.DefaultSender = sender;
        //    Email.DefaultRenderer = new RazorRenderer();

        //    var email = await Email
        //        .From("BloggerTheBestTeam@blogger.com")
        //        .To(mailAdress)
        //        .Subject(mailTitle)
        //        .UsingTemplate(template.ToString(), new { Mail = mailAdress})
        //        .SendAsync();
        //    if (email.Successful)
        //    {
        //        Ok("Sent");
        //    }
        //    else
        //    {
        //        return BadRequest("Error");
        //    }
        //    return NotFound();
        //}        


        [HttpPost("SentMail")]
        public async Task<IActionResult> Send(string To, string Subject)
        {
            string body = "<h1>Kafka History Log</h1>";
            foreach (var item in _posts.GetAll())
            {
                body += $"<p>{item.Id}\t{item.Title}\t{item.Content}</p>";
            }

            _emailService.SendEmail(new EmailDto(To, Subject, body));
            return Ok(body);
        }
    }
}