using ClientPublishers.Interfaces;
using MQTTnet;
using MQTTnet.Client;

namespace ClientPublishers.Clients;

internal abstract class ClientBase : IClient, IDisposable
{
	#region Fields

	private readonly MqttClientOptions _mqttClientOptions;
	private readonly IMqttClient _mqttClient;

	#endregion

	#region Properties

	public string ClientId { get; }

	#endregion

	#region Constructor

	protected ClientBase(string clientId, string serverAddress = "localhost")
	{
		ClientId = clientId;

		var mqttFactory = new MqttFactory();
		_mqttClient = mqttFactory.CreateMqttClient();
		_mqttClientOptions = new MqttClientOptionsBuilder()
			.WithTcpServer(serverAddress)
			.WithClientId(clientId)
			.Build();
	}

	#endregion


	#region Public methods

	public async Task ConnectAsync()
	{
		await _mqttClient.ConnectAsync(_mqttClientOptions, CancellationToken.None);
	}

	public async Task DisconnectAsync()
	{
		await _mqttClient.DisconnectAsync();
	}

	public async Task PublishMessageAsync(string topic, string payload)
	{
		var applicationMessage = new MqttApplicationMessageBuilder()
			.WithTopic(topic)
			.WithPayload(payload)
			.Build();

		await _mqttClient.PublishAsync(applicationMessage, CancellationToken.None);
	}

	public void PerformReadingAsync()
	{
		
	}

	#endregion

	#region Dispose

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			Task.Run(async () => await DisconnectAsync()).Wait();
			_mqttClient.Dispose();
		}
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	#endregion


}