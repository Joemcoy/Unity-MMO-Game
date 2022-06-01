using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Base.Data.Attributes;
using Base.Data.Interfaces;

namespace Data.Service.ArgumentReceivers
{
    public class DataArguments : IArgumentReceiver
    {
        [Argument("SkipAuth")]
        public static bool SkipAuth { get; set; }
    }
}
