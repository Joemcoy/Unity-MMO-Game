using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tFramework.Data.Interfaces
{
    public interface ISerialModel : IModel
    {
        Guid Serial { get; set; }
    }
}
