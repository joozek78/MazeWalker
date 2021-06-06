using System;

namespace MazeWalker.Core.Domain
{
    public class Person
    {
        public Person(int personId, string name, string birthday)
        {
            PersonId = personId;
            Name = name;
            Birthday = birthday;
        }

        public int PersonId { get; }
        public string Name { get; }
        public string Birthday { get; }

        protected bool Equals(Person other)
        {
            return PersonId == other.PersonId && Name == other.Name && Birthday == other.Birthday;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Person) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PersonId, Name, Birthday);
        }
    }
}