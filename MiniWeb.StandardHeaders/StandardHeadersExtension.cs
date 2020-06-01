using System;
using System.Net;
using Autofac;
using MiniWeb.Server;
using MiniWeb.Server.Common;

namespace MiniWeb.StandardHeaders
{
    public static class StandardHeadersExtension
    {
        public static WebServerBuilder AddStandardHeader(
            this WebServerBuilder builder, string header, Func<string> valueFactory)
        {
            Register(builder);

            builder.ContainerBuilder.RegisterBuildCallback(container =>
            {
                var headers = container.Resolve<IStandardHeaders>();
                headers.RegisterFactory(header, valueFactory);
            });

            return builder;
        }

        public static WebServerBuilder AddStandardHeader(
            this WebServerBuilder builder, HttpResponseHeader header, Func<string> valueFactory)
        {
            return AddStandardHeader(builder, header.ToHeaderName(), valueFactory);
        }

        public static WebServerBuilder AddStandardHeader(
            this WebServerBuilder builder, string header, string value)
        {
            Register(builder);

            builder.ContainerBuilder.RegisterBuildCallback(container =>
            {
                var headers = container.Resolve<IStandardHeaders>();
                headers.RegisterValue(header, value);
            });

            return builder;
        }

        public static WebServerBuilder AddStandardHeader(
            this WebServerBuilder builder, HttpResponseHeader header, string value)
        {
            return AddStandardHeader(builder, header.ToHeaderName(), value);
        }

        public static WebServerBuilder Named(this WebServerBuilder builder, string name)
        {
            return AddStandardHeader(builder, HttpResponseHeader.Server, name);
        }
        
        public static WebServerBuilder AddNoCacheControlHeader(this WebServerBuilder builder)
        {
            return AddStandardHeader(builder, HttpResponseHeader.CacheControl, "no-cache");
        }

        private static void Register(WebServerBuilder builder)
        {
            var interceptorRegistered = TryRegisterInterceptor(builder);

            if (interceptorRegistered)
            {
                builder.ContainerBuilder.RegisterType<StandardHeadersContainer>()
                    .As<IStandardHeaders>()
                    .SingleInstance();
            }
        }

        private static bool TryRegisterInterceptor(WebServerBuilder builder)
        {
            try
            {
                builder.AddInterceptor<StandardHeadersInterceptor>();
                return true;
            }
#pragma warning disable CA1031
            catch (ArgumentException)
            {
                return false;
            }
#pragma warning restore CA1031
        }
    }
}
