using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Sumo.Retry.Tests
{
    public class WithRetryTestClass
    {
        public Task<int> FunctionAsync(int x)
        {
            return Task.Run(() =>
            {
                if (x != 1)
                {
                    throw new Exception("boom");
                }

                return x;
            });
        }

        public int X { get; private set; }
        public void Action(int x)
        {
            X = x;
        }

        public int Function(int x)
        {
            return x;
        }

        public int Exception(int x)
        {
            throw new Exception("boom");
        }

        public Task<int> ExceptionAsync(int x)
        {
            return Task.Run(() =>
            {
                if (x != x + 1)
                {
                    throw new Exception("boom");
                }

                return x;
            });
        }

        public static T Invoke<T>(Func<T> action)
        {
            var isAwaitable = action.Method.ReturnType.GetMethod(nameof(Task.GetAwaiter)) != null;
            return (T)action.Method.Invoke(action.Target, null);
        }

        public static void Invoke(Action action)
        {
            action();
        }
    }

    [TestClass]
    public class WithRetry_Tests
    {
        [TestMethod]
        public async Task WithRetry_Invoke()
        {
            var ac = new WithRetryTestClass();
            WithRetry.SetDefaultPolicy(new RetryPolicy(3, TimeSpan.FromMinutes(15)));

            await WithRetry.InvokeAsync(() => ac.Action(1));

            var fr1 = await WithRetry.InvokeAsync(() => ac.Function(1));

            await Assert.ThrowsExceptionAsync<ExceededMaxAttemptsException>(async () => { var fr = await WithRetry.InvokeAsync(() => ac.Exception(1)); });

            WithRetry.SetDefaultPolicy(new RetryPolicy(int.MaxValue, TimeSpan.FromMilliseconds(1)));

            await Assert.ThrowsExceptionAsync<ExceededMaxWaitTimeException>(async () => { var fr = await WithRetry.InvokeAsync(() => ac.Exception(1)); });
        }

        [TestMethod]
        public async Task WithRetry_InvokeAsync()
        {
            var ac = new WithRetryTestClass();
            WithRetry.SetDefaultPolicy(new RetryPolicy(3, TimeSpan.FromSeconds(15)));

            var fr2 = await WithRetry.InvokeAsync(async () => await ac.FunctionAsync(1));

            await Assert.ThrowsExceptionAsync<ExceededMaxAttemptsException>(async () => { var fr = await WithRetry.InvokeAsync(() => ac.ExceptionAsync(1)); });

            WithRetry.SetDefaultPolicy(new RetryPolicy(100000, TimeSpan.FromMilliseconds(10)));

            await Assert.ThrowsExceptionAsync<ExceededMaxWaitTimeException>(async () => { var fr = await WithRetry.InvokeAsync(() => ac.ExceptionAsync(1)); });
        }
    }
}
