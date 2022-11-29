using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace KafkaConsumerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KafkaConsumerController : ControllerBase
    {
        //private readonly IPostRepository _posts;
        //public KafkaConsumerController(IPostRepository postRepository)
        //{
        //    _posts = postRepository;
        //}

        private static IPostRepository _posts = new PostRepository();

        // Pierwsze uruchomienie ~ delay 30 sec
        [HttpGet("GetALL")]
        public async Task<IActionResult> GetALL()
        {
            return Ok(_posts.GetAll());
        }

    }
}