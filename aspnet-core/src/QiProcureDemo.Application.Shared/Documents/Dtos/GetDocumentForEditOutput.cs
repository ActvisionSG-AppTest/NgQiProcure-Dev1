using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.Documents.Dtos
{
    public class GetDocumentForEditOutput
    {
		public CreateOrEditDocumentDto Document { get; set; }

		public string SysRefTenantId { get; set;}

		public string ProductName { get; set;}

		public string ServiceName { get; set;}


    }
}