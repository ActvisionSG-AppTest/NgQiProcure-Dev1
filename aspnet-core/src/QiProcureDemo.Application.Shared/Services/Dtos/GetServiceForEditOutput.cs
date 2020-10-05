using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.Services.Dtos
{
    public class GetServiceForEditOutput
    {
		public CreateOrEditServiceDto Service { get; set; }

		public string CategoryName { get; set;}

		public string SysRefRefCode { get; set;}


    }
}