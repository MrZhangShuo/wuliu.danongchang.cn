using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace LogisticsApi
{
    public static class WebApiConfig
    {
        public static void Register(System.Web.Http.HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
               name: "DefaultApi",
               routeTemplate: "api/{controller}/{id}",
               defaults: new { id = RouteParameter.Optional }
           );
        }
    }
}
