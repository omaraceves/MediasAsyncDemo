using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediasAsyncDemo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediasAsyncDemo.Controllers
{
    [Route("api/sync-medias")]
    [ApiController]
    public class SyncMediasController : ControllerBase
    {
        private IMediasRepository _mediasRepository;

        public SyncMediasController(IMediasRepository mediasRepository)
        {
            _mediasRepository = mediasRepository ?? throw new ArgumentNullException(nameof(mediasRepository));
        }

        [HttpGet]
        public IActionResult GetMediasSync()
        {
            var medias = _mediasRepository.GetMedias();
            return Ok(medias);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetMediaSync(Guid id)
        {
            var media = _mediasRepository.GetMedia(id);
            if(media == null)
            {
                return NotFound();
            }
            return Ok(media);
        }
    }
}