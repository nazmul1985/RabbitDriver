using System;
using System.Collections.Generic;
using EntityLibrary;
using RabbitMessageHandler;

namespace ServerHead3
{
    class Program
    {
        private const string RoutingKey = "#";

        private static void Main(string[] args)
        {
            Console.WriteLine("Head office 3");
            var receiver = new MessageReceiver();
            var queueName = "ServerHead3Queue";
            receiver.CreateReceiver(queueName, ProcessResponse, ExchangeType.Topic, ExchangeNames.TopicExchange, RoutingKey);
            receiver.CreateReceiver(queueName, ProcessResponse, ExchangeType.Fanout, ExchangeNames.FanoutExchange);

            var i = 0;
            while (true)
            {
                Console.WriteLine("Enter local zone (Cumilla/Syllet/...):");
                var route = Console.ReadLine();
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
                    EventType = EventType.Inserted
                });
                var sender = new MessageSender();
                sender.SendMessage(transaction, ExchangeType.Topic, ExchangeNames.TopicExchange, sender.GetRouteKey(route, i));
                i++;
            }
        }

        private static bool ProcessResponse(Transaction response)
        {
            //Console.WriteLine($"Head office 3 received: {response}");
            return true;
        }
    }
}
