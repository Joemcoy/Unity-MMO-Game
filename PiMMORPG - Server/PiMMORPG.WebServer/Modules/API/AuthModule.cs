using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;

using Nancy;
using Nancy.Extensions;
using Nancy.Authentication.Forms;
using tFramework.Helper;

namespace PiMMORPG.WebServer.Modules.API
{
    using Models;
    using Server.General.Drivers;

    public class AuthModule : APIModule
    {
        public AuthModule() : base("auth")
        {
            //Get["/unsecure/{username}/{password}"] = DoAuth;
            Post["/"] = DoAuth;
            Post["/check"] = CheckAuth;
        }

        object DoAuth(dynamic p)
        {
            dynamic model = new ExpandoObject();
            var form = Request.Form;
            Response response = new Response();

            if (form.key.HasValue)
            {
                var key = Guid.Parse(Convert.ToString(form.key as object));
                using (var ctx = new APIAccessDriver())
                {
                    var access = ctx.GetModel(ctx.CreateBuilder().Where(m => m.Serial).Equal(key));
                    if (access != null)
                    {
                        model.Result = true;
                        model.access = access;

                        var aid = access.Serial.ToString("D");
                        Session["AID"] = aid;

                        var cookie = Guid.NewGuid().ToString("D");
                        response = response.WithCookie("AID", cookie);

                        APIDatabase.RegisterSession(cookie, aid);
                    }
                    else
                        model.Result = false;
                }
            }
            else
                model.Result = false;
            return response.WithNJson(model as object);
        }

        object CheckAuth(dynamic p)
        {
            var form = Request.Form;
            var result = false;

            if (form.key.HasValue)
                result = APIDatabase.GetSession(form.key) != null;

            return Response.AsNJson(result);
        }
    }
}