using System.Threading.Tasks;

namespace Sumo.Data.Retry
{
    internal interface IRetryProxySubject
    {
        string Succeed();
        Task<string> SucceedAsync();
        string Fail();
        Task<string> FailAsync();
    }

    internal class RetryProxySubject : IRetryProxySubject
    {
        public string Succeed()
        {
            return "succeed";
        }

        public async Task<string> SucceedAsync()
        {
            return await Task.Run(() => { return "succeed"; });
        }

        public string Fail()
        {
            throw new ProxySubjectException("fail");
        }

        public async Task<string> FailAsync()
        {
            return await Task.Run(() =>
            {
                throw new ProxySubjectException("fail");
#pragma warning disable CS0162 // Unreachable code detected
                return "fail";
#pragma warning restore CS0162 // Unreachable code detected
            });

        }
    }
}
