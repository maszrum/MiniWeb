using MiniWeb.Core.Abstractions;

namespace MiniWeb.Server
{
    public static class DefaultEndpointBuilderExtension
    {
        public static WebServerBuilder AddDefaultEndpoint<TEndpoint>(this WebServerBuilder builder)
            where TEndpoint : IWebEndpoint
        {
            builder.RegisterBuildCallback(server =>
            {
                server.DefaultEndpoint = RegisteredEndpoint.CreateDefault<TEndpoint>();
            });
            
            return builder;
        }
    }
}
