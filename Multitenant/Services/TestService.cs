namespace Multitenant.Services
{
    public interface ITestService
    {
        string Test();
    }

    public class FirstTenantTestService : ITestService
    {
        public string Test()
        {
            return "FirstTenantTestService";
        }
    }

    public class SecondTenantTestService : ITestService
    {
        public string Test()
        {
            return "SecondTenantTestService";
        }
    }

    public class DefaultTestService : ITestService
    {
        public string Test()
        {
            return "DefaultTestService";
        }
    }
}
