using Sumo.Retry.Policies;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Sumo.Retry
{
    public static class WithRetry
    {
        private static RetryPolicy _defaultRetryPolicy = null;
        public static void SetDefaultPolicy(RetryPolicy retryPolicy)
        {
            _defaultRetryPolicy = retryPolicy;
        }

        #region void InvokeAsync(Action action)
        public static Task InvokeAsync(Action action)
        {
            return InvokeAsync(_defaultRetryPolicy, action);
        }

        public static async Task InvokeAsync(RetryPolicy retryPolicy, Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            if (retryPolicy == null)
            {
                throw new ArgumentNullException(nameof(retryPolicy));
            }

            var session = new RetrySession(retryPolicy);
            session.Begin();

            Exception exception = null;

            var complete = false;
            while (!complete)
            {
                try
                {
                    action();
                    complete = true;
                }
                //todo: test if the three catch methods are required to correctly resolve the exception.
                catch (TargetInvocationException ex)
                {
                    exception = ex.GetPrimaryException();
                }
                catch (AggregateException ex)
                {
                    exception = ex.GetPrimaryException();
                }
                catch (Exception ex)
                {
                    exception = ex.GetPrimaryException();
                }

                // test the exception for can retry, exceeded max retry, and timeout
                if (exception != null)
                {
                    var retryException = session.CheckException(exception);
                    if (retryException != null)
                    {
                        throw retryException;
                    }
                    // wait a bit before trying again
                    await session.SleepAsync();
                    //reset exception for next attempt
                    exception = null;
                }
            } // while (!complete)
        }
        #endregion

        #region T InvokeAsync<T>(Func<T> function)
        public static Task<T> InvokeAsync<T>(Func<T> function)
        {
            return InvokeAsync(_defaultRetryPolicy, function);
        }

        public static async Task<T> InvokeAsync<T>(RetryPolicy retryPolicy, Func<T> function)
        {
            if (function == null)
            {
                throw new ArgumentNullException(nameof(function));
            }
            if (retryPolicy == null)
            {
                throw new ArgumentNullException(nameof(retryPolicy));
            }

            var result = default(T);

            var session = new RetrySession(retryPolicy);
            session.Begin();

            Exception exception = null;

            var complete = false;
            while (!complete)
            {
                try
                {
                    result = function();
                    complete = true;
                }
                //todo: test if the three catch methods are required to correctly resolve the exception.
                catch (TargetInvocationException ex)
                {
                    exception = ex.GetPrimaryException();
                }
                catch (AggregateException ex)
                {
                    exception = ex.GetPrimaryException();
                }
                catch (Exception ex)
                {
                    exception = ex.GetPrimaryException();
                }

                // test the exception for can retry, exceeded max retry, and timeout
                if (exception != null)
                {
                    var retryException = session.CheckException(exception);
                    if (retryException != null)
                    {
                        throw retryException;
                    }                    // wait a bit before trying again
                    await session.SleepAsync();
                    //reset exception for next attempt
                    exception = null;
                }
            } // while (!complete)
            return result;
        }
        #endregion

        #region Task<T> InvokeAsync<T>(Func<Task<T>> function)
        public static Task<T> InvokeAsync<T>(Func<Task<T>> function)
        {
            return InvokeAsync(_defaultRetryPolicy, function);
        }

        public static async Task<T> InvokeAsync<T>(RetryPolicy retryPolicy, Func<Task<T>> function)
        {
            if (function == null)
            {
                throw new ArgumentNullException(nameof(function));
            }
            if (retryPolicy == null)
            {
                throw new ArgumentNullException(nameof(retryPolicy));
            }

            var result = default(T);

            var session = new RetrySession(retryPolicy);
            session.Begin();

            Exception exception = null;

            var complete = false;
            while (!complete)
            {
                try
                {
                    result = await function();
                    complete = true;
                }
                //todo: test if the three catch methods are required to correctly resolve the exception.
                catch (TargetInvocationException ex)
                {
                    exception = ex.GetPrimaryException();
                }
                catch (AggregateException ex)
                {
                    exception = ex.GetPrimaryException();
                }
                catch (Exception ex)
                {
                    exception = ex.GetPrimaryException();
                }

                // test the exception for can retry, exceeded max retry, and timeout
                if (exception != null)
                {
                    var retryException = session.CheckException(exception);
                    if (retryException != null)
                    {
                        throw retryException;
                    }
                    // wait a bit before trying again
                    await session.SleepAsync();
                    //reset exception for next attempt
                    exception = null;
                }
            } // while (!complete)
            return result;
        }
        #endregion
    }
}

