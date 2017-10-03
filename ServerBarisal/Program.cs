using System;
using System.Collections.Generic;
using EntityLibrary;
using RabbitMessageHandler;

namespace ServerBarisal
{
    class Program
    {
        private static readonly string RoutingKey = $"{RouteKey.Barisal}.#";

        private static void Main(string[] args)
        {
            Console.WriteLine("Barisal office");
            var receiver = new MessageReceiver();
            var queueName = "ServerBarisalQueue";
            receiver.CreateReceiver(queueName, ProcessResponse, ExchangeType.Topic, ExchangeNames.TopicExchange, RoutingKey);
            receiver.CreateReceiver(queueName, ProcessResponse, ExchangeType.Fanout, ExchangeNames.FanoutExchange);

            var i = 0;
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Enter Name:");
                var name = Console.ReadLine();
                var peson = new Person(name, i);
                var transaction = new Transaction
                {
                    Events = new List<Event>()
                };
                transaction.Events.Add(new Event
                {
                    Entity = peson,
                    EntityName = peson.GetType().Name,
                    EventType = EventType.Inserted
                });
                var sender = new MessageSender();
                sender.SendMessage(transaction, ExchangeType.Topic, ExchangeNames.TopicExchange, sender.GetRouteKey(RouteKey.Barisal, i));
                i++;
            }
        }

        private static bool ProcessResponse(object response)
        {
            //Console.WriteLine($"Barisal office received: {response}");
            return true;
        }
    }
}
