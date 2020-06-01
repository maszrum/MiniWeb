using System;
using System.Collections.Generic;
using System.Linq;
using MiniWeb.SampleSite.Core;
using MiniWeb.SampleSite.Models;

namespace MiniWeb.SampleSite.DataSource
{
	internal class InMemoryDataSource : IDataSource
	{
		private readonly Dictionary<int, FooModel> _models = new Dictionary<int, FooModel>();

		public IEnumerable<FooModel> GetAll()
		{
			return _models.Values;
		}

		public void Remove(int id)
		{
			_models.Remove(id);
		}

		public void Add(FooModel model)
		{
			var id = _models.Count == 0
				? 1
				: _models.Keys.Last() + 1;

			model.Id = id;

			_models.Add(id, model);
		}

		public void Modify(FooModel model)
		{
			if (!_models.ContainsKey(model.Id))
			{
				throw new InvalidOperationException(
					"model with specified id was not found");
			}

			_models[model.Id] = model;
		}
	}
}
