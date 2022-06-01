using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using tFramework.DataDriver.MySQL;

namespace PiMMORPG.Server.General.Drivers
{
    using Models;
    using tFramework.Helper;

    public class AccountDriver : BaseDriver<Account>
    {
        protected override string TableName => "accounts";
        public AccountDriver()
        {
            MapDriver<AccessLevelDriver>(m => m.Access);
        }

        protected override void OnCreateTable()
        {
            using (var ctx = new AccessLevelDriver())
            {
                AddModel(new Account
                {
                    Username = "admin",
                    Password = HashHelper.CalculateMD5("admin"),
                    Nickname = "Adminsitrador",
                    Access = ctx.GetModel(ctx.CreateBuilder().Where(m => m.PanelAccess).Equal(false))
                });
            }
        }

        protected override void OnInsert(Account model)
        {
            base.OnInsert(model);

            model.IsBanned = false;
            using (var ctx = new AccessLevelDriver())
                model.Access = ctx.GetModel(ctx.CreateBuilder().Where(m => m.PanelAccess).Equal(false));
            UpdateModel(model);
        }

        public static ushort RegisterAcount(Account account, string rpass)
        {
            if (string.IsNullOrWhiteSpace(account.Username) || account.Username.Length < 5)
                return 1;// = "O nome de usuário precisa ter no mínimo 5 caractéres.";
            else if (string.IsNullOrWhiteSpace(account.Password) || account.Password.Length < 5)
                return 2;// = "A senha precisa ter no mínimo 5 caractéres.";
            else if (string.IsNullOrWhiteSpace(account.Nickname) || account.Nickname.Length < 5)
                return 3;//= "O apelido precisa ter no mínimo 5 caractéres.";
            else if (!account.Password.Equals(rpass))
                return 4;//= "As senhas não conferem";
            else
            {
                using (var ctx = new AccountDriver())
                {
                    if (ctx.HasModel(ctx.CreateBuilder().Where(u => u.Username).Equal(account.Username)))
                        return 5;//= "O nome de usuário já está sendo utilizado!";
                    if (ctx.HasModel(ctx.CreateBuilder().Where(u => u.Nickname).Equal(account.Nickname)))
                        return 6;//= "O apelido já está sendo utilizado!";
                    else
                    {
                        account.Password = HashHelper.CalculateMD5(account.Password);

                        ctx.AddModel(account);
                        return 0;//= "Registrado com sucesso!";
                    }
                }
            }
        }
    }
}