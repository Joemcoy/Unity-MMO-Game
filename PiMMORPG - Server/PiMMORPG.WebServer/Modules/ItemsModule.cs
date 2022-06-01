//using System;
//using Nancy.ModelBinding;

//using PiMMORPG.Models;
//namespace PiMMORPG.WebServer.Modules
//{
//    using Models;
//    using Server.Drivers;
//    using tFramework.Helper;

//    public class ItemsModule : SecureModule
//    {
//        public ItemsModule()
//            : base("/items")
//        {
//            Get["/"] = ListItems;
//            Get["/register"] = GetRegisterItem;
//            Post["/register"] = PostRegisterItem;
//        }

//        dynamic ListItems(dynamic p)
//        {
//            var model = new UsersModel();

//            using (var ctx = new ItemDriver())
//            {
//                model.Users = ctx.GetModels();
//            }

//            return View["default", model];
//        }

//        dynamic GetRegisterItem(dynamic p)
//        {
//            return View["register"];
//        }

//        dynamic PostRegisterItem(dynamic p)
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