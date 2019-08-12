using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediasAsyncDemo
{
    public class MediaProfile : Profile
    {
        public MediaProfile()
        {
            CreateMap<Entities.Media, Models.Media>()
                .ForMember(m => m.Channel, 
                opt => opt.MapFrom(src => $"{src.Channel.Type}: {src.Channel.Name}"));

            CreateMap<Models.MediaForCreation, Entities.Media>();

            CreateMap<Entities.Media, Models.MediaWithImages>()
               .ForMember(m => m.Channel,
               options => options.MapFrom(src => $"{src.Channel.Type}: {src.Channel.Name}"));

            CreateMap<IEnumerable<ExternalModels.MediaImage>, Models.MediaWithImages>()
                .ForMember(dest => dest.MediaImages, options => options.MapFrom(src => src));

        }

    }
}
