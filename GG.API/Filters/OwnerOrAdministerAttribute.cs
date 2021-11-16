using GG.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Security.Claims;

namespace GG.Api.Filters
{
    public class OwnerOrAdministerAttribute : ActionFilterAttribute
    {

        public Type TypeOfRepository { get; set; }






        public OwnerOrAdministerAttribute(Type typeOfRepository)
        {
            this.TypeOfRepository = typeOfRepository;


        }







        public override void OnActionExecuting(ActionExecutingContext context)
        {


            var sid = context.HttpContext.User.FindFirst(ClaimTypes.Sid).Value;
            var role = context.HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            string idencabezado;
            try
            {
                idencabezado = context.HttpContext.Request.RouteValues["id"].ToString();
            }
            catch (NullReferenceException err)
            {

                idencabezado = null;
            }
           
            var _repository = context.HttpContext.RequestServices.GetService(TypeOfRepository);
            var ActionParameter = context.ActionArguments;


            if (role == Enum.GetName(typeof(RoleType), RoleType.Administrator) || role == Enum.GetName(typeof(RoleType), RoleType.Programmer))
            {
                return;
            }

            if (_repository is IRepository<PrivateUser,string>)
            {
                var _userRepository = _repository as IRepository<PrivateUser,string>;



                if (!string.IsNullOrEmpty(idencabezado))
                {
                    if (sid == idencabezado)
                    {
                        return;
                    }
                    else
                    {
                        context.Result = new UnauthorizedObjectResult("");
                    }

                }

                else if (ActionParameter.Count > 0)
                {

                    var parameter = context.ActionArguments["entity"] as User;

                    if (parameter.Id.ToString() != sid)
                    {
                        context.Result = new UnauthorizedObjectResult("");
                    }



                }






            }

            //if (_repository is IRepository<Orden,int>)
            //{
            //    var _ordenRepository = _repository as IRepository<Orden,int>;
            //    if (!string.IsNullOrEmpty(idencabezado))
            //    {

            //        Orden orden = _ordenRepository.Find(o => o.IdUser.ToString() == idencabezado);

            //        if (orden == null)
            //        {
            //            return;
            //        }

            //        if (sid == orden.IdUser.ToString())
            //        {
            //            return;
            //        }
            //        else
            //        {
            //            context.Result = new UnauthorizedObjectResult("No tienes autorizacion para modificar esta entidad!");
            //        }

            //    }

            //    else if (ActionParameter.Count > 0)
            //    {

            //        var parameter = context.ActionArguments["entity"] != null ? context.ActionArguments["entity"] as OrdenDto : context.ActionArguments["administer"] as OrdenDto;

            //        Orden orden = _ordenRepository.Find(o => o.IdUser == parameter.Id);

            //        if (orden.IdUser != parameter.IdUser)
            //        {
            //            context.Result = new UnauthorizedObjectResult("No puedes modificar el propietario de una entidad!");
            //        }

            //        if (parameter.IdUser.ToString() != sid)
            //        {
            //            context.Result = new UnauthorizedObjectResult("No tienes autorizacion para modificar esta entidad!");
            //        }



            //    }

            //}

            //if (_repository is IRepository<OrdenComment,int>)
            //{
            //    var _ordenRepository = _repository as IRepository<OrdenComment,int>;
            //    if (!string.IsNullOrEmpty(idencabezado))
            //    {

            //        OrdenComment orden = _ordenRepository.Find(o => o.IdUser.ToString() == idencabezado);

            //        if (orden == null)
            //        {
            //            return;
            //        }

            //        if (sid == orden.IdUser.ToString())
            //        {
            //            return;
            //        }
            //        else
            //        {
            //            context.Result = new UnauthorizedObjectResult("No tienes autorizacion para modificar esta entidad!");
            //        }

            //    }

            //    else if (ActionParameter.Count > 0)
            //    {

            //        var parameter = context.ActionArguments["entity"] != null ? context.ActionArguments["entity"] as OrdenCommentDto : context.ActionArguments["administer"] as OrdenCommentDto;

            //        OrdenComment orden = _ordenRepository.Find(o => o.IdUser == parameter.Id);

            //        if (orden.IdUser != parameter.IdUser)
            //        {
            //            context.Result = new UnauthorizedObjectResult("No puedes modificar el propietario de una entidad!");
            //        }

            //        if (parameter.IdUser.ToString() != sid)
            //        {
            //            context.Result = new UnauthorizedObjectResult("No tienes autorizacion para modificar esta entidad!");
            //        }



            //    }

            //}




        }





    }

   // [System.AttributeUsage(AttributeTargets.Method)]
    //public class OwnerOrdenAttribute : OwnerOrAdministerAttribute
    //{
    //    public OwnerOrdenAttribute() : base(typeof(IRepository<Orden,int>))
    //    {
    //    }
    //}

    //public class OwnerUserAttribute : OwnerOrAdministerAttribute
    //{
    //    public OwnerUserAttribute() : base(typeof(IRepository<PrivateUser,string>))
    //    {
    //    }
    //}

    //public class OwnerCommentAttribute : OwnerOrAdministerAttribute
    //{
    //    public OwnerCommentAttribute() : base(typeof(IRepository<OrdenComment,int>))
    //    {
    //    }
    //}
}
