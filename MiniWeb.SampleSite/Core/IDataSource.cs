using System.Collections.Generic;
using MiniWeb.SampleSite.Models;

namespace MiniWeb.SampleSite.Core
{
    internal interface IDataSource
    {
        IEnumerable<FooModel> GetAll();
        void Remove(int id);
        void Add(FooModel model);
        void Modify(FooModel model);
    }
}
