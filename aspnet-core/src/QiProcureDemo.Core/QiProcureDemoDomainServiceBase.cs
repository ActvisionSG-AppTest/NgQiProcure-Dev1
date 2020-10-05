using Abp.Domain.Services;

namespace QiProcureDemo
{
    public abstract class QiProcureDemoDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected QiProcureDemoDomainServiceBase()
        {
            LocalizationSourceName = QiProcureDemoConsts.LocalizationSourceName;
        }
    }
}
