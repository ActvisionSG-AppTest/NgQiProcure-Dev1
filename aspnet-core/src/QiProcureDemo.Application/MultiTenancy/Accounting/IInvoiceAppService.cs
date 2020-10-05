using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using QiProcureDemo.MultiTenancy.Accounting.Dto;

namespace QiProcureDemo.MultiTenancy.Accounting
{
    public interface IInvoiceAppService
    {
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateInvoiceDto input);
    }
}
