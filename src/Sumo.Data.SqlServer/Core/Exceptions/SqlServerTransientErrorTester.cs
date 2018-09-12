using Sumo.Retry;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Sumo.Data.SqlServer
{
    public class SqlServerTransientErrorTester : IExceptionWhiteList
    {
        // 20 The instance of SQL Server does Not support encryption.
        // 64 An error occurred during login. 
        // 233 Connection initialization error. 
        // 10053 A transport-level error occurred when receiving results from the server. 
        // 10054 A transport-level error occurred when sending the request to the server. 
        // 10060 Network Or instance-specific error. 
        // 40143 Connection could Not be initialized. 
        // 40197 The service encountered an error processing your request.
        // 40501 The server Is busy. 
        // 40613 The database Is currently unavailable.
        private static readonly HashSet<int> _transientErrors = new HashSet<int>(new int[] { 20, 64, 121, 233, 10053, 10054, 10060, 10060, 40143, 40197, 40501, 40613 });

        public bool CanRetry(Exception exception)
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));

            return (exception is SqlException) && _transientErrors.Contains(((SqlException)exception).Number);
        }
    }
}
