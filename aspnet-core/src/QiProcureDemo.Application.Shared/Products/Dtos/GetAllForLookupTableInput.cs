﻿using Abp.Application.Services.Dto;

namespace QiProcureDemo.Products.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}