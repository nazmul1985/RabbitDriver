using System;
using System.Collections.Generic;
using System.Data;
using EntityLibrary;
using Newtonsoft.Json;
using RabbitMessageHandler;
using SqLiteDataDriver;

namespace DeviceFramework
{
    public class DataHandler
    {
        private readonly SqLiteDriver _dataDriver;
        private readonly string _routingKey;

        public DataHandler(string dbPath, string routingKey)
        {
            this._routingKey = routingKey;
            this._dataDriver = new SqLiteDriver(dbPath);
        }

        public bool CreatePerson(Person person)
        {
            var sql = $"insert into Person (Id,Name,Age,Gender) values ('{person.Id}','{person.Name}',{person.Age},{(int)person.Gender})";
            return this._dataDriver.ExecuteNonQuery(sql);
        }

        public bool QueueTransaction(Transaction transaction)
        {
            var transJson = JsonConvert.SerializeObject(transaction);
            var sql = $"insert into TransactionQueue(Id,TransactionJson) values ('{Guid.NewGuid().ToString()}','{transJson}')";
            return this._dataDriver.ExecuteNonQuery(sql);
        }

        public void PublishQueuedTransactions()
        {
            var tableName = "TransactionQueue";
            var dataTable = this._dataDriver.LoadData(tableName);
            foreach (DataRow row in dataTable.Rows)
            {
                //var person = new Person
                //{
                //    Id = row[0].ToString(),
                //    Name = row[1].ToString(),
                //    Age = Convert.ToInt32(row[2].ToString()),
                //    Gender = (Gender)int.Parse(row[3].ToString())
                //};
                //var transaction = new Transaction
                //{
                //    Events = new List<Event>()
                //};
                //transaction.Events.Add(new Event
                //{
                //    Entity = person,
                //    EventType = EventType.Inserted
                //});
                var json = row["TransactionJson"].ToString();
                var id = row["Id"].ToString();
                var transaction = JsonConvert.DeserializeObject<Transaction>(json);
                var msgSender = new MessageSender();
                msgSender.SendMessage(transaction, ExchangeType.Topic, ExchangeNames.TopicExchange, this._routingKey);
                this._dataDriver.DeleteById(id, tableName);
            }
        }

        public bool UpdateWithReceivedData(Person person)
        {
            var count = (int)this._dataDriver.ExecureScaler("select count(*) from Person where Id=" + person);
            var sql = count > 0
                ? $"update Person set Name='{person.Name}',Age={person.Age},Gender={(int)person.Gender} where Id='{person.Id}'"
                : $"insert into Person (Id,Name,Age,Gender) values ('{person.Id}','{person.Name}',{person.Age},{(int)person.Gender})";
            return this._dataDriver.ExecuteNonQuery(sql);
        }
    }
}