using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Nancy;
using Nancy.ModelBinding;

namespace PiMMORPG.WebServer.Modules.API
{
    using Models;
    using Server.General.Drivers;

    public class AccountsModule : SecureAPIModule
    {
        public AccountsModule() : base("accounts")
        {
            Post["/"] = ListAccounts;
            Post["/count"] = CountAccounts;
            Post["/register"] = RegisterAccount;
        }

        object ListAccounts(dynamic p)
        {
            using (var ctx = new AccountDriver())
            {
                var models = ctx.GetModels();
                return Response.AsNJson(models);
            }
        }

        object CountAccounts(dynamic p)
        {
            using (var ctx = new AccountDriver())
            {
                var c = ctx.Count();
                return Response.AsNJson(c);
            }
        }

        object RegisterAccount(dynamic p)
        {
            var user = this.Bind<Account>();
            var rpass = Request.Form["repeat-password"];

            return Response.AsNJson(AccountDriver.RegisterAcount(user, rpass) as object);
        }
    }
}