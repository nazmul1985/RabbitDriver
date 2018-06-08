using System;
using DeviceFramework;
using EntityLibrary;
using RabbitMessageHandler;

namespace DeviceB2
{
    internal class Program
    {
        private const string DbPath = @"E:\RnD Projects\RabbitMQ\SqLiteDB\DeviceB2.db";
        private static readonly string RoutingKey = $"{RouteKey.Barisal}.{RouteKey.B2}";

        private static void Main(string[] args)
        {
            var title = "Barisal Device 2";
            Console.Title = title;
            Console.WriteLine(title);
            var receiver = new MessageReceiver();
            var queueName = "BarisalDevice2Queue";
            receiver.CreateReceiver(queueName, ProcessResponse, ExchangeType.Topic, ExchangeNames.TopicExchange, RoutingKey);
            receiver.CreateReceiver(queueName, ProcessResponse, ExchangeType.Fanout, ExchangeNames.FanoutExchange);
            
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