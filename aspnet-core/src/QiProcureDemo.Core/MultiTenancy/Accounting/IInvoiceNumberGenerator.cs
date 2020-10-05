using System.Threading.Tasks;
using Abp.Dependency;

namespace QiProcureDemo.MultiTenancy.Accounting
{
    public interface IInvoiceNumberGenerator : ITransientDependency
    {
        Task<string> GetNewInvoiceNumber();
    }
}