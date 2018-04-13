using System.Web.Http;
using System.Net.Http.Formatting;

namespace RecordParser.WebApp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Enable Cross-Origin Requests
            config.EnableCors();
            
            // Web API routes
            config.MapHttpAttributeRoutes();

            // set JSON output formatter
            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());
        }
    }
}
