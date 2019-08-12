using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediasAsyncDemo.Context;
using MediasAsyncDemo.Entities;
using MediasAsyncDemo.ExternalModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MediasAsyncDemo.Services
{
    public class MediasRepository : IMediasRepository, IDisposable
    {
        private MediasContext _context;
        private IHttpClientFactory _httpClientFactory;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly ILogger<MediasRepository> _logger;

        public MediasRepository(MediasContext context, IHttpClientFactory httpClientFactory, ILogger<MediasRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Media> GetMediaAsync(Guid id)
        {
            var result =  await _context.Medias.Include(m => m.Channel).
                FirstOrDefaultAsync(m => m.Id == id);

            return result;
        }

        public async Task<IEnumerable<Media>> GetMediasAsync(IEnumerable<Guid> mediaIds)
        {
            var result = await _context.Medias.Where(m => mediaIds.Contains(m.Id)).Include(m => m.Channel).ToListAsync();

            return result;
        }

        public async Task<IEnumerable<Media>> GetMediasAsync()
        {
            await _context.Database.ExecuteSqlCommandAsync("WAITFOR DELAY '00:00:02';");
            return await _context.Medias.Include(m => m.Channel).ToListAsync();
        }

        public IEnumerable<Media> GetMedias()
        {
            _context.Database.ExecuteSqlCommand("WAITFOR DELAY '00:00:02';");
            return _context.Medias.Include(m => m.Channel).ToList();
        }

        public Media GetMedia(Guid id)
        {
            var media = _context.Medias.Include(m => m.Channel).FirstOrDefault(m => m.Id == id);
            return media;
        }



        public void AddMedia(Media mediaToAdd)
        {
            if(mediaToAdd == null)
            {
                throw new ArgumentNullException(nameof(mediaToAdd));
            }

            _context.Add(mediaToAdd);
        }

        public async Task<MediaImage> GetMediaImageAsync(string imageId)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.
                GetAsync($"http://localhost:52644/api/mediaImages/{imageId}");

            if(response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<MediaImage>(await response.Content.ReadAsStringAsync());
            }

            return null;
        }

        public async Task<IEnumerable<MediaImage>> GetMediaImagesAsync(Guid mediaId)
        {
            var client = _httpClientFactory.CreateClient();
            var mediaImages = new List<MediaImage>();
            _cancellationTokenSource = new CancellationTokenSource();

            var cancellationToken = _cancellationTokenSource.Token;

            //generate urls with mock ids
            var mediaImagesUrls = new[]
            {
                $"http://localhost:52644/api/mediaImages/{mediaId}-mockImageId1",
                $"http://localhost:52644/api/mediaImages/{mediaId}-mockImageId2",
                $"http://localhost:52644/api/mediaImages/{mediaId}-mockImageId3",
                $"http://localhost:52644/api/mediaImages/{mediaId}-mockImageId4",
                $"http://localhost:52644/api/mediaImages/{mediaId}-mockImageId5",
            };

            //create the tasks
            var downloadImagesTaskQuery =
                from mediaImageUrl
                in mediaImagesUrls
                select DownloadMediaImageAsyncMock(client, mediaImageUrl, cancellationToken);

            //start the tasks
            var downloadMediaImagesTask = downloadImagesTaskQuery.ToList(); //A Linq query doesn't executes until is evaluated.

            //return result after ALL task are completed
            return await Task.WhenAll(downloadMediaImagesTask);

        }

        private async Task<MediaImage> DownloadMediaImageAsyncMock(HttpClient client, string mediaImageurl, CancellationToken token)
        {
            return new MediaImage()
            {
                Name = mediaImageurl.Substring(49)
            };
        }

        /// <summary>
        /// This method showcases a cancelled operation after simulating a failure on mockImageId2
        /// </summary>
        /// <param name="mediaId">mediaId</param>
        /// <returns>Media Images</returns>
        public async Task<IEnumerable<MediaImage>> GetMediaImagesAsyncFaulty(Guid mediaId)
        {
            var client = _httpClientFactory.CreateClient();
            var mediaImages = new List<MediaImage>();
            _cancellationTokenSource = new CancellationTokenSource();

            var cancellationToken = _cancellationTokenSource.Token;

            //generate urls with mock ids
            var mediaImagesUrls = new[]
            {
                $"http://localhost:52644/api/mediaImages/{mediaId}-mockImageId1",
                $"http://localhost:52644/api/mediaImages/{mediaId}-mockImageId2?returnFault=true",
                $"http://localhost:52644/api/mediaImages/{mediaId}-mockImageId3",
                $"http://localhost:52644/api/mediaImages/{mediaId}-mockImageId4",
                $"http://localhost:52644/api/mediaImages/{mediaId}-mockImageId5",
            };

            //create the tasks
            var downloadImagesTaskQuery =
                from mediaImageUrl
                in mediaImagesUrls
                select DownloadMediaImageAsync(client, mediaImageUrl, cancellationToken);

            //start the tasks
            var downloadMediaImagesTask = downloadImagesTaskQuery.ToList(); //A Linq query doesn't executes until is evaluated.

            try
            {
                //return result after ALL task are completed
                return await Task.WhenAll(downloadMediaImagesTask);
            }
            catch (OperationCanceledException operationCanceledException)
            {
                _logger.LogInformation($"{operationCanceledException.Message}");
                foreach (var task in downloadMediaImagesTask)
                {
                    _logger.LogInformation($"Task {task.Id} has status {task.Status}");
                }

                return new List<MediaImage>();
            }

        }

        private async Task<MediaImage> DownloadMediaImageAsync(HttpClient client, string mediaImageurl, CancellationToken token)
        {
           
            var response = await client.
                GetAsync(mediaImageurl, token);

            if (response.IsSuccessStatusCode)
            {
              return JsonConvert.DeserializeObject<MediaImage>(
                    await response.Content.ReadAsStringAsync());
            }

            _cancellationTokenSource.Cancel();

            return null;
        }

        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                return (await _context.SaveChangesAsync() > 0);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        #region IDisposable methods
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
                if(_cancellationTokenSource != null)
                {
                    _cancellationTokenSource.Dispose();
                    _cancellationTokenSource = null;
                }
            }
        }
        #endregion
    }
}
