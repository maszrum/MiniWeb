using MiniWeb.Authentication.Jwt;
using MiniWeb.Core.Abstractions;
using MiniWeb.Json;
using MiniWeb.SampleSite.Models;
using MiniWeb.Server.Responses;

namespace MiniWeb.SampleSite.Endpoints
{
	internal class SignInEndpoint : JsonEndpoint<SignInModel>
	{
		private readonly IAuthenticationService<SignInModel> _authenticationService;

		public SignInEndpoint(IDependencyProvider dependencyProvider)
		{
			_authenticationService = dependencyProvider.Get<IAuthenticationService<SignInModel>>();
		}

		public override IWebResponse Process(SignInModel input, IWebRequest request)
		{
			var success = _authenticationService.Authenticate(input, out var token);

			if (!success)
			{
				return new UnauthorizedResponse();
			}

			return new JsonResponse(new { token });
		}
	}
}