
using System;
using Abp.Application.Services.Dto;

namespace QiProcureDemo.ServiceImages.Dtos
{
    public class ServiceImageDto : EntityDto
    {
		public string Description { get; set; }

		public string Url { get; set; }

		public bool IsMain { get; set; }

		public bool IsApproved { get; set; }

		public byte[] Bytes { get; set; }


		 public int? ServiceId { get; set; }

		public string ImageBase64String { get; set; }

	}
}