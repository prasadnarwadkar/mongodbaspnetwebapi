using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MongoDbASPNetWebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Attribute routing.
            config.MapHttpAttributeRoutes();

            config.EnableCors();
            config.MessageHandlers.Add(new PreflightRequestsHandler());



            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            
        }
    }
}
