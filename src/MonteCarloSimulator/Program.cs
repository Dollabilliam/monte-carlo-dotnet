using MonteCarloSimulator;
using MonteCarloSimulator.Queues;
using MonteCarloSimulator.Queues.Messages;
using MonteCarloSimulator.Result;
using MonteCarloSimulator.ScenarioProcessor;
using MonteCarloSimulator.Status;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSingleton<IEnqueueQueue<QueueMessage>, RequestQueue>();
builder.Services.AddSingleton<IDequeueQueue<QueueMessage>, RequestQueue>();

builder.Services.AddSingleton<IGetStatusRepository, StatusRepository>();
builder.Services.AddSingleton<ISetStatusRepository, StatusRepository>();

builder.Services.AddSingleton<IGetResultRepository, ResultRepository>();
builder.Services.AddSingleton<ISetResultRepository, ResultRepository>();

builder.Services.AddTransient<IScenarioProcessor, ScenarioProcessor>();

builder.Services.AddHostedService<QueueWorker>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();