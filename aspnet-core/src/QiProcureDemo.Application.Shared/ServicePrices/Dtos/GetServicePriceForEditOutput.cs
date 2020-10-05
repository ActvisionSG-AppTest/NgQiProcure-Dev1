using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.ServicePrices.Dtos
{
    public class GetServicePriceForEditOutput
    {
		public CreateOrEditServicePriceDto ServicePrice { get; set; }

		public string ServiceName { get; set;}


    }
}