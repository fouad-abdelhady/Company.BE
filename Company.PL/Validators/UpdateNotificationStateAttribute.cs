using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Company.PL.Validators
{
    public class UpdateNotificationStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.Request.Query.TryGetValue("notificationId", out var notificationIdStr))
            {
                _getResponse(context, "Task Id is required");
                return;
            }
            if (!int.TryParse(notificationIdStr, out int notificationId))
            {
                _getResponse(context, "Only numbers are accepted");
                return;
            }
        }
        private void _getResponse(ActionExecutingContext context, string message)
        {
            context.ModelState.AddModelError("BadReq", message);
            context.Result = new BadRequestObjectResult(context.ModelState);
        }
    }
}
