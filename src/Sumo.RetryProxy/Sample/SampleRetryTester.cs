using System;
using System.Collections.Generic;

namespace Sumo.Retry.Sample
{
    internal static class ErrorCodesCanRetryMap
    {
        static ErrorCodesCanRetryMap()
        {
            var errorCodeType = typeof(ErrorCodes);
            var retryAllowedAttributeType = typeof(SampleRetryAllowedAttribute);

            foreach(var memberInfo in errorCodeType.GetMembers())
            {
                _canRetryMap[(ErrorCodes)Enum.Parse(errorCodeType, memberInfo.Name)] = 
                    memberInfo.GetCustomAttributes(retryAllowedAttributeType, false).Length != 0;
            }
        }

        private static readonly Dictionary<ErrorCodes, bool> _canRetryMap = new Dictionary<ErrorCodes, bool>();

        internal static bool CanRetry(ErrorCodes errorCode)
        {
            return _canRetryMap[errorCode];
        }
    }

    internal class SampleCustomRetryPolicy : CustomRetryPolicy
    {
        public SampleCustomRetryPolicy(int maxAttempts, TimeSpan timeout, TimeSpan? initialInterval = null) : base(maxAttempts, timeout, initialInterval)
        {
        }

        public override bool IsRetryAllowed(Exception exception)
        {
            return exception is SampleException && ErrorCodesCanRetryMap.CanRetry(((SampleException)exception).ErrorCode);
        }
    }
}
