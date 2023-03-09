# MQTT playground
# Assignment:
The problem to solve is following:

- You monitor devices, which are sending data to you.
- Each device has a unique name.
- Each device produces measurements.

The challenge is:
- Compute number of messages you got or read from the devices.

# Description
The solution contains 3 projects - ClientPublisher, ClientSubscriber, and MqttBroker.
## MqttBroker
The server solution for MTTQ protocol. By default the server is listening on port 1883, it can be changed by defining `.WithDefaultEndpointPort(1234)` option.
The server is currently counting all published messages and writing results into the console. It can be changed into debug mode, where all messages, connections, etc .. are written in the console.

## ClientPublisher
MQTT client application which is creating a couple of sample devices. Each device, based on the type, contains a couple of monitors (e.g. temperature measurement, etc ..).
The main goal of this application is to simulate MQTT messages from multiple devices with multiple topics.

## ClientSubscriber
Another MQQT client, which is subscribing for all topics are starting with the `monitoring` value and writing some statistic results into the console.
The client is not deserializing payloads, that would be for further implementation.

# How to build
In order to build the solution .NET 6 DEV must be installed. The projects are using external nuget packages, make sure that all nuget packages are restored prior to building.
In order to restore nuget packages, execute the command `dotnet restore`.

# How to run
Run all application on the same computer - by default the `localhost` is used as a server address.
1. Run `MqttBroker.exe` Console will display counter measurements for received messages. In order to debug the communication run `MqttBroker.exe debug`.
2. Run `ClientPublishers.exe x`, where x is the number of devices (clients) that will be automatically created. 
3. Optionally: Run `ClientSubscriber.exe` or `ClientSubscriber.exe detail` if you want to see more information about devices and monitors.
4. Optionally: Run [MQTT explorer](http://mqtt-explorer.com/).

# What is not yet covered and what could be improved
- Payload is serialized into json, transferred, but not read.
- Only very basic unit tests are covered, some integration tests are missing due to lack of time.
- Improve with IoC and DI for example to decide whether to log a messages into the console or into a file - Serilog, Log4Net, etc ...
- Currently the ClientSubscriber is calculating received images after it starts. It could have been enhanced by asking Broker about number of received messages for fat (Broke is internally counting all messages already).
