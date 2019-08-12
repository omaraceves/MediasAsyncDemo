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

namespace MediasAsyncDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaCollectionsController : ControllerBase
    {
        private readonly IMediasRepository _repository;
        private readonly IMapper _mapper;

        public MediaCollectionsController(IMediasRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        //api/mediaCollections/(id1, id2)
        [HttpGet("({bookIds})", Name = "GetBookCollection")]
        [MediasResultFilter]
        public async Task<IActionResult> GetBookCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> bookIds)
        {
            var mediaEntities = await _repository.GetMediasAsync(bookIds);

            if(bookIds.Count() != mediaEntities.Count())
            {
                return NotFound();
            }

            return Ok(mediaEntities);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMediaCollection([FromBody] IEnumerable<MediaForCreation> mediaCollection)
        {
            var mediaEntities = Mapper.Map<IEnumerable<Entities.Media>>(mediaCollection);

            foreach(var mediaEntity in mediaEntities)
            {
                _repository.AddMedia(mediaEntity);
            }

            await _repository.SaveChangesAsync();

            return Ok(mediaEntities);
        }

    }
}