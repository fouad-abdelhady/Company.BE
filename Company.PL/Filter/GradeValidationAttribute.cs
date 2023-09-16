using Company.BL.Dtos.TaskDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Company.PL.Filter
{
    public class GradeValidationAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var taskState = context.ActionArguments["taskGradeUpdate"] as TaskStateUpdate;
            if (taskState.State < 0)
            {
                context.ModelState.AddModelError("Grade", "Invalid Grade");
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
    }
}
