using CommandService.Data;
using CommandService.EventProcessing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext <AppDbContext>(Options => Options.UseInMemoryDatabase("InMemoryDb"));

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<ICommandRepo, CommandRepo>();
builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
var app = builder.Build();

app.MapControllers();

app.Run();