using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Nancy;
using Nancy.Security;
using Nancy.Authentication.Forms;
using Nancy.Authentication.Basic;

using tFramework.Helper;
using tFramework.Data.Serializer;
using PiMMORPG.WebServer.Bases;
using PiMMORPG.Models;

namespace PiMMORPG.WebServer
{
    using Server.General.Drivers;

    public class UserDatabase : BaseDatabase<UserDatabase, AccountDriver, Account, AuthenticatedUser>, IUserValidator
    {
        static Dictionary<string, string> sessions = new Dictionary<string, string>();

        IUserIdentity IUserValidator.Validate(string username, string password)
        {
            var hashed = HashHelper.CalculateMD5(password);
            using (var ctx = new AccountDriver())
            {
                var user = ctx.GetModel(ctx.CreateBuilder().Where(u => u.Username).Equal(username).And(u => u.Password).Equal(hashed));
                return user == null ? null : new AuthenticatedUser(user);
            }
        }
    }
}