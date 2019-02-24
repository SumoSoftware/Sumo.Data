using Sumo.Retry.Policies;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Sumo.Retry
{
    public class RetrySession
    {
        private RetrySession() { }

        internal RetrySession(RetryPolicy retryPolicy)
        {
            _retryPolicy = retryPolicy ?? throw new ArgumentNullException(nameof(retryPolicy));
            _waitTime = retryPolicy.InitialInterval;
        }

        internal RetrySession(RetrySession retrySession) : this(retrySession._retryPolicy)
        {
            Attempts = retrySession.Attempts;
            ElapsedTime = TimeSpan.FromTicks(retrySession.ElapsedTime.Ticks);
            Exceptions.AddRange(retrySession.Exceptions);
        }

        private readonly RetryPolicy _retryPolicy;
        private TimeSpan _waitTime;
        private TimeSpan _timeSpan = TimeSpan.FromTicks(0);
        private Stopwatch _stopwatch = null;

        public bool Active => _stopwatch != null;
        public int Attempts { get; set; } = 0;
        public TimeSpan ElapsedTime
        {
            get => _stopwatch == null ? _timeSpan : _stopwatch.Elapsed;
            private set => _timeSpan = value;
        }
        public List<Exception> Exceptions { get; } = new List<Exception>();

        //todo: set wait time increment type in policy. for example: ExponentialRetryPolicy from service bus
        private void AdjustWaitTime()
        {
            // ratcheting up - allows wait times up to ~10 seconds per try
            _waitTime = TimeSpan.FromMilliseconds(_waitTime.TotalMilliseconds <= 110
                ? _waitTime.TotalMilliseconds * 1.25
                : _waitTime.TotalMilliseconds);
        }

        /// <summary>
        /// uses thread.sleep
        /// </summary>
        internal void Sleep()
        {
            Thread.Sleep(_waitTime);
            AdjustWaitTime();
        }

        /// <summary>
        /// uses task.delay
        /// </summary>
        /// <returns></returns>
        internal async Task SleepAsync()
        {
            await Task.Delay(_waitTime);
            AdjustWaitTime();
        }

        internal void Begin()
        {
            if (_stopwatch == null)
            {
                _stopwatch = Stopwatch.StartNew();
            }
        }

        internal void Stop()
        {
            _stopwatch?.Stop();
        }

        private RetryException CheckAttempts(Exception exception)
        {
            return ++Attempts >= _retryPolicy.MaxAttempts
                ? new ExceededMaxAttemptsException(this, _retryPolicy, exception)
                : null;
        }

        private RetryException CheckTimeout(Exception exception)
        {
            if (!Active)
            {
                Begin();
            }

            return ElapsedTime >= _retryPolicy.Timeout
                ? new ExceededMaxWaitTimeException(this, _retryPolicy, exception)
                : null;
        }

        internal RetryException CheckException(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            Exceptions.Add(exception);

            var result = CheckAttempts(exception);
            if (result == null)
            {
                result = CheckTimeout(exception);
            }
            if (result == null && !_retryPolicy.CanRetry(exception))
            {
                result = new RetryNotAllowedException(this, _retryPolicy, exception);
            }

            return result;
        }
    }
}
