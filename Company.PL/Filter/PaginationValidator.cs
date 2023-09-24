using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Company.PL.Filter
{
    public class PaginationValidator : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            
            if (!context.HttpContext.Request.Query.TryGetValue("page", out var pageValue) || 
                !context.HttpContext.Request.Query.TryGetValue("limit", out var limitValue)) {
                _getResponse(context, "Page or Limit cannot be null");
                return;
            }
            if (!int.TryParse(pageValue, out int page) || !int.TryParse(limitValue, out int limit)) {
                _getResponse(context, "Page or Limit cannot be characters");
                return;
            }
            if (page < 1 || limit < 1) _getResponse(context, "Page or Limit cannot be less than 1");
            
             
        }
        private void _getResponse(ActionExecutingContext context, string message) {
            context.ModelState.AddModelError("BatReq", message);
            context.Result = new BadRequestObjectResult(context.ModelState);
        }
    }
}
