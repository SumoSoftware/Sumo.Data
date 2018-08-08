using Microsoft.Data.Sqlite;
using Sumo.Retry;
using System;
using System.Collections.Generic;

namespace Sumo.Data.Sqlite
{
    public class SqliteTransientErrorTester : ICanRetryTester
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

        // https://sqlite.org/rescode.html
        private static HashSet<int> _transientErrors = new HashSet<int>(new int[] { 5, 6, 7, 9, 17, 261, 262, 264,266,283, 513, 517, 3082});

        public bool CanRetry(Exception exception)
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));
            if (!(exception is SqliteException)) throw new ArgumentException($"Type of {nameof(exception)} must be {nameof(SqliteException)}.");

            return _transientErrors.Contains(((SqliteException)exception).ErrorCode) || _transientErrors.Contains(((SqliteException)exception).SqliteExtendedErrorCode);
        }
    }
}
