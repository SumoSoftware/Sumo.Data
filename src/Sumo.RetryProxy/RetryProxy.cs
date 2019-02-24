using Sumo.Retry.Policies;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Sumo.Retry
{
    public class RetryProxy : DispatchProxy
    {
        public static TInterface Create<TInterface>(TInterface instance, RetryPolicy retryPolicy) where TInterface : class
        {
            var result = Create<TInterface, RetryProxy>();

            var proxy = (result as RetryProxy);
            proxy._instance = instance ?? throw new ArgumentNullException(nameof(instance));
            proxy._retryPolicy = retryPolicy ?? throw new ArgumentNullException(nameof(retryPolicy));

            return result;
        }

        public static TInterface Create<TInterface, TImplementation>(RetryPolicy retryPolicy)
            where TInterface : class
            where TImplementation : class, new()
        {
            return Create(Activator.CreateInstance<TImplementation>() as TInterface, retryPolicy);
        }

        private object _instance;
        private RetryPolicy _retryPolicy;

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            object result = null;

            var session = new RetrySession(_retryPolicy)
                .Begin();

            Exception exception = null;

            var complete = false;
            while (!complete)
            {
                try
                {
                    var isAwaitable = targetMethod.ReturnType.GetMethod(nameof(Task.GetAwaiter)) != null;
                    result = isAwaitable ? InvokeAsynchronous(targetMethod, args) : InvokeSynchronous(targetMethod, args);
                    //result = targetMethod.Invoke(_instance, args);
                    complete = true;
                }
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
                    throw new RetryException(session, $"Unexpected exception type '{ex.GetType().FullName}'. See inner exception for details.", ex);
                }

                // test the exception for can retry, exceeded max retry, and timeout
                if (exception != null)
                {
                    var retryException = session.CheckException(exception);
                    if (retryException != null)
                    {
                        throw retryException;
                    }
                }

                //reset exception for next attempt
                exception = null;

                // wait a bit before trying again
                session.Sleep();
            } // while (!complete)

            session.End();

            return result;
        }

        private object InvokeSynchronous(MethodInfo targetMethod, object[] args)
        {
            return targetMethod.Invoke(_instance, args);
        }

        private object InvokeAsynchronous(MethodInfo targetMethod, object[] args)
        {
            var result = targetMethod.Invoke(_instance, args);
            var task = (Task)result;
            var continuation = task.ContinueWith(t =>
            {
                if (t.Status == TaskStatus.Faulted)
                {
                    throw t.Exception;
                }
            });
            continuation.Wait();
            return result;
        }
    }
}
