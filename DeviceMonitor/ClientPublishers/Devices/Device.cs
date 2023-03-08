using ClientPublishers.Events;
using ClientPublishers.Interfaces;
using MQTTnet;
using MQTTnet.Client;

namespace ClientPublishers.Devices;

/// <summary>
/// Base class for devices. The class is encapsulating MQTT client.
/// </summary>
public sealed class Device : IClient, IDisposable
{
	#region Fields

	private readonly MqttClientOptions _mqttClientOptions;
	private readonly IMqttClient _mqttClient;

	#endregion

	#region Properties
	
	private readonly IPublishMesurement _publishMesurement;
	public string ClientName { get; }

	#endregion

	#region Constructor

	public Device(string clientId, string serverAddress, IPublishMesurement publishMesurement)
	{
		ClientName = clientId;
		
		var mqttFactory = new MqttFactory();
		_mqttClient = mqttFactory.CreateMqttClient();
		_mqttClientOptions = new MqttClientOptionsBuilder()
			.WithTcpServer(serverAddress)
			.WithClientId(clientId)
			.Build();
		
		_publishMesurement = publishMesurement;
		_publishMesurement.OnMeasured += OnPublish;
	}
	#endregion
	
	#region Public methods

	/// <summary>
	/// Connect to MQTT server and inform publishing behaviour about connecting.
	/// </summary>
	public async Task ConnectAsync()
	{
		await _mqttClient.ConnectAsync(_mqttClientOptions, CancellationToken.None);
		_publishMesurement.OnConnect();
	}

	/// <summary>
	/// Disconnect from MQTT server and inform publishing behaviour about disconnectiong.
	/// </summary>
	public async Task DisconnectAsync()
	{
		await _mqttClient.DisconnectAsync();
		_publishMesurement.OnDisconnect();
	}

	/// <summary>
	/// Publish MQTT message.
	/// </summary>
	/// <param name="topic">MQTT topic identifier.</param>
	/// <param name="payload">MQTT data payload.</param>
	public async Task PublishMessageAsync(string topic, string payload)
	{
		var applicationMessage = new MqttApplicationMessageBuilder()
			.WithTopic(topic)
			.WithPayload(payload)
			.Build();

		await _mqttClient.PublishAsync(applicationMessage, CancellationToken.None);
	}
	
	/// <summary>
	/// On publish event.
	/// </summary>
	/// <param name="e">Measure evenat data.</param>
	private async Task OnPublish(MeasureEventArgs e)
	{
		await PublishMessageAsync(e.Topic, e.Payload ?? String.Empty);
	}
	
	#endregion

	#region Dispose

	private void Dispose(bool disposing)
	{
		if (!disposing) 
			return;
		
		Task.Run(async () => await DisconnectAsync()).Wait();
		_mqttClient.Dispose();
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	#endregion
}