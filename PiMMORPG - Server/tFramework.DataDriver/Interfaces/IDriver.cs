using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tFramework.DataDriver.Interfaces
{
	using Data.Interfaces;

	public interface IDriver : IDisposable
	{
		Type ModelType { get; }
        string ConnectionString { get; }

        IModel GetModel(int ID);
        Task<IModel> GetModelAsync(int ID);
		IModel[] GetModels(params int[] IDs);
        Task<IModel[]> GetModelsAsync(params int[] IDs);
        //string GetColumn(string mapped, bool property = true);

        void AddModel(IModel model);
        Task AddModelAsync(IModel model);
        void UpdateModel(IModel model);
        Task UpdateModelAsync(IModel model);
        void RemoveModel(IModel model);
        Task RemoveModelAsync(IModel model);
    }

	public interface IDriver<TModel> : IDriver
		where TModel : IModel, new()
	{
		QueryBuilder<TModel> CreateBuilder();
        DriverSettings<TModel> Settings { get; }

        new TModel GetModel(int ID);
        new Task<TModel> GetModelAsync(int ID);
        new TModel[] GetModels(params int[] ID);
        new Task<TModel[]> GetModelsAsync(params int[] ID);

        TModel GetModel(QueryBuilder<TModel> Builder = null);
        Task<TModel> GetModelAsync(QueryBuilder<TModel> Builder = null);
        TModel[] GetModels(QueryBuilder<TModel> Builder = null);
        Task<TModel[]> GetModelsAsync(QueryBuilder<TModel> Builder = null);
        bool HasModel(out TModel model, QueryBuilder<TModel> Builder = null);
        Task<Tuple<bool,TModel>> HasModelAsync(QueryBuilder<TModel> Builder = null);
        bool HasModel(QueryBuilder<TModel> Builder = null);
        int Count(QueryBuilder<TModel> Builder = null);
        Task<int> CountAsync(QueryBuilder<TModel> Builder = null);

        void AddModel(TModel model);
        Task AddModelAsync(TModel model);
        void UpdateModel(TModel model);
        Task UpdateModelAsync(TModel model);
        void RemoveModel(TModel model);
        Task RemoveModelAsync(TModel model);
        void RemoveModel(QueryBuilder<TModel> Builder = null);
        Task RemoveModelAsync(QueryBuilder<TModel> Builder = null);
    }
}