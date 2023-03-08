# MQTT playground
# Assignment:
The problem to solve is following:

- You monitor devices, which are sending data to you.
- Each device has a unique name.
- Each device produces measurements.

 

The challenge is:
- Compute number of messages you got or read from the devices.

# Description
The solution contians 3 projects - ClientPublisher, ClientSubscriber and MqttBroker.
## MqqtBroker
The server solution for MTTQ protocol. By default the server is listening on port 1883, it can be change by defining `.WithDefaultEndpointPort(1234)` option.
The server is currently counting all published messages and writing results into the console.

## ClientPublisher
MQTT client application which is creating couple of sample devices. Each device, based on the type, contains couple of monitors (e.g. temperature measurement, etc ..).
The main goal for this application is to simulate MQTT messages from multiple devices with multiple topics.

## ClientSubscriber
Another MQQT client, which is subscribing for all topics with are starting with `monitoring` value and writing some statiscit results into the console.
The client is not desirializing payloads, that would be for further implementation.

# How to build
In order to buid the solution .NET 6 DEV must be installed. The projects are using external nuget packages, make sure that all nuget packages are restored prior building.
In order to restore nuget packages, execute command `dotnet restore`.

# How to run it
1. Run `MqttBroker.exe` Console will display counter measurement for received messages. In order to debug the communication run `MqttBroker.exe debug`.
2. Run `ClientPublishers.exe x`, where x is the number of devices (clients) which will be automatically created. 
3. Optionaly: Run `ClientSubscriber.exe` or `ClientSubscriber.exe detail` if you want to see more information about devices and monitors.

# What is not covered yet
- payload is serialized into json, transfered, but not read
- only very basic unit tests are covered, some integration tests are missing due to lack of time
