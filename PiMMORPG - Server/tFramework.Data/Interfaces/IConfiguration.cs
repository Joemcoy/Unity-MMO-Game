using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace tFramework.Data.Interfaces
{
    public interface IConfiguration
    {
        bool Secure { get; }
        string Filename { get; }
    }
}