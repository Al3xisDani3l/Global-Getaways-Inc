using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Security.Claims;


namespace GG.Api.Controllers
{
    public class SameUserFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var controller = context.Controller as ControllerBase;



            if (!controller.User.HasClaim(c => c.Type == ClaimTypes.Sid))
            {
                context.Result = new JsonResult(new
                {
                    StatusCode = StatusCodes.Status403Forbidden,
                    Value = "Token invalido"

                });
                return;
            }

            var claim = controller.User.FindFirst(ClaimTypes.Sid);

            if (claim == null)
            {
                context.Result = new JsonResult(new
                {
                    StatusCode = StatusCodes.Status403Forbidden,
                    Value = "Token invalido"

                });
                return;
            }

            object id;
            bool Existid = controller.Request.RouteValues.TryGetValue("id", out id);

            if (id == null)
            {
                context.Result = new JsonResult(new
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Value = "No se a proporcionado un Id valido"

                });
                return;
            }

            if ((int)id != Convert.ToInt32(claim.Value))
            {

                context.Result = new JsonResult(new
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Value = "No tienes autorizacion para realizar operaciones sobre este usuario"

                });
                return;
            }



        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }
    }
}
