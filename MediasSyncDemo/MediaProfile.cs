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
                .ForMember(m => m.Channel, opt => opt.MapFrom(src => $"{src.Channel.Type}: {src.Channel.Name}"));  
                
        }

    }
}
