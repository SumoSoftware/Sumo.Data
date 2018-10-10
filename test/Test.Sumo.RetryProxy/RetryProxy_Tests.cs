using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Sumo.Retry.Tests
{
    [TestClass]
    public class RetryProxy_Tests
    {
        #region async and task continuation experimentation
        //[TestMethod]
        public async Task TestAction()
        {
            await Foo();
        }

        private Task Foo()
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
        public void Create_InstanceNull()
        {
            var instance = new RetryProxySubject();
            var policy = new CanRetryProxySubjectPolicy(5, TimeSpan.FromSeconds(5));
            Assert.ThrowsException<ArgumentNullException>(() => RetryProxy.Create<IRetryProxySubject>(null, policy));
        }

        [TestMethod]
        public void Create_PolicyNull()
        {
            var instance = new RetryProxySubject();
            var policy = new CanRetryProxySubjectPolicy(5, TimeSpan.FromSeconds(5));
            Assert.ThrowsException<ArgumentNullException>(() => RetryProxy.Create<IRetryProxySubject>(instance, null));
        }
        #endregion

        #region success
        [TestMethod]
        public void InvokeSuccess()
        {
            var instance = new RetryProxySubject();
            var retryPolicy = new CanRetryProxySubjectPolicy(5, TimeSpan.FromSeconds(5));
            var retryProxy = RetryProxy.Create<IRetryProxySubject>(instance, retryPolicy);
            var result = retryProxy.Succeed();
            Assert.AreEqual("succeed", result);
        }

        [TestMethod]
        public async Task InvokeSuccessAsync()
        {
            var instance = new RetryProxySubject();
            var retryPolicy = new CanRetryProxySubjectPolicy(5, TimeSpan.FromSeconds(5));
            var retryProxy = RetryProxy.Create<IRetryProxySubject>(instance, retryPolicy);
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
            var retryPolicy = new CanNotRetryProxySubjectPolicy(5, TimeSpan.FromMinutes(15));
            var retryProxy = RetryProxy.Create<IRetryProxySubject>(instance, retryPolicy);
            try
            {
                var result = retryProxy.Fail();
            }
            catch (RetryException ex)
            {
                Assert.IsTrue(ex.InnerException is ProxySubjectTestException);
                Assert.AreEqual(1, ex.RetrySession.Attempts);
                Assert.AreEqual(ex.RetrySession.Attempts, ex.RetrySession.Exceptions.Count);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ExceededMaxAttemptsException))]
        public void InvokeFailure_MaxAttemptsExceeded()
        {
            var instance = new RetryProxySubject();
            var retryPolicy = new CanRetryProxySubjectPolicy(5, TimeSpan.FromSeconds(60));
            var retryProxy = RetryProxy.Create<IRetryProxySubject>(instance, retryPolicy);
            try
            {
                var result = retryProxy.Fail();
            }
            catch (RetryException ex)
            {
                Assert.IsTrue(ex.RetrySession.Exceptions.Count > 0);
                Assert.IsTrue(ex.RetrySession.Exceptions[0] is ProxySubjectTestException);
                Assert.AreEqual(5, ex.RetrySession.Attempts);
                Assert.AreEqual(ex.RetrySession.Attempts, ex.RetrySession.Exceptions.Count);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ExceededMaxWaitTimeException))]
        public void InvokeFailure_TimeoutExceeded()
        {
            var instance = new RetryProxySubject();
            var retryPolicy = new CanRetryProxySubjectPolicy(int.MaxValue, TimeSpan.FromSeconds(1));
            var retryProxy = RetryProxy.Create<IRetryProxySubject>(instance, retryPolicy);
            try
            {
                var result = retryProxy.Fail();
            }
            catch (RetryException ex)
            {
                Assert.IsTrue(ex.RetrySession.Exceptions.Count > 0);
                Assert.IsTrue(ex.RetrySession.Exceptions[0] is ProxySubjectTestException);
                Assert.IsTrue(ex.RetrySession.ElapsedTime >= TimeSpan.FromSeconds(1));
                Assert.AreEqual(ex.RetrySession.Attempts, ex.RetrySession.Exceptions.Count);
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
            var retryPolicy = new CanNotRetryProxySubjectPolicy(5, TimeSpan.FromSeconds(5));
            var retryProxy = RetryProxy.Create<IRetryProxySubject>(instance, retryPolicy);
            //Assert.ThrowsException<RetryNotAllowedException>()
            try
            {
                var result = await retryProxy.FailAsync();
            }
            catch (RetryException ex)
            {
                Assert.IsTrue(ex.InnerException is ProxySubjectTestException);
                Assert.AreEqual(1, ex.RetrySession.Attempts);
                Assert.AreEqual(ex.RetrySession.Attempts, ex.RetrySession.Exceptions.Count);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ExceededMaxAttemptsException))]
        public async Task InvokeFailure_MaxAttemptsExceededAsync()
        {
            var instance = new RetryProxySubject();
            var retryPolicy = new CanRetryProxySubjectPolicy(3, TimeSpan.FromMinutes(60));
            var retryProxy = RetryProxy.Create<IRetryProxySubject>(instance, retryPolicy);
            try
            {
                var result = await retryProxy.FailAsync();
            }
            catch (RetryException ex)
            {
                Assert.IsTrue(ex.RetrySession.Exceptions.Count > 0);
                Assert.IsTrue(ex.RetrySession.Exceptions[0] is ProxySubjectTestException);
                Assert.AreEqual(3, ex.RetrySession.Attempts);
                Assert.AreEqual(ex.RetrySession.Attempts, ex.RetrySession.Exceptions.Count);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ExceededMaxWaitTimeException))]
        public async Task InvokeFailure_TimeoutExceededAsync()
        {
            var instance = new RetryProxySubject();
            var retryPolicy = new CanRetryProxySubjectPolicy(10000, TimeSpan.FromSeconds(1));
            var retryProxy = RetryProxy.Create<IRetryProxySubject>(instance, retryPolicy);
            try
            {
                var result = await retryProxy.FailAsync();
            }
            catch (RetryException ex)
            {
                Assert.IsTrue(ex.RetrySession.Exceptions.Count > 0);
                Assert.IsTrue(ex.RetrySession.Exceptions[0] is ProxySubjectTestException);
                Assert.IsTrue(ex.RetrySession.ElapsedTime >= TimeSpan.FromSeconds(1));
                Assert.AreEqual(ex.RetrySession.Attempts, ex.RetrySession.Exceptions.Count);
                throw;
            }
        }
        #endregion
    }
}
