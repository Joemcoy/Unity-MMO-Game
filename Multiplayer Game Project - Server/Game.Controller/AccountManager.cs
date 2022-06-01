using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Base.Factories;
using Game.Data.Models;
using Base.Data.Enums;
using Game.Data.Enums;
using Game.Controller;
using Base.Data.Interfaces;

namespace Game.Controller
{
    public class AccountManager
    {
        public static AccountModel Login(string Username, string Password, uint Server)
        {
            IBaseController Base = ControllerFactory.GetBaseController("accounts");
            return Base.GetModels<AccountModel>(A => A.Username == Username && A.Password == Password && A.Server == Server).FirstOrDefault();
        }

        public static void IncrementLoginCount(int ID)
        {
            IBaseController Base = ControllerFactory.GetBaseController("accounts");

            AccountModel Account = Base.GetModels<AccountModel>(A => A.ID == ID).FirstOrDefault();
            if (Account != null)
            {
                Account.LoginCount++;
                Base.UpdateModel(Account);
            }
        }

        public static AccountModel GetAccountByID(int ID)
        {
            IBaseController Base = ControllerFactory.GetBaseController("accounts");
            return Base.GetModel<AccountModel>(A => A.ID == ID);
        }

        public static AccountModel GetAccount(Predicate<AccountModel> Condition)
        {
            IBaseController Base = ControllerFactory.GetBaseController("accounts");
            return Base.GetModel<AccountModel>(A => Condition(A));
        }

        public static bool CheckAccount(string Username, string Nickname, string Email, uint Server)
        {
            IBaseController Base = ControllerFactory.GetBaseController("accounts");
            return Base.GetModels<AccountModel>(A => A.Server == Server && (A.Username == Username || A.Nickname == Nickname || A.Email == Email)).Any();
        }

        public static void Register(string Username, string Password, string Nickname, string Email, uint Server)
        {
            IBaseController Base = ControllerFactory.GetBaseController("accounts");

            AccountModel Account = new AccountModel();
            Account.Username = Username;
            Account.Password = Password;
            Account.Nickname = Nickname;
            Account.Email = Email;
            Account.Server = Server;

            Base.AddModel(Account);
        }

        public static void UpdateAccount(AccountModel Account)
        {
            IBaseController Base = ControllerFactory.GetBaseController("accounts");
            Base.UpdateModel(Account);
        }
    }
}