using System;
using Autofac;
using MiniWeb.Core.Abstractions;
using MiniWeb.Server;

namespace MiniWeb.Authentication.Jwt
{
    public static class JwtAuthenticationExtension
    {
        public static WebServerBuilder AddJwtAuthentication<TData, TService>(
            this WebServerBuilder builder, string secret)
            where TService : IAuthenticationService<TData>
        {
            var settings = new JwtSettings()
            {
                Secret = secret
            };

            Register<TData, TService>(builder, settings);

            return builder;
        }

        public static WebServerBuilder AddJwtAuthentication<TData, TService>(
            this WebServerBuilder builder, Action<JwtSettings> settings)
            where TService : IAuthenticationService<TData>
        {
            var s = new JwtSettings();
            settings(s);

            Register<TData, TService>(builder, s);

            return builder;
        }

        private static void Register<TData, TService>(WebServerBuilder builder, JwtSettings settings)
        {
            var converter = new JwtConverter(settings);

            builder.ContainerBuilder
                .RegisterType<TService>()
                .As<IAuthenticationService<TData>>()
                .SingleInstance();

            builder.ContainerBuilder
                .RegisterInstance(converter)
                .AsSelf()
                .SingleInstance();

            builder.ContainerBuilder
                .RegisterType<JwtAuthenticationInterceptor<TData>>()
                .As<IInterceptor>()
                .SingleInstance();
        }
    }
}
