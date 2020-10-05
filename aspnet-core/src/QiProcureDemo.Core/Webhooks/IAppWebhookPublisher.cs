using System.Threading.Tasks;
using QiProcureDemo.Authorization.Users;

namespace QiProcureDemo.WebHooks
{
    public interface IAppWebhookPublisher
    {
        Task PublishTestWebhook();
    }
}
