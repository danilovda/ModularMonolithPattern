using System.Diagnostics;

namespace Common.RabbitMq;

public static class RabbitMqHelper
{
    public static void AddMessagingTags(Activity activity, string queueName)
    {        
        activity?.SetTag("messaging.system", "rabbitmq");
        activity?.SetTag("messaging.destination_kind", "queue");
        activity?.SetTag("messaging.rabbitmq.queue", queueName);
    }
}
