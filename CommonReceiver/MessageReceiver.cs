using System;
using System.Collections.Generic;
using System.Text;
using EntityLibrary;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMessageHandler
{
    public class MessageReceiver
    {
        private IModel GetChannel()
        {
            var factory = new ConnectionFactory
            {
                HostName = "172.16.0.135",
                Port = 5672,
                UserName = "nazmul",
                Password = "topsecret"
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            connection.AutoClose = true;
            return channel;
        }

        public void CreateReceiver(string queueName, Func<Transaction, bool> handleMessage, string exchangeType, string exchangeName, string routeKey = null)
        {
            var channel = this.GetChannel();
            channel.ExchangeDeclare(exchange: exchangeName, type: exchangeType);
            var arguments = new Dictionary<string, object>
            {
                {"x-queue-mode", "lazy"}
            };
            channel.QueueDeclare(queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: arguments);

            var key = string.IsNullOrWhiteSpace(routeKey) ? "" : $"Route.{routeKey}";
            channel.QueueBind(queue: queueName,
                exchange: exchangeName,
                routingKey: key);

            Console.WriteLine(" [*] Waiting for messages.");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, e) =>
            {
                var body = e.Body;
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received '{0}':'{1}'",
                    e.RoutingKey, message);
                //var dataType = e.BasicProperties.Headers["DataType"];
                //var type = Type.GetType(Encoding.UTF8.GetString(dataType as byte[]));
                var obj = JsonConvert.DeserializeObject<Transaction>(message);

                if (handleMessage(obj))
                {
                    Console.WriteLine("Message handled '{0}':'{1}'",
                    e.RoutingKey, message);
                    channel.BasicAck(e.DeliveryTag, false);
                }
                else
                {
                    Console.WriteLine("Message processing failed...:(");
                }
            };
            channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
            //channel.Close();
            //channel.Dispose();

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

    }
}
