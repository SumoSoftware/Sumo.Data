using System;

namespace Sumo.Csv
{
    public class CsvException : Exception
    {
        public CsvException() : base() { }

        public CsvException(string message) : base(message) { }

        public CsvException(string message, Exception innerException) : base(message, innerException) { }
    }
}
