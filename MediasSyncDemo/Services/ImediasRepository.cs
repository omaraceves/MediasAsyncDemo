using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediasAsyncDemo.Services
{
    public interface IMediasRepository
    {
        IEnumerable<Entities.Media> GetMedias();

        Entities.Media GetMedia(Guid id);

        Task<IEnumerable<Entities.Media>> GetMediasAsync();

        Task<Entities.Media> GetMediaAsync(Guid id);

        void AddMedia(Entities.Media mediaToAdd);

        Task<bool> SaveChangesAsync();
    }
}
