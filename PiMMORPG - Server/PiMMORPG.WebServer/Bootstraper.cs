using System;
using System.IO;
using System.Linq;
using System.Dynamic;

using Nancy;
using Nancy.Session;
using Nancy.TinyIoc;
using Nancy.Responses;
using Nancy.Conventions;
using Nancy.Bootstrapper;
using Nancy.Authentication.Basic;
using Nancy.Authentication.Forms;
using Nancy.Authentication.Stateless;
using tFramework.Factories;
using Nancy.Cryptography;

namespace PiMMORPG.WebServer
{
    public class Bootstraper : DefaultNancyBootstrapper, IRootPathProvider
    {
        private CryptographyConfiguration cryptographyConfiguration = new CryptographyConfiguration
        (
            new RijndaelEncryptionProvider(new PassphraseKeyGenerator("SuperSecretPass", new byte[] { 2, 0xFF, 3, 4, 5, 6, 7, 8 })),
            new DefaultHmacProvider(new PassphraseKeyGenerator("UberSuperSecure", new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 }))
        );

        protected override IRootPathProvider RootPathProvider
        {
            get { return this; }
        }

        public string GetRootPath()
        {
            var dir = Path.Combine(Environment.CurrentDirectory, string.Format("Content/{0}", WebServer.Configuration.Template));
            dir = dir.Replace('/', Path.DirectorySeparatorChar);

            if (dir.Last() != Path.DirectorySeparatorChar)
                dir += Path.DirectorySeparatorChar;

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            return dir;
        }

        protected override void ConfigureConventions(NancyConventions conventions)
        {
            var bdir = GetRootPath();
            foreach (var dir in Directory.GetDirectories(bdir, "*.*", SearchOption.TopDirectoryOnly))
            {
                ConfigureDir(bdir, dir, conventions);
            }
        }

        void ConfigureDir(string bdir, string dir, NancyConventions conventions)
        {
            var hDir = dir.Replace(bdir, string.Empty);
            conventions.StaticContentsConventions.AddDirectory(hDir, hDir);

            LoggerFactory.GetLogger(this).LogSuccess("{0} => {1}", hDir, hDir);
            if (hDir.First() == Path.DirectorySeparatorChar)
            {
                hDir = hDir.Substring(1);
                conventions.StaticContentsConventions.AddDirectory(hDir, hDir);
                LoggerFactory.GetLogger(this).LogSuccess("{0} => {1}", hDir, hDir);
            }

            var cdirs = Directory.GetDirectories(dir, "*.*", SearchOption.TopDirectoryOnly);
            if (cdirs.Length > 0)
                foreach (var cdir in cdirs)
                    ConfigureDir(bdir, cdir, conventions);
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            pipelines.AfterRequest.AddItemToEndOfPipeline((ctx) =>
            {
                if (ctx.Response.ContentType == "text/html")
                    ctx.Response.ContentType = "text/html; charset=utf-8";
            });

            pipelines.OnError.AddItemToEndOfPipeline((ctx, e) =>
            {
                LoggerFactory.GetLogger<NancyEngine>().LogFatal(e);

                var body = string.Format("<span>{0}</span><br/>", e.Message);
                body += string.Format("<b>{0}</b><br/>", e.StackTrace);
                body = body.Replace(Environment.NewLine, "<br/>");

                return new HtmlResponse()
                {
                    ContentType = "text/html; charset=utf-8",
                    Contents = stream =>
                    {
                        using (var writer = new StreamWriter(stream)) writer.WriteLine(body);
                    }
                };
            });

            CookieBasedSessions.Enable(pipelines);
            //pipelines.EnableBasicAuthentication(new BasicAuthenticationConfiguration(container.Resolve<IUserValidator>(), "WebServer"));
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);

            if (!context.Request.Url.Path.StartsWith("/api"))
                container.Register<IUserMapper, UserDatabase>();
            else
                container.Register<IUserMapper, APIDatabase>();
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            pipelines.AfterRequest.AddItemToEndOfPipeline(x =>
            {
                x.Response.Headers.Add("Access-Control-Allow-Origin", WebServer.Configuration.Url);
                x.Response.Headers.Add("Access-Control-Allow-Methods", "POST,GET,DELETE,PUT,OPTIONS");
                x.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
                x.Response.Headers.Add("Access-Control-Allow-Headers", "Accept,Origin,Content-type");
                x.Response.Headers.Add("Access-Control-Expose-Headers", "Accept,Origin,Content-type");
            });

            if (!context.Request.Url.Path.StartsWith("/api"))
            {
                WebPipelines(pipelines, context);
                var configuration = new FormsAuthenticationConfiguration
                {
                    RedirectUrl = "~/login",
                    UserMapper = container.Resolve<IUserMapper>(),
                    //CryptographyConfiguration = cryptographyConfiguration
                };
                FormsAuthentication.Enable(pipelines, configuration);
            }
            else
            {
                //pipelines.EnableBasicAuthentication(new BasicAuthenticationConfiguration(container.Resolve<IUserValidator>(), "WebServer"));
                /*StatelessAuthentication.Enable(pipelines, new StatelessAuthenticationConfiguration(c =>
                {
                    if (c.Request.Cookies.ContainsKey("_ncfa"))
                        return APIDatabase.GetAuthenticatedIdentity(c.Request.Cookies["_ncfa"]);
                    else
                        return null;
                }));*/
            }
        }

        void WebPipelines(IPipelines pipelines, NancyContext context)
        {
            pipelines.BeforeRequest.AddItemToEndOfPipeline(x =>
            {
                if (x.Request.Cookies.ContainsKey("UID") && x.Request.Session["UID"] == null)
                {
                    var uid = UserDatabase.GetSession(x.Request.Cookies["UID"]);

                    if (uid != null)
                        x.Request.Session["UID"] = uid;
                    else
                        x.Request.Cookies.Remove("UID");
                }
                return null;
            });
        }
    }
}