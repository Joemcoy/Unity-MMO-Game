//using System;
//using Nancy.ModelBinding;

//using PiMMORPG.Models;
//namespace PiMMORPG.WebServer.Modules
//{
//	using Models;
//    using Server.General.Drivers;

//    public class UsersModule : SecureModule
//	{
//		public UsersModule()
//			: base("/users")
//		{
//			Get["/"] = ListUsers;
//			Get["/register"] = GetRegisterUser;
//            Post["/register"] = PostRegisterUser;
//		}

//		dynamic ListUsers(dynamic p)
//		{
//			var model = new UsersModel();

//			using (var ctx = new AccountDriver())
//			{
//				model.Users = ctx.GetModels();
//			}

//			return View["default", model];
//		}

//		dynamic GetRegisterUser(dynamic p)
//		{
//			return View["register"];
//		}

//        dynamic PostRegisterUser(dynamic p)
//        {
//            var user = this.Bind<Account>();
//            var model = new SuccessModel();
//            var rpass = Request.Form["repeat-password"];
//            var success = false;
            
//            model.Message = AccountDriver.RegisterAcount(user, rpass, out success);
//            model.Success = success;

//            return View["register", model];
//        }
//	}
//}