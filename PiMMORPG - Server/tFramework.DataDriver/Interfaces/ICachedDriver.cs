using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tFramework.DataDriver.Interfaces
{
    using tFramework.Interfaces;
    using Data.Interfaces;

    public interface ICachedDriver : IComponent
    {
        Type ModelType { get; }

        IModel InternalGetModel(Predicate<IModel> condition = null);
        IModel[] InternalGetModels(Predicate<IModel> condition = null);
        bool InternalHasModel(out IModel model, Predicate<IModel> condition = null);
        bool InternalHasModel(Predicate<IModel> condition = null);
        int InternalCount(Predicate<IModel> condition = null);

        void InternalAddModel(IModel model);
        void InternalUpdateModel(IModel model);
        void InternalRemoveModel(IModel model);
    }
}
