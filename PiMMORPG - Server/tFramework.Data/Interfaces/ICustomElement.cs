using tFramework.Data.Bases;

namespace tFramework.Data.Interfaces
{
    public interface ICustomElement
    {
        void Serialize(SerializerElement element);
        void Deserialize(SerializerElement element);
    }
}