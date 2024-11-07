using Microsoft.Extensions.Configuration;

namespace AssistantsWrapper.Helpers
{
    public static class ConfigurationHelper
    {
        public static IConfiguration LoadConfiguration()
        {
            return new ConfigurationBuilder().AddUserSecrets<Program>().Build();
        }
    }
}