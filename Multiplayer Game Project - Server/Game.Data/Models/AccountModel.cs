using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data.Enums;
using Game.Data.Attributes;
using Base.Data.Interfaces;
using Network.Data.Interfaces;
using Game.Data.Abstracts;

namespace Game.Data.Models
{
	public class AccountModel : APacketWrapper, IModel
	{
		public int ID { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string Nickname { get; set; }
        public string Email { get; set; }
		public int Cash { get; set; }
		public AccessLevel Access { get; set; }
		public DateTime RegisterDate { get; set; }
		public int LoginCount { get; set; }
        public int LastCharacter { get; set; }
        public uint Server { get; set; }
        public string LastIP { get; set; }
        public bool IsBanned { get; set; }

        public AccountModel()
        {
            RegisterDate = DateTime.Now;
        }
	}
}
