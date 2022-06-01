using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy.Security;

using PiMMORPG.Models;
namespace PiMMORPG.WebServer
{
    public class AuthenticatedAPI: IUserIdentity
    {
        public APIAccess Access { get; private set; }
        public string UserName => Access.Serial.ToString("D");
        public IEnumerable<string> Claims
        {
            get
            {
                yield return "Administrator";
            }
        }

        public AuthenticatedAPI(APIAccess access)
        {
            Access = access;
        }
    }
}
