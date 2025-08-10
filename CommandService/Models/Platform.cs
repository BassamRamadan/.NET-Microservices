using System.Collections.Generic;

namespace CommandService.Models
{
    public class Platform
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public int ExternalPlatformId { get; set; }

        public ICollection<Command> Commands { get; set; } = new List<Command>(); // Navigation property for related Commands
    }
}