using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediasAsyncDemo.Context;
using MediasAsyncDemo.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediasAsyncDemo.Services
{
    public class MediasRepository : IMediasRepository, IDisposable
    {
        private MediasContext _context;

        public MediasRepository(MediasContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Media> GetMediaAsync(Guid id)
        {
            var result =  await _context.Medias.Include(m => m.Channel).
                FirstOrDefaultAsync(m => m.Id == id);

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
            }
        }

        
    }
}
