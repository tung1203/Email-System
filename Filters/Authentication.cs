using System;
using LoginSystem.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace LoginSystem.Filters
{
    public class Authentication : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {

            var userId = context.HttpContext.Session.GetInt32("userId");
            if (userId == null)
            {
                var controller = (Controller)context.Controller;
                context.Result = controller.Redirect("/Home/Login");

            }
        }

    }
}
