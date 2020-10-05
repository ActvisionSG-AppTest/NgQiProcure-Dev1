using Abp.Application.Services.Dto;

namespace QiProcureDemo.SysStatuses.Dtos
{
    public class SysStatusesysRefLookupTableDto
    {
		public int Id { get; set; }

		public string DisplayName { get; set; }

        public string RefCode { get; set; }

    }
}