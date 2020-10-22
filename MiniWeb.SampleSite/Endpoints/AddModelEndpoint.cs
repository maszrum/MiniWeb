using MiniWeb.Authentication.Jwt;
using MiniWeb.Core.Abstractions;
using MiniWeb.Json;
using MiniWeb.SampleSite.Core;
using MiniWeb.SampleSite.Models;
using MiniWeb.Server.Responses;

namespace MiniWeb.SampleSite.Endpoints
{
    [AuthenticationRequired]
    internal class AddModelEndpoint : JsonEndpoint<FooModel>
    {
        private readonly IDataSource _dataSource;

        public AddModelEndpoint(IDependencyProvider dependencyProvider)
        {
            _dataSource = dependencyProvider.Get<IDataSource>();
        }

        public override IWebResponse Process(FooModel input, IWebRequest request)
        {
            _dataSource.Add(input);

            return new OkResponse();
        }
    }
}
