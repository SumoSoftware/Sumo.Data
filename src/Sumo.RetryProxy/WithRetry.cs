using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Sumo.Retry
{
    public static class WithRetry
    {
        private static RetryOptions _defaultRetryOptions = null;
        public static void SetDefaultOptions(RetryOptions options)
        {
            _defaultRetryOptions = options;
        }

        public static void Invoke(Action action)
        {
            Invoke(_defaultRetryOptions, null, action);
        }

        public static void Invoke(RetryOptions options, Action action)
        {
            Invoke(options, null, action);
        }

        public static void Invoke(IExceptionWhiteList exceptionWhiteList, Action action)
        {
            Invoke(_defaultRetryOptions, exceptionWhiteList, action);
        }

        public static void Invoke(RetryOptions options, IExceptionWhiteList exceptionWhiteList, Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            List<Exception> exceptions = null;

            var waitTime = 1.0; // tenths of a second
            var currentAttempt = 1;
            Exception exception = null;

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var complete = false;
            while (!complete)
            {
                try
                {
                    action();
                    complete = true;
                }
                catch (TargetInvocationException ex)
                {
                    exception = ex.InnerException;
                }
                catch (AggregateException ex)
                {
                    exception = ex.GetBaseException();
                }
                catch (Exception ex)
                {
                    throw new RetryException($"Unexpected exception type '{ex.GetType().FullName}'. See inner exception for details.", ex)
                    {
                        Attempts = currentAttempt,
                        Duration = stopwatch.Elapsed,
                        Exceptions = exceptions
                    };
                }

                // test the exception for can retry, exceeded max retry, and timeout
                if (exception != null)
                {
                    if (exceptions == null)
                    {
                        exceptions = new List<Exception>();
                    }

                    var retryException = TestException(options, exceptionWhiteList, exception, currentAttempt++, stopwatch.Elapsed, exceptions);
                    if (retryException != null)
                    {
                        throw retryException;
                    }
                }

                //reset exception for next attempt
                exception = null;

                // wait a bit before trying again
                Thread.Sleep((int)(waitTime * 100));

                // ratcheting up - allows wait times up to ~10 seconds per try
                waitTime = waitTime <= 110 ? waitTime * 1.25 : waitTime;
            }
        }

        public static T Invoke<T>(Func<T> action)
        {
            return Invoke(_defaultRetryOptions, null, action);
        }

        public static T Invoke<T>(RetryOptions options, Func<T> action)
        {
            return Invoke(options, null, action);
        }

        public static T Invoke<T>(IExceptionWhiteList exceptionWhiteList, Func<T> action)
        {
            return Invoke(_defaultRetryOptions, exceptionWhiteList, action);
        }

        public static T Invoke<T>(RetryOptions options, IExceptionWhiteList exceptionWhiteList, Func<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            T result = default(T);

            List<Exception> exceptions = null;

            var waitTime = 1.0; // tenths of a second
            var currentAttempt = 1;
            Exception exception = null;

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var complete = false;
            while (!complete)
            {
                try
                {
                    var isAwaitable = action.Method.ReturnType.GetMethod(nameof(Task.GetAwaiter)) != null;
                    result = isAwaitable ? InvokeAsynchronous<T>(action.Target, action.Method) : InvokeSynchronous<T>(action.Target, action.Method);
                    complete = true;
                }
                catch (TargetInvocationException ex)
                {
                    exception = ex.InnerException;
                }
                catch (AggregateException ex)
                {
                    exception = ex.GetBaseException();
                }
                catch (Exception ex)
                {
                    throw new RetryException($"Unexpected exception type '{ex.GetType().FullName}'. See inner exception for details.", ex)
                    {
                        Attempts = currentAttempt,
                        Duration = stopwatch.Elapsed,
                        Exceptions = exceptions
                    };
                }

                // test the exception for can retry, exceeded max retry, and timeout
                if (exception != null)
                {
                    if (exceptions == null)
                    {
                        exceptions = new List<Exception>();
                    }

                    var retryException = TestException(options, exceptionWhiteList, exception, currentAttempt++, stopwatch.Elapsed, exceptions);
                    if (retryException != null)
                    {
                        throw retryException;
                    }
                }

                //reset exception for next attempt
                exception = null;

                // wait a bit before trying again
                Thread.Sleep((int)(waitTime * 100));

                // ratcheting up - allows wait times up to ~10 seconds per try
                waitTime = waitTime <= 110 ? waitTime * 1.25 : waitTime;
            }
            return result;
        }

        private static RetryException TestException(RetryOptions options, IExceptionWhiteList exceptionWhiteList, Exception exception, int currentAttempt, TimeSpan elapsed, List<Exception> exceptions)
        {
            RetryException result = null;

            exceptions.Add(exception);

            if (exceptionWhiteList != null && exception != null && !exceptionWhiteList.CanRetry(exception))
            {
                result = new RetryNotAllowedException("Exception does not qualify for retry. See inner exception for details.", exception)
                {
                    Attempts = currentAttempt,
                    Duration = elapsed,
                    Exceptions = exceptions
                };
            }

            if (currentAttempt >= options.MaxAttempts)
            {
                result = new ExceededMaxAttemptsException($"Exceeded maximum attempts: {options.MaxAttempts}.. See inner exception for details.", exception)
                {
                    Attempts = currentAttempt,
                    Duration = elapsed,
                    Exceptions = exceptions
                };
            }

            if (elapsed >= options.Timeout)
            {
                result = new ExceededMaxWaitTimeException($"Exceeded maximum wait time: {options.Timeout} seconds.. See inner exception for details.", exception)
                {
                    Attempts = currentAttempt,
                    Duration = elapsed,
                    Exceptions = exceptions
                };
            }

            return result;
        }

        private static T InvokeSynchronous<T>(object targetInstance, MethodInfo targetMethod)
        {
            return (T)targetMethod.Invoke(targetInstance, null);
        }

        private static T InvokeAsynchronous<T>(object targetInstance, MethodInfo targetMethod)
        {
            var result = targetMethod.Invoke(targetInstance, null);
            var task = (Task)result;
            var continuation = task.ContinueWith(t =>
            {
                if (t.Status == TaskStatus.Faulted)
                {
                    throw t.Exception;
                }
            });
            continuation.Wait();
            return (T)result;
        }
    }
}
