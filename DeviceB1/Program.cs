using System;
using DeviceFramework;
using EntityLibrary;
using RabbitMessageHandler;

namespace DeviceB1
{
    class Program
    {
        private static readonly string RoutingKey = $"{RouteKey.Barisal}.{RouteKey.B1}";
        private const string DbPath = @"E:\RnD Projects\RabbitMQ\SqLiteDB\DeviceB1.db";

        private static void Main(string[] args)
        {
            var title = "Barisal Device 1";
            Console.Title = title;
            Console.WriteLine(title);
            var receiver = new MessageReceiver();
            var topicQueue = "BarisalDevice1Queue";
            receiver.CreateReceiver(topicQueue, ProcessResponse, ExchangeType.Topic, ExchangeNames.TopicExchange, RoutingKey);
            receiver.CreateReceiver(topicQueue, ProcessResponse, ExchangeType.Fanout, ExchangeNames.FanoutExchange);

            //var i = 0;
            //while (true)
            //{
            //    Console.WriteLine();
            //    Console.WriteLine("Enter Name:");
            //    var name = Console.ReadLine();
            //    var peson = new Person(name, i);
            //    var sender = new MessageSender();
            //    sender.SendMessage(peson, RoutingKey, i);
            //    i++;
            //}
        }

        private static bool ProcessResponse(object person)
        {
            var dataHandler = new DataHandler(DbPath, RoutingKey);
            return dataHandler.UpdateWithReceivedData(person as Person);
        }
    }
}
