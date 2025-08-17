using CommandService.Models;

namespace CommandService.Data
{
    public interface ICommandRepo
    {
        bool SaveChanges();

        IEnumerable<Platform> GetAllPlatforms();
        Platform? GetPlatformById(int id);
        void CreatePlatform(Platform platform);
        void DeletePlatform(Platform platform);
        bool PlatformExists(int platformId);
        bool ExternalPlatformExists(int externalPlatformId);

        IEnumerable<Command> GetCommandsForPlatform(int platformId);
        Command? GetCommandById(int platformId, int commandId);
        void CreateCommand(int platformId, Command command);
        void DeleteCommand(Command command);
    }
}