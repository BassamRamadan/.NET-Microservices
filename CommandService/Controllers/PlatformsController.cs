using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using Microsoft.AspNetCore.Mvc;
using System;
namespace CommandsService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly ICommandRepo _repo;
        private readonly IMapper _mapper;

        public PlatformsController(ICommandRepo repo, IMapper mapper)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            var platforms = _repo.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            var platform = _repo.GetPlatformById(id);
            if (platform == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<PlatformReadDto>(platform));
        }

        [HttpGet("{id}/commands")]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int id)
        {
            var commands = _repo.GetCommandsForPlatform(id);
            if (commands == null || !commands.Any())
            {
                return NotFound();
            }
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
        }

        [HttpGet("{id}/commands/{commandId}", Name = "GetCommandById")]
        public ActionResult<CommandReadDto> GetCommandById(int id, int commandId)
        {
            var command = _repo.GetCommandById(id, commandId);
            if (command == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<CommandReadDto>(command));
        }

        [HttpPost("{id}/commands")]
        public ActionResult<CommandReadDto> CreateCommandForPlatform(int id, CommandCreateDto commandCreateDto)
        {
            Console.WriteLine($"--> Received Platform via POST for platform:{id}");
            var platform = _repo.GetPlatformById(id);
            if (platform == null)
            {
                return NotFound();
            }

            var command = _mapper.Map<Command>(commandCreateDto);
            command.PlatformId = id;

            _repo.CreateCommand(id, command);
            _repo.SaveChanges();

            var commandReadDto = _mapper.Map<CommandReadDto>(command);
            return CreatedAtRoute("GetCommandById", new { id = id, commandId = commandReadDto.Id }, commandReadDto);
        }

        [HttpPost]
        public ActionResult ReceivePlatform()
        {
            Console.WriteLine("--> Received Platform via POST");
            return Ok("platforms");
        }
    }
}