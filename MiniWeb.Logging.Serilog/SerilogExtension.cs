using Autofac;
using MiniWeb.Server;
using Serilog;

namespace MiniWeb.Logging.Serilog
{
    public static class SerilogExtension
    {
        public static WebServerBuilder AddSerilog(this WebServerBuilder builder, ILogger logger)
        {
            builder.ContainerBuilder
                .RegisterInstance(logger)
                .As<ILogger>();

            builder.AddInterceptor<SerilogInterceptor>();

            builder.ContainerBuilder
                .RegisterType<SerilogLogger>()
                .AsSelf();

            builder.RegisterBuildCallback(server =>
            {
                var exceptionLogger = server.DependencyProvider.Get<SerilogLogger>();
                server.ExceptionThrown += exceptionLogger.LogException;
            });

            return builder;
        }
    }
}
