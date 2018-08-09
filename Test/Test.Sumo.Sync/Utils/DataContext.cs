using Sumo.Data.Orm;
using System;
using System.Linq;
using System.Collections.Generic;
using GSP.X.Repository.Local;

namespace Test.Sumo.Sync.Utils
{
    public class DataContext
    {

        IRepository _repo;

        public const string OUTPUT_FILENAME = @"L:\SQLITEDB1.sqlite";

        public DataContext(IOrmDataComponentFactory factory)
        {
            _repo = new Repository(factory);           
        }

        public List<Stores> GetStores()
        {
            return _repo.Read<Stores>(new Dictionary<String,object>()).ToList();
        }

    }
}
