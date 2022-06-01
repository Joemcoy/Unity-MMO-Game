using tFramework.Data.Interfaces;

namespace PiMMORPG.WebServer
{
    public class WebConfiguration : IConfiguration
    {
        string IConfiguration.Filename => "web.cfg";
        bool IConfiguration.Secure => false;

        public string Url { get; set; } = "http://localhost:8080/";
        public string Template { get; set; } = "v1";
    }
}