using Confluent.Kafka;
using KafkaConsumerAPI;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//-------------------------

//builder.Services.AddScoped<IPostRepository, PostRepository>();

var app = builder.Build();


// Configure the HTTP request pipeline.
app.UseSwagger();
    app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// -=---------------------
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

            //_posts.Add(result);
            //return Ok(_posts);
            Console.WriteLine(message);

        }
        catch (KafkaException e)
        {
            Console.WriteLine($"Commit error: {e.Error.Reason}");
        }
    }
    consumer.Close();
}

//-----------------------------------