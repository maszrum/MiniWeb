using System.IO;
using Autofac;
using MiniWeb.Server;

namespace MiniWeb.StaticFiles
{
    public static class StaticFilesExtension
    {
        public static WebServerBuilder WithStaticFiles(this WebServerBuilder builder, string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                throw new DirectoryNotFoundException(
                    $"specified static files directory was not found: {directoryPath}");
            }

            builder.ContainerBuilder
                .RegisterInstance(directoryPath)
                .Named<string>(StaticFilesEndpoint.DirectoryPathKey);

            builder.ContainerBuilder
                .RegisterType<StaticFilesResponseFactory>()
                .AsSelf()
                .SingleInstance();

            builder.RegisterBuildCallback(server =>
            {
                server.DefaultEndpoint = RegisteredEndpoint.CreateDefault<StaticFilesEndpoint>();
            });

            return builder;
        }
    }
}
