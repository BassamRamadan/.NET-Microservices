using System.Net.Http;
using System.Text;
using System.Text.Json;
using PlatformService.Dtos;

namespace PlatformService.SyncDataToServices.Http
{
    public class HttpCommandServiceClient : ICommandServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpCommandServiceClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        
        public async Task SendPlatformToCommand(PlatformReadDto platform)
        {
            // Implementation for sending platform data to command service
            var httpPlatfomContent = new StringContent(
                JsonSerializer.Serialize(platform),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync($"{_configuration["CommandService"]}/api/c/platforms/", httpPlatfomContent);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("[PlatformService] Sent Platform to CommandService ");
            }
            else
            {
                Console.WriteLine("[PlatformService] Failed to send Platform");
            }

        }
    }
}