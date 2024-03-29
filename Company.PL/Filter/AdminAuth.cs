﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Company.PL.Filter
{
    public class AdminAuth : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userClaims = context.HttpContext.User.Claims;
            var role = userClaims.FirstOrDefault(c => c.Type == "Role")!.Value;
            Console.WriteLine(role);
            if (role.ToLower() != "admin") {
                context.ModelState.AddModelError("auth", "You are not Authorized");
                context.Result = new UnauthorizedObjectResult(context.ModelState);
            }
        }
    }
}
