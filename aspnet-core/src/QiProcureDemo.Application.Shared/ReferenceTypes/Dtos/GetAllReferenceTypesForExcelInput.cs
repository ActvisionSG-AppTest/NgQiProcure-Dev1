using Abp.Application.Services.Dto;
using System;

namespace QiProcureDemo.ReferenceTypes.Dtos
{
    public class GetAllReferenceTypesForExcelInput
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }

		public string ReferenceTypeCodeFilter { get; set; }

		public string ReferenceTypeGroupFilter { get; set; }



    }
}