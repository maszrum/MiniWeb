using MiniWeb.Authentication.Jwt;
using MiniWeb.Core.Abstractions;
using MiniWeb.Json;
using MiniWeb.SampleSite.Core;

namespace MiniWeb.SampleSite.Endpoints
{
	[AuthenticationRequired]
	internal class GetModelsEndpoint : IWebEndpoint
	{
		private readonly IDataSource _dataSource;

		public GetModelsEndpoint(IDependencyProvider dependencyProvider)
		{
			_dataSource = dependencyProvider.Get<IDataSource>();
		}

		public IWebResponse Process(IWebRequest request)
		{
			var models = _dataSource.GetAll();

			return new JsonResponse(models);
		}
	}
}