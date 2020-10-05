using Microsoft.Extensions.Configuration;

namespace QiProcureDemo.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
