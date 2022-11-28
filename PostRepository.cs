using Confluent.Kafka;
using Newtonsoft.Json;

namespace KafkaConsumerAPI
{
    public class PostRepository : IPostRepository
    {
        public static ISet<Post> posts = new HashSet<Post>();

        public void AddPost(Post post)
        {
            posts.Add(post);
        }
    }
}
