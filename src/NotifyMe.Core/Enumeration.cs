namespace NotifyMe.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public abstract class Enumeration : IComparable
    {
        protected Enumeration(int id, string name) => (this.Id, this.Name) = (id, name);

        public int Id { get; private set; }

        public string Name { get; private set; }

        public static IEnumerable<T> GetAll<T>() where T : Enumeration =>
            typeof(T).GetFields(BindingFlags.Public |
                                BindingFlags.Static |
                                BindingFlags.DeclaredOnly)
                     .Select(f => f.GetValue(null))
                     .Cast<T>();

        public int CompareTo(object other) => this.Id.CompareTo(((Enumeration)other).Id);

        public override bool Equals(object obj)
        {
            if (obj is not Enumeration otherValue)
            {
                return false;
            }

            var typeMatches = this.GetType().Equals(obj.GetType());
            var valueMatches = this.Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Id, this.Name);
        }

        public override string ToString() => this.Name;
    }
}