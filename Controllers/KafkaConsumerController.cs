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

        private static ISet<Post> _posts = new HashSet<Post>();

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9094",
                GroupId = "newPost",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };


            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumer.Subscribe(new string[] { "addedPosts" });
                bool cancelled = false;
                while (!cancelled)
                {
                    var consumeResult = consumer.Consume();

                    try
                    {
                        var message = consumeResult.Value.ToString();
                        consumer.Commit(consumeResult);

                        Console.WriteLine($"{message}");
                        var result = JsonConvert.DeserializeObject<Post>(message);

                        _posts.Add(result);
                        //return Ok(_posts);
                        Console.WriteLine(message);
                        return Ok(_posts);

                    }
                    catch (KafkaException e)
                    {
                        Console.WriteLine($"Commit error: {e.Error.Reason}");
                    }
                }
                consumer.Close();
            }
            return BadRequest("cos nie dziala ;/");
        }

    }
}