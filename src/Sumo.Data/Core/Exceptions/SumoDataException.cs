﻿using System;

namespace Sumo.Data
{
    public class SumoDataException : Exception
    {
        public SumoDataException()
        {
        }

        public SumoDataException(string message) : base(message)
        {
        }

        public SumoDataException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
