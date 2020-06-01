using MiniWeb.Core.Abstractions;
using MiniWeb.Server.Responses;

namespace MiniWeb.StaticFiles
{
	internal class StaticFilesEndpoint : IWebEndpoint
	{
		public const string DirectoryPathKey = "DirectoryPath";

		//private readonly string _directoryPath;
		//private readonly StaticFilesResponseFactory _responseFactory;

		//public StaticFilesEndpoint(IDependencyProvider dependencyProvider)
		//{
		//	_directoryPath = dependencyProvider.GetNamed<string>(DirectoryPathKey);
		//	_responseFactory = dependencyProvider.Get<StaticFilesResponseFactory>();
		//}

		public IWebResponse Process(IWebRequest request)
		{
			return new NotFoundResponse();
		}
	}
}