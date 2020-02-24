namespace Multitenant.Services
{
    public interface ITestService
    {
        string GetName();
    }

    public class FirstTenantTestService : ITestService
    {
        public string GetName()
        {
            return "FirstTenantTestService";
        }
    }

    public class SecondTenantTestService : ITestService
    {
        public string GetName()
        {
            return "SecondTenantTestService";
        }
    }

    public class DefaultTestService : ITestService
    {
        public string GetName()
        {
            return "DefaultTestService";
        }
    }
}
