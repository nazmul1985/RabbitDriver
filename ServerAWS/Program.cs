using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using EntityLibrary;
using RabbitMessageHandler;

namespace ServerAWS
{
    internal class Program
    {
        private const string RoutingKey = "#";

        private static void Main(string[] args)
        {
            //SortString();
            //return;

            //TestTransaction();
            //return;
            Console.WriteLine("AWS server");
            var receiver = new MessageReceiver();
            var queueName = "ServerAWSQueue";
            receiver.CreateReceiver(queueName, ProcessResponse, ExchangeType.Topic, ExchangeNames.TopicExchange, RoutingKey);
            receiver.CreateReceiver(queueName, ProcessResponse, ExchangeType.Fanout, ExchangeNames.FanoutExchange);

            var i = 0;
            while (true)
            {
                Console.WriteLine("Enter local zone (khulna/barisal/..):");
                var localOfficeName = Console.ReadLine();
                Console.WriteLine();
                Console.WriteLine("Enter Name:");
                var name = Console.ReadLine();
                var person = new Person(name, i);
                var transaction = new Transaction
                {
                    Events = new List<Event>()
                };
                transaction.Events.Add(new Event
                {
                    Entity = person,
                    EventType = EventType.Inserted
                });
                var sender = new MessageSender();
                sender.SendMessage(transaction, ExchangeType.Topic, ExchangeNames.TopicExchange, sender.GetRouteKey(localOfficeName, i));
                i++;
            }
        }

        private static void SortString()
        {
            var list = new List<string>
            {
                "S1","S2","S5","S15","S17","S18","S4A","S4B"
            };

            var sortedList = list.Select(e => new
            {
                Item = e,
                Position = Convert.ToInt32(Regex.Match(e, "\\d+").Value)
            }).OrderBy(e => e.Position).Select(e => e.Item);

            Console.WriteLine(string.Join(",", sortedList));
        }

        //private static void TestTransaction()
        //{
        //    var Events = new List<Event>();
        //    var transaction = new Transaction
        //    {
        //        Events = Events
        //    };
        //    var id = Guid.NewGuid().ToString();
        //    Events.Add(new Event
        //    {
        //        Entity = new Person
        //        {
        //            Id = id,
        //            Name = "Nazmul",
        //            Age = 30,
        //            Gender = Gender.Male
        //        },
        //        EntityType = typeof(Person).FullName,
        //        EventType = EventType.Inserted
        //    });
        //    Events.Add(new Event
        //    {
        //        Entity = new Person
        //        {
        //            Id = id,
        //            Name = "Nazmul Haque",
        //            Age = 30,
        //            Gender = Gender.Male
        //        },
        //        EntityType = typeof(Person).FullName,
        //        EventType = EventType.Updated
        //    });
        //    Events.Add(new Event
        //    {
        //        Entity = new Person
        //        {
        //            Id = id,
        //            Name = "Nazmul Haque",
        //            Age = 30,
        //            Gender = Gender.Male
        //        },
        //        EntityType = typeof(Person).FullName,
        //        EventType = EventType.Deleted
        //    });

        //    var json = JsonConvert.SerializeObject(transaction, Formatting.Indented, new JsonSerializerSettings());
        //    var obj = JsonConvert.DeserializeObject<Transaction>(json);
        //}

        private static bool ProcessResponse(Transaction response)
        {
            //Console.WriteLine($"AWS server received: {response}");
            return true;
        }
    }
}