using KafkaConsumerAPI.Models;

namespace KafkaConsumerAPI.Services.EmailService
{
    public interface IEmailService
    {
        void SendEmail(EmailDto request);
    }
}
