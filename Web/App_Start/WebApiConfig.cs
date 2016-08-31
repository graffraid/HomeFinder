namespace Web
{
    using System.Web.Http;
    using System.Web.Http.ExceptionHandling;
    using Elmah.Contrib.WebApi;
    using Newtonsoft.Json;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Services.Add(typeof(IExceptionLogger), new ElmahExceptionLogger());
            var formatter = config.Formatters.JsonFormatter;
            formatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        }
    }
}
