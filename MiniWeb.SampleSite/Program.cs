using System;
using System.Threading.Tasks;
using Autofac;
using MiniWeb.Authentication.Jwt;
using MiniWeb.Core;
using MiniWeb.SampleSite.Authentication;
using MiniWeb.SampleSite.Core;
using MiniWeb.SampleSite.DataSource;
using MiniWeb.SampleSite.Endpoints;
using MiniWeb.SampleSite.Models;
using MiniWeb.Server;

namespace MiniWeb.SampleSite
{
    internal class Program
    {
        private static async Task Main()
        {
            var containerBuilder = new ContainerBuilder();
            ConfigureServices(containerBuilder);

            var server = new WebServerBuilder(containerBuilder)
                .WithBaseUrl("http://localhost:8080/mysite/")
                .AddEndpoint<AddModelEndpoint>(HttpMethod.Post, "foos")
                .AddEndpoint<GetModelsEndpoint>(HttpMethod.Get, "foos")
                .AddEndpoint<SignInEndpoint>(HttpMethod.Post, "signin")
                .AddJwtAuthentication<SignInModel, AuthenticationService>(settings =>
                {
                    settings.Expires = TimeSpan.FromDays(1);
                    settings.Secret = "zaq1@WSXcde3$RFVzaq1@WSXcde3$RFV";
                })
                .ConfigureSerilog()
                .Build();

            AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
            {
                server.Dispose();
            };

            await server.OpenAsync();
        }

        private static void ConfigureServices(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<InMemoryDataSource>()
                .As<IDataSource>()
                .SingleInstance();
        }
    }
}
