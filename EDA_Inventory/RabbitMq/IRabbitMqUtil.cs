namespace EDA_Inventory.RabbitMq;

public interface IRabbitMqUtil
{
    Task PublishMessageQueue(string routingKey, string eventData);
}