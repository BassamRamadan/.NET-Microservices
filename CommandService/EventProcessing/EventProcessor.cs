using System.Text.Json;
using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;

namespace CommandService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory serviceScopeFactory, IMapper mapper)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _mapper = mapper;
        }

        public void ProcessEvent(string message)
        {
            // Logic to process the event message
            Console.WriteLine($"Processing event: {message}");
            // Additional processing logic can be added here
            var EventType = DetermineEvent(message);

            switch (EventType)
            {
                case EventType.platformPublishedDto:
                    AddPlaform(message);
                    break;
                default:
                    Console.WriteLine($"Event type {EventType} is not recognized.");
                    break;
            }
        }

        private EventType DetermineEvent(string message)
        {
            var eventType = JsonSerializer.Deserialize<PlatformPublishedDto>(message)?.Event ?? "undetermined";

            switch (eventType)
            {
                case "Platform_Published":
                    return EventType.platformPublishedDto;
                default:
                    return EventType.undetermined;
            }
        }

        private void AddPlaform(string message)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var myCommandRepo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();

                var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(message);
                try
                {
                    var plat = _mapper.Map<Platform>(platformPublishedDto);
                    if(!myCommandRepo.ExternalPlatformExists(plat.ExternalPlatformId))
                    {
                        myCommandRepo.CreatePlatform(plat);
                        myCommandRepo.SaveChanges();
                        Console.WriteLine("Platform added successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Platform already exists.");
                    }
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"Error deserializing message: {ex.Message}");
                    return;
                }
            }
        }
    }

    enum EventType
    {
        platformPublishedDto,
        undetermined
    }
}