using ClientPublishers.Events;
using ClientPublishers.Factories;
using ClientPublishers.Interfaces;
using MQTTnet;
using MQTTnet.Client;

namespace ClientPublishers.Devices;

public abstract class Device : IClient, IDisposable
{
	#region Fields

	private readonly MqttClientOptions _mqttClientOptions;
	private readonly IMqttClient _mqttClient;

	#endregion

	#region Properties

	private List<IMonitor> _monitors = new();

	#endregion

	#region Constructor

	public Device(string clientId, string serverAddress = "localhost", params MonitorType[] monitors)
	{
		var mqttFactory = new MqttFactory();
		_mqttClient = mqttFactory.CreateMqttClient();
		_mqttClientOptions = new MqttClientOptionsBuilder()
			.WithTcpServer(serverAddress)
			.WithClientId(clientId)
			.Build();

		foreach (var monitorType in monitors)
		{
			var monitor = MonitorFactory.CreateMonitor(monitorType);
			if (monitor is null)
				continue;
			
			_monitors.Add(monitor);
		}
	}

	private async Task OnMeasuredProcessing(MeasureEventArgs e)
	{
		await PublishMessageAsync(e.Topic, e.Payload ?? string.Empty);
	}

	#endregion
	
	#region Public methods

	public async Task ConnectAsync()
	{
		await _mqttClient.ConnectAsync(_mqttClientOptions, CancellationToken.None);
		SubscribeToMonitorEvents();
	}

	private void SubscribeToMonitorEvents()
	{
		foreach (var monitor in _monitors)
		{
			monitor.OnMeasured += OnMeasuredProcessing;
		}
	}

	public async Task DisconnectAsync()
	{
		await _mqttClient.DisconnectAsync();
		UnsubscribeFromMonitorEvents();
	}

	private void UnsubscribeFromMonitorEvents()
	{
		foreach (var monitor in _monitors)
		{
			monitor.OnMeasured -= OnMeasuredProcessing;
		}
	}

	public async Task PublishMessageAsync(string topic, string payload)
	{
		var applicationMessage = new MqttApplicationMessageBuilder()
			.WithTopic(topic)
			.WithPayload(payload)
			.Build();

		await _mqttClient.PublishAsync(applicationMessage, CancellationToken.None);
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