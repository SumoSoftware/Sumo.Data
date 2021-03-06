﻿using System;
using System.Collections.Generic;

namespace Sumo.Data.Schema
{
    /// <summary>
    /// Entity is the base class for all all database entities. An entity is any database item with a name. 
    /// For example, Catalog, Schema, Table, Column, etc.
    /// </summary>
    [Serializable]
    public class EntityDefinition
    {
        public EntityDefinition() { }
        public EntityDefinition(string name)
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
            return !string.IsNullOrEmpty(name)&& !string.IsNullOrWhiteSpace(name) && !name.Contains(" ");
        }

        public static implicit operator string(EntityDefinition name)
        {
            return name != null ? name.Name : string.Empty;
        }

        public static implicit operator EntityDefinition(string name)
        {
            return string.IsNullOrEmpty(name) ? null : new EntityDefinition(name);
        }

        public override string ToString()
        {
            return $"[{Name}]";
        }
    }
}
