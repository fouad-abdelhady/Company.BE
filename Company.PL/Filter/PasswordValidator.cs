using Company.BL.Dtos.AuthDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Company.PL.Filter
{
    public class PasswordValidator: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var body = context.ActionArguments["newPassword"] as PasswordUpdateReq;
            if (body == null) {
                context.ModelState.AddModelError("body", "Empty Body");
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
    }
}
