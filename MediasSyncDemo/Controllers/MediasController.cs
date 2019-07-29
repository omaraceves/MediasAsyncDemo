using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediasAsyncDemo.Filters;
using MediasAsyncDemo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static MediasAsyncDemo.Filters.MediasResultFilterAttribute;

namespace MediasAsyncDemo.Controllers
{
    [Route("api/medias")]
    [ApiController]
    public class MediasController : ControllerBase
    {
        private IMediasRepository _mediasRepository;

        public MediasController(IMediasRepository mediasRepository)
        {
            _mediasRepository = mediasRepository ?? throw new ArgumentNullException(nameof(mediasRepository));
        }

        [HttpGet]
        [MediaResultFilter]
        [Route("{id}")]
        public async Task<IActionResult> GetMedia(Guid id)
        {
            var mediaEntity = await _mediasRepository.GetMediaAsync(id);
            if(mediaEntity == null)
            {
                return NotFound();
            }
            return Ok(mediaEntity);
        }

        [HttpGet]
        [MediasResultFilter]
        public async Task<IActionResult> GetMedias()
        {
            var mediaEntities = await _mediasRepository.GetMediasAsync();
            return Ok(mediaEntities);
        }
    }
}