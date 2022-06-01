using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace tFramework.Data.Contracts
{
    using Interfaces;
    using tFramework.Data.Bases;

    public class IPEndPointContract : IContract
    {
        public Type AssociatedType => typeof(IPEndPoint);

        public object Deserialize(SerializerElement element)
        {
            var address = element.Attributes.First(a => a.Name == "address").Value;
            var port = element.Attributes.First(a => a.Name == "port").Value;

            return new IPEndPoint(IPAddress.Parse(address), int.Parse(port));
        }

        public void Serialize(SerializerElement element, object value)
        {
            var ep = value as IPEndPoint;
            element.Attributes.Add(new SerializerAttribute("address", ep.Address.ToString()));
            element.Attributes.Add(new SerializerAttribute("port", Convert.ToString(ep.Port)));
        }
    }
}
