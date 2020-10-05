using Abp.Application.Services.Dto;
using System;

namespace QiProcureDemo.Documents.Dtos
{
    public class GetAllDocumentsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string UrlFilter { get; set; }

		public string NameFilter { get; set; }

		public string DescriptionFilter { get; set; }


		 public string SysRefTenantIdFilter { get; set; }

		public string ProductNameFilter { get; set; }

		public string ServiceNameFilter { get; set; }

		public int ProductIdFilter { get; set; }

		public int ServiceIdFilter { get; set; }

	}
}