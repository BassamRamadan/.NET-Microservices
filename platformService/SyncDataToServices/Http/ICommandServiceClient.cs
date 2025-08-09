using PlatformService.Dtos;

namespace PlatformService.SyncDataToServices.Http
{
    public interface ICommandServiceClient
    {
        public Task SendPlatformToCommand(PlatformReadDto platform);
    }
}