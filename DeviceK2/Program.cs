﻿using System;
using DeviceFramework;
using EntityLibrary;
using RabbitMessageHandler;

namespace DeviceK2
{
    class Program
    {
        private static readonly string RoutingKey = $"{RouteKey.Khulna}.{RouteKey.K2}";

        private const string DbPath = @"E:\RnD Projects\RabbitMQ\SqLiteDB\DeviceK2.db";

        private static void Main(string[] args)
        {
            Console.WriteLine("Khulna Device 2");
            var receiver = new MessageReceiver();
            var queueName = "KhulnaDevice2Queue";
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