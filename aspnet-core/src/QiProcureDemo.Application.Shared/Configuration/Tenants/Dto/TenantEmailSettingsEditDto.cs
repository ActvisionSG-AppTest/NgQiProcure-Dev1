using Abp.Auditing;
using QiProcureDemo.Configuration.Dto;

namespace QiProcureDemo.Configuration.Tenants.Dto
{
    public class TenantEmailSettingsEditDto : EmailSettingsEditDto
    {
        public bool UseHostDefaultEmailSettings { get; set; }
    }
}