using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediasAsyncDemo.Filters;
using MediasAsyncDemo.Models;
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
        private IMapper _mapper;

        public MediasController(IMediasRepository mediasRepository, IMapper mapper)
        {
            _mediasRepository = mediasRepository ?? throw new ArgumentNullException(nameof(mediasRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [MediaWithImagesResultFilter]
        [Route("{id}", Name = "GetMedia")]
        public async Task<IActionResult> GetMedia(Guid id)
        {
            var mediaEntity = await _mediasRepository.GetMediaAsync(id);
            if(mediaEntity == null)
            {
                return NotFound();
            }

            var mediaImagesCollection = await _mediasRepository.GetMediaImagesAsync(id);

            return Ok((mediaEntity, mediaImagesCollection));
        }

        [HttpGet]
        [MediasResultFilter]
        public async Task<IActionResult> GetMedias()
        {
            var mediaEntities = await _mediasRepository.GetMediasAsync();
            return Ok(mediaEntities);
        }

        [HttpPost]
        [MediaResultFilter]
        public async Task<IActionResult> CreateMedia([FromBody] MediaForCreation media)
        {
            Entities.Media mediaToAdd = _mapper.Map<Entities.Media>(media);
            _mediasRepository.AddMedia(mediaToAdd);

            await _mediasRepository.SaveChangesAsync();

            return CreatedAtRoute("GetMedia", new { id = mediaToAdd.Id }, mediaToAdd);
        }
    }
}