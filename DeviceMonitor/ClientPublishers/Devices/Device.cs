using ClientPublishers.Events;
using ClientPublishers.Factories;
using ClientPublishers.Interfaces;
using MQTTnet;
using MQTTnet.Client;

namespace ClientPublishers.Devices;

public interface IPublishBehaviour
{
	void OnConnect();
	void OnDisconnect();
	event PublishBehaviour.AsyncEventHandler<MeasureEventArgs>? OnMeasured;
}

public abstract class Device : IClient, IDisposable
{
	#region Fields

	private readonly MqttClientOptions _mqttClientOptions;
	private readonly IMqttClient _mqttClient;

	#endregion

	#region Properties
	
	private readonly IPublishBehaviour _publishBehaviour;

	#endregion

	#region Constructor

	protected Device(string clientId, string serverAddress, IPublishBehaviour publishBehaviour)
	{
		var mqttFactory = new MqttFactory();
		_mqttClient = mqttFactory.CreateMqttClient();
		_mqttClientOptions = new MqttClientOptionsBuilder()
			.WithTcpServer(serverAddress)
			.WithClientId(clientId)
			.Build();
		
		_publishBehaviour = publishBehaviour;
		_publishBehaviour.OnMeasured += OnPublish;
	}
	#endregion
	
	#region Public methods

	public async Task ConnectAsync()
	{
		await _mqttClient.ConnectAsync(_mqttClientOptions, CancellationToken.None);
		_publishBehaviour.OnConnect();

	}

	public async Task DisconnectAsync()
	{
		await _mqttClient.DisconnectAsync();
		_publishBehaviour.OnDisconnect();
	}

	public async Task PublishMessageAsync(string topic, string payload)
	{
		var applicationMessage = new MqttApplicationMessageBuilder()
			.WithTopic(topic)
			.WithPayload(payload)
			.Build();

		await _mqttClient.PublishAsync(applicationMessage, CancellationToken.None);
	}
	
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