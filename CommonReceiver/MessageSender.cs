using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using EntityLibrary;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace RabbitMessageHandler
{
    public class MessageSender
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

        public string GetRouteKey(string areaName, int index)
        {
            var zones = new Dictionary<string, List<string>>
            {
                {
                    RouteKey.Khulna, new List<string>
                    {
                        RouteKey.K1,
                        RouteKey.K2
                    }
                },
                {
                    RouteKey.Barisal, new List<string>
                    {
                        RouteKey.B1,
                        RouteKey.B2
                    }
                }
            };
            if (!zones.ContainsKey(areaName))
            {
                return areaName;
            }
            var users = zones[areaName];
            var user = users[index % 2];
            var routeKey = $"{areaName}.{user}";
            return routeKey;
        }

        public bool SendMessage<T>(T message, string exchangeType, string exchangeName, string routingKey = null) where T : Transaction
        {
            var channel = this.GetChannel();
            channel.ExchangeDeclare(exchange: exchangeName,
                type: exchangeType);

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);
            var basicProperties = channel.CreateBasicProperties();
            var headers = new Dictionary<string, object>
            {
                {"DataType", typeof(T).FullName}
            };
            basicProperties.Headers = headers;
            basicProperties.Persistent = true;
            routingKey = string.IsNullOrWhiteSpace(routingKey) ? null : $"Route.{routingKey}";
            channel.BasicPublish(exchange: exchangeName,
                routingKey: routingKey,
                basicProperties: basicProperties,
                body: body);
            channel.Close();
            channel.Dispose();
            Debug.WriteLine(" [x] Sent '{0}':'{1}'", routingKey, message);
            return true;
        }
    }
}