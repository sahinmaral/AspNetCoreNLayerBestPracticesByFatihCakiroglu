using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.API.Filters
{
    public class NotFoundFilter<T> : IAsyncActionFilter where T : BaseEntity, new()
    {
        private readonly IService<T> _service;

        public NotFoundFilter(IService<T> service)
        {
            _service = service;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var entityIdValue = context.ActionArguments.Values.FirstOrDefault();
            if(entityIdValue == null)
            {
                await next();
                return;
            }

            if(entityIdValue is IEnumerable<int>)
            {
                var entityIds = (IEnumerable<int>)entityIdValue;

                List<int> notFoundEntities = new List<int>();
                foreach (var entityId in entityIds)
                {
                    var anyEntity = await _service.AnyAsync(x => x.Id == entityId);
                    if(!anyEntity) notFoundEntities.Add(entityId);
                }

                if (notFoundEntities.Count == 0)
                {
                    await next();
                    return;
                }

                context.Result = new NotFoundObjectResult(CustomResponseDto<NoContentDto>.Fail(404, $"{typeof(T).Name} not found with [{string.Join(",", notFoundEntities)}] ids"));
            }
            else
            {
                var entityId = (int)entityIdValue;
                var anyEntity = await _service.AnyAsync(x => x.Id == entityId);
                if (anyEntity)
                {
                    await next();
                    return;
                }

                context.Result = new NotFoundObjectResult(CustomResponseDto<NoContentDto>.Fail(404, $"{typeof(T).Name} not found with {entityId} id"));
            }
            

        }
    }
}
