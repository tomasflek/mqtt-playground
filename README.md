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
MQQT client application which is creating couple of sample devices. Each device, based on the type, contains couple of monitors (e.g. temperature measurement, etc ..).

## ClientSubscriber
Another MQQT client, which is monitoring for `monitoring` topics and writing results into the console.

# Build
In order to buid the solution .NET 6 DEV must be installed. The projects are using external nuget packages, make sure that all nuget packages are restored prior building.

# How to runt it
1. Make sure that all projects are successfully built.
2. Run `MqttBroker.exe`
3. Run `ClientPublishers.exe`
4. Optionaly: run `ClientSubscriber.exe`
