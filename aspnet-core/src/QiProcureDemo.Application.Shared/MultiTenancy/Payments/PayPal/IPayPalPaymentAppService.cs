using System.Threading.Tasks;
using Abp.Application.Services;
using QiProcureDemo.MultiTenancy.Payments.PayPal.Dto;

namespace QiProcureDemo.MultiTenancy.Payments.PayPal
{
    public interface IPayPalPaymentAppService : IApplicationService
    {
        Task ConfirmPayment(long paymentId, string paypalOrderId);

        PayPalConfigurationDto GetConfiguration();
    }
}
