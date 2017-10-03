using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EntityLibrary
{
    public class Event
    {
        [JsonProperty(TypeNameHandling = TypeNameHandling.Objects)]
        public IEntity Entity { get; set; }

        public string EntityName { get; set; }
        public EventType EventType { get; set; }
    }

    public class Transaction
    {
        public List<Event> Events { get; set; }
    }

    public interface IEntity
    {
        string Id { get; set; }
    }

    public enum EventType
    {
        Inserted,
        Updated,
        Deleted
    }

    public class EventConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var evnt = new Event();
            var jo = JObject.Load(reader);
            var entityType = (string)jo["EntityType"];
            evnt.EventType = (EventType)Enum.Parse(typeof(EventType), jo["EventType"].ToString());
            var entity = jo["Entity"].ToObject(Type.GetType(entityType), serializer);
            evnt.Entity = (IEntity)entity;
            return evnt;
        }
    }
}