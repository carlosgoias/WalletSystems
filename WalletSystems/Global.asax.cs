using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace WalletSystems
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            UnityConfig.RegisterComponents();
        }

        protected void Application_BeginRequest()
        {
            if (HttpContext.Current.Request.Url.AbsolutePath == "/")
            {
                HttpContext.Current.Response.Redirect("/swagger/ui/index");
            }
        }
    }
}
