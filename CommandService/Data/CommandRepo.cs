using CommandService.Models;

namespace CommandService.Data
{
    public class CommandRepo : ICommandRepo
    {
        private readonly AppDbContext _context;

        public CommandRepo(AppDbContext context)
        {
            _context = context;
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms.ToList();
        }

        public Platform? GetPlatformById(int id)
        {
            return _context.Platforms.FirstOrDefault(p => p.Id == id);
        }

        public void CreatePlatform(Platform platform)
        {
            if (platform == null) throw new ArgumentNullException(nameof(platform));
            _context.Platforms.Add(platform);
        }

        public void DeletePlatform(Platform platform)
        {
            if (platform == null) throw new ArgumentNullException(nameof(platform));
            _context.Platforms.Remove(platform);
        }

        public IEnumerable<Command> GetCommandsForPlatform(int platformId)
        {
            return _context.Commands.Where(c => c.PlatformId == platformId).ToList();
        }

        public Command? GetCommandById(int platformId, int commandId)
        {
            return _context.Commands.FirstOrDefault(c => c.Id == commandId && c.PlatformId == platformId);
        }

        public void CreateCommand(int platformId, Command command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            command.PlatformId = platformId;
            _context.Commands.Add(command);
        }

        public void DeleteCommand(Command command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            _context.Commands.Remove(command);
        }
    }
}