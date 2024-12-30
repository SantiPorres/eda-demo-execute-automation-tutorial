using System.Text;
using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;

namespace EDA_Inventory.RabbitMq;

public class RabbitMqUtil : IRabbitMqUtil
{
    public async Task PublishMessageQueue(string routingKey, string eventData)
    {
        Console.WriteLine("Publishing message queue to RabbitMq");
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest",
            Port = 5672,
        };
        var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();
        // await channel.ExchangeDeclareAsync(
        //     exchange: "topic.exchange",
        //     type: ExchangeType.Topic,
        //     durable: true
        // );
        await channel.QueueDeclareAsync(queue: routingKey, durable: true, exclusive: false, autoDelete: false, arguments: null);
        
        try
        {
            var body = Encoding.UTF8.GetBytes(eventData);
            await channel.BasicPublishAsync(exchange: "topic.exchange", routingKey: routingKey, body: body);
            Console.WriteLine("Message published to RabbitMq");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        await Task.CompletedTask;
    }
}