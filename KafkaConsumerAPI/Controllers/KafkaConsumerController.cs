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

        // Pierwsze uruchomienie ~ delay 60 sec
        [HttpGet("GetALL")]
        public async Task<IActionResult> GetALL()
        {
            return Ok(_posts.GetAll());
        }

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