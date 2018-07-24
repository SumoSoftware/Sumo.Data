using System;
using System.Collections.Generic;

namespace Sumo.Data.Schema
{
    public class Entity
    {
        public Entity() { }
        public Entity(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            Name = name;
        }

        private string _name = null;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value && IsNameValid(value))
                {
                    _name = value;
                }
            }
        }

        public List<string> Comments { get; set; } = null;
        public void AddComment(string comment)
        {
            if (string.IsNullOrEmpty(comment)) throw new ArgumentNullException(nameof(comment));
            if (Comments == null) Comments = new List<string>();

            Comments.Add(comment);
        }

        protected virtual bool IsNameValid(string name)
        {
            return !name.Contains(" ");
        }

        public static implicit operator string(Entity name)
        {
            return name != null ? name.Name : string.Empty;
        }

        public static implicit operator Entity(string name)
        {
            return string.IsNullOrEmpty(name) ? null : new Entity(name);
        }

        public override string ToString()
        {
            return $"[{Name}]";
        }
    }
}
