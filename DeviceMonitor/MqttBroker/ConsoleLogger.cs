using MQTTnet.Diagnostics;

namespace MqttBroker
{
	internal class ConsoleLogger : IMqttNetLogger
	{
		readonly object _consoleSyncRoot = new();

		public bool IsEnabled => true;

		public void Publish(MqttNetLogLevel logLevel, string source, string message, object[]? parameters, Exception? exception)
		{
			var foregroundColor = logLevel switch
			{
				MqttNetLogLevel.Verbose => ConsoleColor.White,
				MqttNetLogLevel.Info => ConsoleColor.Green,
				MqttNetLogLevel.Warning => ConsoleColor.DarkYellow,
				MqttNetLogLevel.Error => ConsoleColor.Red,
				_ => ConsoleColor.White
			};

			if (parameters?.Length > 0)
			{
				message = string.Format(message, parameters);
			}

			lock (_consoleSyncRoot)
			{
				Console.ForegroundColor = foregroundColor;
				Console.WriteLine(message);

				if (exception != null)
				{
					Console.WriteLine(exception);
				}
			}
		}
	}
}
