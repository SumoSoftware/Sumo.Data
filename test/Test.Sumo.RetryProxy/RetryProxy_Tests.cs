using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Sumo.Retry
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
    public class RetryProxy_Tests
    {
        [TestMethod]
        public async Task WithRetry_Tests()
        {
            var ac = new WithRetryTestClass();
            WithRetry.SetDefaultOptions(new RetryOptions(5, TimeSpan.FromMilliseconds(500)));

            WithRetry.Invoke(() => ac.Action(1));

            var fr1 = WithRetry.Invoke(() => ac.Function(1));

            var fr2 = await WithRetry.Invoke(() => ac.FunctionAsync(1));

            Assert.ThrowsException<ExceededMaxWaitTimeException>(() => { var fr = WithRetry.Invoke(() => ac.Exception(1)); });

            await Assert.ThrowsExceptionAsync<ExceededMaxWaitTimeException>(async () => { var fr = await WithRetry.Invoke(() => ac.ExceptionAsync(1)); });
        }

        #region async and task continuation experimentation
        //[TestMethod]
        public async Task TestAction()
        {
            await f();
        }

        private Task f()
        {
            var ac = new WithRetryTestClass();
            try
            {
                var x = 1;
                var y = WithRetryTestClass.Invoke(() => { return ac.FunctionAsync(x); });
                WithRetryTestClass.Invoke(async () =>
                {
                    var b = await ac.FunctionAsync(x);
                    if (b == 2)
                    {
                        Console.Write("");
                    }
                });
                return y;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Task.FromException(ex);
            }
        }

        public async Task Test()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var task = TestAsync();
            var elapsed = stopwatch.Elapsed;

            stopwatch.Restart();
            var result = await TestAsync();
            elapsed = stopwatch.Elapsed;
        }

        public Task<string> TestAsync()
        {
            var result = Task.Run(() =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                return "hello";
            });
            var continuation = result.ContinueWith(t => { });
            try
            {
                continuation.Wait();
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }
            return result;
        }

        public async Task Test2()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var task = TestAsync2();
            var elapsed = stopwatch.Elapsed;

            stopwatch.Restart();
            var result = await TestAsync2();
            elapsed = stopwatch.Elapsed;
        }

        public async Task<string> TestAsync2()
        {
            return await Task.Run(() =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                return "hello";
            });
        }
        #endregion

        #region null params test
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_InstanceNull()
        {
            var instance = new RetryProxySubject();
            var options = new RetryOptions(5, TimeSpan.FromSeconds(5));
            var canRetryTester = new CanRetryProxySubjectException();
            try
            {
                var retryProxy = RetryProxy.Create<IRetryProxySubject>(null, options, canRetryTester);
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual(nameof(instance), ex.ParamName);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_OptionsNull()
        {
            var instance = new RetryProxySubject();
            var options = new RetryOptions(5, TimeSpan.FromSeconds(5));
            var canRetryTester = new CanRetryProxySubjectException();
            try
            {
                var retryProxy = RetryProxy.Create<IRetryProxySubject>(instance, null, canRetryTester);
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual(nameof(options), ex.ParamName);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_CanRetryTesterNull()
        {
            var instance = new RetryProxySubject();
            var options = new RetryOptions(5, TimeSpan.FromSeconds(5));
            var canRetryTester = new CanRetryProxySubjectException();
            try
            {
                var retryProxy = RetryProxy.Create<IRetryProxySubject>(instance, options, null);
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual(nameof(canRetryTester), ex.ParamName);
                throw;
            }
        }
        #endregion

        #region success
        [TestMethod]
        public void InvokeSuccess()
        {
            var instance = new RetryProxySubject();
            var retryTest = new CanRetryProxySubjectException();
            var retryOptions = new RetryOptions(5, TimeSpan.FromSeconds(5));
            var retryProxy = RetryProxy.Create<IRetryProxySubject>(instance, retryOptions, retryTest);
            var result = retryProxy.Succeed();
            Assert.AreEqual("succeed", result);
        }

        [TestMethod]
        public async Task InvokeSuccessAsync()
        {
            var instance = new RetryProxySubject();
            var retryTest = new CanRetryProxySubjectException();
            var retryOptions = new RetryOptions(5, TimeSpan.FromSeconds(5));
            var retryProxy = RetryProxy.Create<IRetryProxySubject>(instance, retryOptions, retryTest);
            var result = await retryProxy.SucceedAsync();
            Assert.AreEqual("succeed", result);
        }
        #endregion

        #region invoke failure synchronous
        [TestMethod]
        [ExpectedException(typeof(RetryNotAllowedException))]
        public void InvokeFailure_RetryForbidden()
        {
            var instance = new RetryProxySubject();
            var retryTest = new CanNotRetryProxySubjectException();
            var retryOptions = new RetryOptions(5, TimeSpan.FromSeconds(5));
            var retryProxy = RetryProxy.Create<IRetryProxySubject>(instance, retryOptions, retryTest);
            try
            {
                var result = retryProxy.Fail();
            }
            catch (RetryException ex)
            {
                Assert.IsTrue(ex.InnerException is ProxySubjectException);
                Assert.AreEqual(1, ex.Attempts);
                Assert.AreEqual(ex.Attempts, ex.Exceptions.Count);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ExceededMaxAttemptsException))]
        public void InvokeFailure_MaxAttemptsExceeded()
        {
            var instance = new RetryProxySubject();
            var retryTest = new CanRetryProxySubjectException();
            var retryOptions = new RetryOptions(5, TimeSpan.FromSeconds(60));
            var retryProxy = RetryProxy.Create<IRetryProxySubject>(instance, retryOptions, retryTest);
            try
            {
                var result = retryProxy.Fail();
            }
            catch (RetryException ex)
            {
                Assert.IsTrue(ex.InnerException is ProxySubjectException);
                Assert.AreEqual(5, ex.Attempts);
                Assert.AreEqual(ex.Attempts, ex.Exceptions.Count);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ExceededMaxWaitTimeException))]
        public void InvokeFailure_TimeoutExceeded()
        {
            var instance = new RetryProxySubject();
            var retryTest = new CanRetryProxySubjectException();
            var retryOptions = new RetryOptions(int.MaxValue, TimeSpan.FromSeconds(1));
            var retryProxy = RetryProxy.Create<IRetryProxySubject>(instance, retryOptions, retryTest);
            try
            {
                var result = retryProxy.Fail();
            }
            catch (RetryException ex)
            {
                Assert.IsTrue(ex.InnerException is ProxySubjectException);
                Assert.IsTrue(ex.Duration >= TimeSpan.FromSeconds(1));
                Assert.AreEqual(ex.Attempts, ex.Exceptions.Count);
                throw;
            }
        }
        #endregion

        #region invoke failure async
        [TestMethod]
        [ExpectedException(typeof(RetryNotAllowedException))]
        public async Task InvokeFailure_RetryForbiddenAsync()
        {
            var instance = new RetryProxySubject();
            var retryTest = new CanNotRetryProxySubjectException();
            var retryOptions = new RetryOptions(5, TimeSpan.FromSeconds(5));
            var retryProxy = RetryProxy.Create<IRetryProxySubject>(instance, retryOptions, retryTest);
            //Assert.ThrowsException<RetryNotAllowedException>()
            try
            {
                var result = await retryProxy.FailAsync();
            }
            catch (RetryException ex)
            {
                Assert.IsTrue(ex.InnerException is ProxySubjectException);
                Assert.AreEqual(1, ex.Attempts);
                Assert.AreEqual(ex.Attempts, ex.Exceptions.Count);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ExceededMaxAttemptsException))]
        public async Task InvokeFailure_MaxAttemptsExceededAsync()
        {
            var instance = new RetryProxySubject();
            var retryTest = new CanRetryProxySubjectException();
            var retryOptions = new RetryOptions(5, TimeSpan.FromSeconds(60));
            var retryProxy = RetryProxy.Create<IRetryProxySubject>(instance, retryOptions, retryTest);
            try
            {
                var result = await retryProxy.FailAsync();
            }
            catch (RetryException ex)
            {
                Assert.IsTrue(ex.InnerException is ProxySubjectException);
                Assert.AreEqual(5, ex.Attempts);
                Assert.AreEqual(ex.Attempts, ex.Exceptions.Count);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ExceededMaxWaitTimeException))]
        public async Task InvokeFailure_TimeoutExceededAsync()
        {
            var instance = new RetryProxySubject();
            var retryTest = new CanRetryProxySubjectException();
            var retryOptions = new RetryOptions(10000, TimeSpan.FromSeconds(1));
            var retryProxy = RetryProxy.Create<IRetryProxySubject>(instance, retryOptions, retryTest);
            try
            {
                var result = await retryProxy.FailAsync();
            }
            catch (RetryException ex)
            {
                Assert.IsTrue(ex.InnerException is ProxySubjectException);
                Assert.IsTrue(ex.Duration >= TimeSpan.FromSeconds(1));
                Assert.AreEqual(ex.Attempts, ex.Exceptions.Count);
                throw;
            }
        }
        #endregion
    }
}
