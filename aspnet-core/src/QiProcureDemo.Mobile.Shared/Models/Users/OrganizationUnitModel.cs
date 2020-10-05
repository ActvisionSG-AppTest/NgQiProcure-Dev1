using Abp.AutoMapper;
using QiProcureDemo.Organizations.Dto;

namespace QiProcureDemo.Models.Users
{
    [AutoMapFrom(typeof(OrganizationUnitDto))]
    public class OrganizationUnitModel : OrganizationUnitDto
    {
        public bool IsAssigned { get; set; }
    }
}