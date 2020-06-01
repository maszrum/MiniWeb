using MiniWeb.Logging.Serilog;
using MiniWeb.Server;
using Serilog;

namespace MiniWeb.SampleSite
{
	internal static class LoggingExtension
	{
		public static WebServerBuilder ConfigureSerilog(this WebServerBuilder builder)
		{
			var logger = new LoggerConfiguration()
				.MinimumLevel.Debug()
				.WriteTo.Console()
				.CreateLogger();

			return builder.AddSerilog(logger);
		}
	}
}