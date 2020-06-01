using System.Net;

namespace MiniWeb.Server.Responses
{
	public sealed class OkResponse : BaseResponse
	{
		public override HttpStatusCode StatusCode { get; set; }

		public OkResponse()
		{
			StatusCode = HttpStatusCode.OK;
		}

		protected override void Rewrite(HttpListenerResponse response)
		{
		}
	}
}