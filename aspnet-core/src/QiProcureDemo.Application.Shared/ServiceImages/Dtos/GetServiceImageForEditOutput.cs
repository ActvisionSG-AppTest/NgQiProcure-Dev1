using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.ServiceImages.Dtos
{
    public class GetServiceImageForEditOutput
    {
		public CreateOrEditServiceImageDto ServiceImage { get; set; }

		public string ServiceName { get; set;}


    }
}