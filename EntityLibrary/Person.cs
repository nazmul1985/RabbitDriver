using System;

namespace EntityLibrary
{
    public class Person : IEntity
    {
        public Person()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        public Person(string name, int i = 0) : this()
        {
            this.Name = name;
            this.Age = i % 2 == 0 ? 25 : 30;
            this.Gender = i % 2 == 0 ? Gender.Female : Gender.Male;
        }

        public string Name { get; set; }
        public string Id { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
    }

    public enum Gender
    {
        Male,
        Female

    }
}
