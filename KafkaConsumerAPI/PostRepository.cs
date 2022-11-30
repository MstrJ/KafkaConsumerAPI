using Confluent.Kafka;
using KafkaConsumerAPI.Models;
using KafkaConsumerAPI.Services.EmailService;
using Newtonsoft.Json;

namespace KafkaConsumerAPI
{
    public class PostRepository : IPostRepository
    {
        private static ISet<Post> _posts = new HashSet<Post>();

        public PostRepository()
        {
            Task.Run(() => AddPost());
        }


        public IEnumerable<Post> GetAll()
        {
            return _posts;
        }


        public async Task AddPost()
        {
            var config = new ConsumerConfig
            {
                //BootstrapServers = "kafka.host:9092",
                //BootstrapServers = "kafka.docker.internal:9092",
                //BootstrapServers = "host.docker.internal:9092",
                //BootstrapServers = "host.docker.internal:9094",
                //BootstrapServers = "localhost:9092",
                //BootstrapServers = "kafka:9094",
                //BootstrapServers = "localhost:9094", // dziala w vs
                BootstrapServers = "kafka:9092",

                //SecurityProtocol = SecurityProtocol.Ssl,
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
                        var message = consumeResult.Message.Value;
                        consumer.Commit(consumeResult);
                        var result = JsonConvert.DeserializeObject<Post>(message);

                        _posts.Add(result);
                        Console.WriteLine(message);
                    }
                    catch (KafkaException e)
                    {
                        Console.WriteLine($"Commit error: {e.Error.Reason}");
                    }
               }
                consumer.Close();
           }
        }
    }
}
