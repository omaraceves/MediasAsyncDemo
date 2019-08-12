using MediasAsyncDemo.ExternalModels;
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

        Task<IEnumerable<Entities.Media>> GetMediasAsync(IEnumerable<Guid> mediaIds);

        void AddMedia(Entities.Media mediaToAdd);

        Task<bool> SaveChangesAsync();

        Task<MediaImage> GetMediaImageAsync(string imageId);

        Task<IEnumerable<MediaImage>> GetMediaImagesAsync(Guid mediaId);

    }
}
