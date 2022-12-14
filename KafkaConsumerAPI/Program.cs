using Confluent.Kafka;
using KafkaConsumerAPI;
using KafkaConsumerAPI.Services.EmailService;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IPostRepository, PostRepository>();
builder.Services.BuildServiceProvider().GetRequiredService<IPostRepository>(); // kiedy to jest off to swagger sie nie odpala jeszcze, html nie dziala
builder.Services.AddScoped<IEmailService, EmailService>();
var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.Run();

