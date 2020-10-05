using Abp.Application.Services.Dto;

namespace QiProcureDemo.DynamicEntityParameters.Dto
{
    public class DynamicParameterValueDto : EntityDto
    {
        public virtual string Value { get; set; }

        public int? TenantId { get; set; }

        public int DynamicParameterId { get; set; }
    }
}
