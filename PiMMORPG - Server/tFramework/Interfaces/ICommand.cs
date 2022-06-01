using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tFramework.Interfaces
{
    public interface ICommand
    {
        string Name { get; }
        string Description { get; }

        bool Parse(object caller, params string[] args);
        bool Execute();
    }
}