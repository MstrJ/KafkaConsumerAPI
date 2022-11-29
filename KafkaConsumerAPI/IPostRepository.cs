namespace KafkaConsumerAPI
{
    public interface IPostRepository 
    {
        public Task AddPost();
        public IEnumerable<Post> GetAll();
    }
}
