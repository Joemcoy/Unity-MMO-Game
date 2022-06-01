using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Dynamic;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Authentication.Forms;

using tFramework.Helper;

namespace PiMMORPG.WebServer.Modules.API
{
    using Models;
    using Server.General.Drivers;
    public class LoginModule : SecureAPIModule
    {
        public LoginModule() : base("/")
        {
            Post["/login"] = DoAuth;
            Post["/logout"] = Logout;
        }

        object DoAuth(dynamic p)
        {
            dynamic model = new ExpandoObject();
            var login = this.Bind<Account>();
            var response = new Response();

            if (string.IsNullOrWhiteSpace(login.Username) || login.Username.Length < 5)
                model.Result = 1;
            else if (string.IsNullOrWhiteSpace(login.Password) || login.Password.Length < 5)
                model.Result = 2;
            else
            {
                using (var ctx = new AccountDriver())
                {
                    var user = ctx.GetModel(ctx.CreateBuilder().Where(c => c.Username).Equal(login.Username));
                    if (user == null)
                        model.Result = 3;
                    else if (user.Password != HashHelper.CalculateMD5(login.Password))
                        model.Result = 4;
                    else
                    {
                        model.Result = 0;
                        model.User = user;

                        DateTime? expires = null;
                        if (Request.Form.remember.HasValue && Request.Form.remember)
                            expires = DateTime.Now.AddMonths(1);

                        var uid = user.Serial.ToString("D");
                        Session["UID"] = uid;

                        var cookie = Guid.NewGuid().ToString("D");
                        response = response.WithCookie("UID", cookie, expires);

                        UserDatabase.RegisterSession(cookie, uid);
                    }
                }
            }

            response = response.WithNJson(model as object);
            return response;
        }

        object Logout(dynamic p)
        {
            dynamic model = new ExpandoObject();
            
            if (Session.Any(c => c.Key == "UID"))
            {
                Session.Delete("UID");
                var cookie = Request.Cookies["UID"];

                UserDatabase.RemoveSession(cookie);
                Request.Cookies.Remove("UID");

                model.Result = true;
            }
            else
                model.Result = false;
            return Response.AsNJson(model as object);
        }
    }
}