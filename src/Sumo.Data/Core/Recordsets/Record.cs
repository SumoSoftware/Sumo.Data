//using System;
using System.Collections.Generic;
using System.Data;

namespace Sumo.Data
{
    public class Record
    {
        public Record() : base() { }

        public Record(List<object> items) : base()
        {
            Items = items.ToArray();
        }

        public Record(object[] items) : base()
        {
            //todo: find someone to argue over whether it's better to do a deep or shallow copy from items

            //todo: I can't find any conclusive evidence online that one of the 3 ways is better than the other, though obviously way 1 is the fastsest - but is it stable?

            // way 1
            Items = items;

            // way 2
            //Items = (object[])items.Clone();

            // way 3
            //Items = new object[items.Length];
            //Array.Copy(items, Items, items.Length);
        }

        public Record(DataRow row) : this(row.ItemArray)
        {
        }

        public object[] Items { get; set; }

        public object this[int index] => Items[index];
    }
}
