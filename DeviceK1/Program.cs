using System;
using DeviceFramework;
using EntityLibrary;
using RabbitMessageHandler;

namespace DeviceK1
{
    static class Program
    {
        private static readonly string RoutingKey = $"{RouteKey.Khulna}.{RouteKey.K1}";
        private const string DbPath = @"E:\RnD Projects\RabbitMQ\SqLiteDB\DeviceK1.db";

        private static void Main(string[] args)
        {
            Console.WriteLine("Khulna Device 1");
            var receiver = new MessageReceiver();
            var queueName = "KhulnaDevice1Queue";
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
