#if !(UNITY_5)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base.Data.Interfaces
{
    public interface IBaseController
    {
        event EventHandler TableCreated;

        string TableName { get; set; }
        Type ModelType { get; set; }
        int Interval { get; }

        bool Open();
        bool Close();

        bool LoadData();
        void SaveData();
        IModel SaveModel(IModel Model);

        void AddModel(IModel Model);
        void RemoveModel(IModel Model);
        void UpdateModel(IModel Model);

        IModel[] GetModels();
        IModel GetModel(int ID);

        TModel GetModel<TModel>(Predicate<TModel> Condition) where TModel : IModel;
        TModel GetModel<TModel>(int ID) where TModel : IModel;
        TModel[] GetModels<TModel>() where TModel : IModel;
        TModel[] GetModels<TModel>(Predicate<TModel> Condition) where TModel : IModel;
        void RegisterAfterLoadModelCallback(Action<IBaseController> Callback);
        void RegisterAfterSaveCallback(Action<IBaseController> Callback);
        void RegisterAfterSaveModelCallback(Action<IBaseController, IModel> Callback);
    }
}
#endif