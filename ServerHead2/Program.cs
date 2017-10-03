using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLibrary;
using RabbitMessageHandler;

namespace ServerHead2
{
    class Program
    {
        private const string RoutingKey = "#";

        private static void Main(string[] args)
        {
            Console.WriteLine("Head office 2");
            var receiver = new MessageReceiver();
            var queueName = "ServerHead2Queue";
            receiver.CreateReceiver(queueName, ProcessResponse, ExchangeType.Topic, ExchangeNames.TopicExchange, RoutingKey);
            receiver.CreateReceiver(queueName, ProcessResponse, ExchangeType.Fanout, ExchangeNames.FanoutExchange);

            var i = 0;
            while (true)
            {
                Console.WriteLine("Enter local zone (Rajshai/Bogra):");
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

        private static bool ProcessResponse(Transaction transaction)
        {
            //Console.WriteLine($"Head office 2 received: {response}");
            return true;
        }
    }
}
