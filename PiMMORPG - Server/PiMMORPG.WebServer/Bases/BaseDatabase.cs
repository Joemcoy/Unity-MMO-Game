using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Security;

using PiMMORPG.Models;
using PiMMORPG.Server.General.Drivers;

using tFramework.Data.Serializer;
using tFramework.Interfaces;
using tFramework.Factories;

namespace PiMMORPG.WebServer.Bases
{
    public abstract class BaseDatabase<TDatabase, TDriver, TModel, TIdentity> : IUserMapper
        where TDatabase : BaseDatabase<TDatabase, TDriver, TModel, TIdentity>, new()
        where TModel : SerialModelBase, new()
        where TDriver : BaseDriver<TModel>, new()
        where TIdentity : IUserIdentity
    {
        static Dictionary<string, string> sessions = new Dictionary<string, string>();
        static TDatabase Instance = new TDatabase();

        public virtual string SessionFileName { get { return "sessions.cfg"; } }

        public virtual IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            using (var ctx = new TDriver())
            {
                var model = ctx.GetModel(ctx.CreateBuilder().Where(m => m.Serial).Equal(identifier));
                if (model != null)
                    return (TIdentity)Activator.CreateInstance(typeof(TIdentity), model);
            }
            return null;
        }

        public static bool LoadSessions()
        {
            sessions = new Dictionary<string, string>();
            var fpath = Path.Combine(Environment.CurrentDirectory, "Configuration", Instance.SessionFileName);
            if (File.Exists(fpath))
            {
                lock (sessions)
                {
                    using (var stream = File.OpenRead(fpath))
                        if (XMLSerializer.Load(ref sessions, stream)) return true;

                    File.Delete(fpath);
                }
            }
            return SaveSessions();
        }

        public static bool SaveSessions()
        {
            var fpath = Path.Combine(Environment.CurrentDirectory, "Configuration", Instance.SessionFileName);
            if (File.Exists(fpath)) File.Delete(fpath);

            using (var stream = File.Create(fpath))
                return XMLSerializer.Save(sessions, stream);
        }

        public static void RegisterSession(string cookie, string uid)
        {
            lock (sessions)
            {
                if (!sessions.ContainsKey(cookie))
                {
                    if (sessions.ContainsValue(uid))
                        sessions.Remove(sessions.First(k => k.Value.Equals(uid)).Key);

                    sessions.Add(cookie, uid);
                    SaveSessions();
                }
            }
        }

        public static string GetSession(string cookie)
        {
            lock (sessions)
            {
                string session = null;
                return sessions.TryGetValue(cookie, out session) ? session : null;
            }
        }

        public static void RemoveSession(string cookie)
        {
            lock (sessions)
            {
                if (sessions.ContainsKey(cookie))
                {
                    sessions.Remove(cookie);
                    SaveSessions();
                }
            }
        }
    }
}