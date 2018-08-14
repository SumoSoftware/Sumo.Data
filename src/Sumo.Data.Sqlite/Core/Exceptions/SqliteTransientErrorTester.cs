using Microsoft.Data.Sqlite;
using Sumo.Retry;
using System;
using System.Collections.Generic;

namespace Sumo.Data.Sqlite
{
    public class SqliteTransientErrorTester : ICanRetryTester
    {
        // https://sqlite.org/rescode.html
        private static HashSet<int> _transientErrors = new HashSet<int>(new int[] { 5, 6, 7, 9, 17, 261, 262, 264,266,283, 513, 517, 3082});

        public bool CanRetry(Exception exception)
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));

            return (exception is SqliteException) && _transientErrors.Contains(((SqliteException)exception).ErrorCode) || _transientErrors.Contains(((SqliteException)exception).SqliteExtendedErrorCode);
        }
    }
}
