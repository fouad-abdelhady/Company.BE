using Company.BL.Dtos.TaskDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Company.PL.Filter
{
    public class StatusValidation : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var taskState = context.ActionArguments["taskStatusUpdate"] as TaskStateUpdate;
            if (taskState.State <= 1) {
                context.ModelState.AddModelError("Status", "You are not permitted to perform this Action");
                context.Result = new UnauthorizedObjectResult(context.ModelState);
            }
        }
    }
}
