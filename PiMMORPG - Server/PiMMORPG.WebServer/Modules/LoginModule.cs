//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Nancy;
//using Nancy.Authentication.Forms;
//using Nancy.ModelBinding;
//using tFramework.Helper;

//using PiMMORPG.Models;
//namespace PiMMORPG.WebServer.Modules
//{
//    using Models;
//    using Server.General.Drivers;

//    public class LoginModule : NancyModule
//    {
//        public LoginModule() : base("/")
//        {
//            Get["login"] = _ =>
//            {
//                if (Context.CurrentUser != null)
//                    return Response.AsRedirect("/");
//                else
//                    return View["login"];
//            };
//            Post["login"] = HandleLogin;

//            Get["register"] = _ => View["register"];
//            Post["register"] = HandleRegister;

//            Get["logout"] = _ => this.Logout("/login");
//        }

//        object HandleLogin(dynamic p)
//        {
//            var login = this.Bind<Account>();
//            var model = new LoginModel();

//            if (string.IsNullOrWhiteSpace(login.Username) || login.Username.Length < 5)
//                model.Message = "O nome de usuário precisa ter no mínimo 5 caractéres!";
//            else if (string.IsNullOrWhiteSpace(login.Password) || login.Password.Length < 5)
//                model.Message = "A senha precisa ter no mínimo 5 caractéres!";
//            else
//            {
//                using (var ctx = new AccountDriver())
//                {
//                    var user = ctx.GetModel(ctx.CreateBuilder().Where(c => c.Username).Equal(login.Username));
//                    if (user == null)
//                        model.Message = "Usuário não encontrado!";
//                    else if (user.Password != HashHelper.CalculateMD5(login.Password))
//                        model.Message = "As senhas não conferem!";
//                    else
//                    {
//                        model.Message = "Logado! Por favor aguarde...";
//                        model.Success = true;
//                        model.URL = Request.Query["returnUrl"];

//                        return this.LoginAndRedirect(user.Serial);
//                    }
//                }
//            }

//            return View["login", model];
//        }

//        object HandleRegister(dynamic p)
//        {
//            var user = this.Bind<Account>();
//            var model = new SuccessModel();
//            var rpass = Request.Form["repeat-password"];
//            var success = false;

//            model.Message = AccountDriver.RegisterAcount(user, rpass, out success);
//            model.Success = success;

//            return View["register", model];
//        }
//    }
//}