﻿namespace ClientPublishers.Interfaces;

public interface IClient
{
    public Task ConnectAsync();
    public Task DisconnectAsync();
    public Task PublishMessageAsync(string topic, string payload);
}