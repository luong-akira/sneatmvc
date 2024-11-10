using System.Web.Http;
using WebActivatorEx;
using Sneat.MVC;
using Swashbuckle.Application;
using System.IO;
using System.Reflection;
using System;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace Sneat.MVC
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, "bin", xmlFile);
            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "Sneat.MVC");
                        c.ApiKey("token")
                          .Description("Token Authentication")
                          .Name("token")
                          .In("header");
                        c.IncludeXmlComments(xmlPath);

                    })
                .EnableSwaggerUi(c =>
                    {
                        c.EnableApiKeySupport("token", "header");
                    });
        }
    }
}
