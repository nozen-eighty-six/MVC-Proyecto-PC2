using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc; //Para usar herencia

namespace CapaPresentacionTienda.Filter
{
    public class ValidarSesionAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["Cliente"] == null)
            {
                filterContext.Result = new RedirectResult("~/Acceso/Index");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}