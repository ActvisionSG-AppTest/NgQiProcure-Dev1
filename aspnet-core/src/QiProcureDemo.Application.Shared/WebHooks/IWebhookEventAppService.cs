using System.Threading.Tasks;
using Abp.Webhooks;

namespace QiProcureDemo.WebHooks
{
    public interface IWebhookEventAppService
    {
        Task<WebhookEvent> Get(string id);
    }
}
