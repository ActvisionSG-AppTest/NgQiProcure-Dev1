
using System;
using Abp.Application.Services.Dto;

namespace QiProcureDemo.Documents.Dtos
{
    public class DocumentDto : EntityDto
    {
		public string Url { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }


		 public int? SysRefId { get; set; }

		 		 public int? ProductId { get; set; }

		 		 public int? ServiceId { get; set; }
		public string ImageBase64String { get; set; }


	}
}