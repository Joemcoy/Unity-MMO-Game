using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy.Security;

using PiMMORPG.Models;
namespace PiMMORPG.WebServer
{
    public class AuthenticatedUser : IUserIdentity
    {
        public Account User { get; private set; }
        public string UserName => User.Nickname;
        public IEnumerable<string> Claims
        {
            get
            {
                yield return "Administrator";
            }
        }

        public AuthenticatedUser(Account User)
        {
            this.User = User;
        }
    }
}
