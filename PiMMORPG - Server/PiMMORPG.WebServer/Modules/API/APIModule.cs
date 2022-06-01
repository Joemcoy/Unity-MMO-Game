using Nancy;

namespace PiMMORPG.WebServer.Modules.API
{
    public abstract class APIModule : NancyModule
    {
        public APIModule(string path) : base("/api/" + path)
        {
        }
    }
}