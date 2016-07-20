namespace Web
{
    using System.Web.Http;
    using Newtonsoft.Json;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            var formatter = config.Formatters.JsonFormatter;
            formatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        }
    }
}
