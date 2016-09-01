using System;

namespace DataAccessEF
{
    public class Entity
    {
        public Guid? Id { get; set; }
        public int Version { get; set; }
        public string Description { get; set; }

        public Entity() { }

        public Entity(Guid id, int version, string description)
        {
            Id = id;
            Version = version;
            Description = description;
        }
    }
}