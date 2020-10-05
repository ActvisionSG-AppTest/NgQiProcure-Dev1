
using System;
using Abp.Application.Services.Dto;

namespace QiProcureDemo.ProductImages.Dtos
{
    public class ProductImageDto : EntityDto
    {
		public string Description { get; set; }

		public string Url { get; set; }

		public bool IsMain { get; set; }

		public bool IsApproved { get; set; }


		public int? ProductId { get; set; }

		public byte[] Bytes { get; set; }

		public string FileName { get; set; }

		public string FileType { get; set; }

		public string ImageBase64String { get; set; }
	}
}