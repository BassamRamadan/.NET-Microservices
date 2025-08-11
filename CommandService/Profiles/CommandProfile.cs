namespace CommandService.Profiles
{
    using AutoMapper;
    using CommandService.Dtos;
    using CommandService.Models;

    public class CommandProfile : Profile
    {
        public CommandProfile()
        {
            // Source -> Target
            CreateMap<Command, CommandReadDto>();
            CreateMap<CommandCreateDto, Command>();
            CreateMap<Platform, PlatformReadDto>();

            CreateMap<PlatformPublishedDto, Platform>()
                .ForMember(dest => dest.ExternalPlatformId, opt => opt.MapFrom(src => src.Id));

        }
    }
}