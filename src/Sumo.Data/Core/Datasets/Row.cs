//using System;
using System.Collections.Generic;
using System.Data;

namespace Sumo.Data.Datasets
{
    public class Row
    {
        public Row() : base() { }

        public Row(List<object> items) : base()
        {
            Items = items.ToArray();
        }

        public Row(object[] items) : base()
        {
            //todo: find someone to argue over whether it's better to do a deep or shallow copy from items
            //Items = (object[])items.Clone();

            //Items = new object[items.Length];
            //Array.Copy(items, Items, items.Length);

            Items = items;
        }

        public Row(DataRow row) : this(row.ItemArray)
        {
        }

        public object[] Items { get; set; }
    }
}
