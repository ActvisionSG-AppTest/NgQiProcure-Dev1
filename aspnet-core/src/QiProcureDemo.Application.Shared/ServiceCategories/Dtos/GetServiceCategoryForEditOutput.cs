using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.ServiceCategories.Dtos
{
    public class GetServiceCategoryForEditOutput
    {
		public CreateOrEditServiceCategoryDto ServiceCategory { get; set; }

		public string ServiceName { get; set;}

		public string CategoryName { get; set;}


    }
}