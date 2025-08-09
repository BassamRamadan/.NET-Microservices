using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.SyncDataToServices.Http;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddDbContext<AppDbContext>(options =>
options.UseInMemoryDatabase("InMem"));

builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpClient<ICommandServiceClient, HttpCommandServiceClient>();

var app = builder.Build();

PrebDb.PrepPopulation(app, builder.Environment.IsProduction());

app.MapControllers();

app.Run();