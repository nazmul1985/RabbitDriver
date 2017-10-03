namespace RabbitMessageHandler
{
    public class ExchangeType
    {
        public const string Topic = RabbitMQ.Client.ExchangeType.Topic;
        public const string Fanout = RabbitMQ.Client.ExchangeType.Fanout;
        public const string Direct = RabbitMQ.Client.ExchangeType.Direct;
        public const string Headers = RabbitMQ.Client.ExchangeType.Headers;
    }
}