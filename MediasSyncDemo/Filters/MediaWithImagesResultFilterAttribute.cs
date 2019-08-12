using AutoMapper;
using MediasAsyncDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediasAsyncDemo.Filters
{
    public class MediaWithImagesResultFilterAttribute : ResultFilterAttribute
    {
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var resultoFromAction = context.Result as ObjectResult;

            if (resultoFromAction?.Value == null || resultoFromAction.StatusCode < 200
                || resultoFromAction.StatusCode >= 300)
            {
                await next();
                return;
            }

            var (media, mediaImages) = ((Entities.Media, 
                IEnumerable<ExternalModels.MediaImage>))resultoFromAction.Value;

            var mappedMediaWithImages = Mapper.Map<MediaWithImages>(media);

            resultoFromAction.Value = Mapper.Map(mediaImages, mappedMediaWithImages);

            await next();
        }
    }
}
