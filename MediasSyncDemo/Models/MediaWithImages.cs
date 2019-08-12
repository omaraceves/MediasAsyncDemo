using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediasAsyncDemo.Models
{
    public class MediaWithImages : Media
    {
        public IEnumerable<MediaImage> MediaImages { get; set; } = new List<MediaImage>();
    }
}
