using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;
using NLayer.Web.Models;

namespace NLayer.Web.Filters
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
            if (entityIdValue == null)
            {
                await next();
                return;
            }

            var entityId = (int)entityIdValue;
            var anyEntity = await _service.AnyAsync(x => x.Id == entityId);
            if (anyEntity)
            {
                await next();
                return;
            }

            var errorViewModel = new ErrorViewModel();
            errorViewModel.Errors.Add($"{typeof(T).Name} not found with {entityId} id");
            context.Result = new RedirectToActionResult("Error", "Home", errorViewModel);
        }
    }
}
