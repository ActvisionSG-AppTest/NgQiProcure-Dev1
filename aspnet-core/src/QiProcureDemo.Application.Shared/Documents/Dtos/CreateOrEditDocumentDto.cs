
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.Documents.Dtos
{
    public class CreateOrEditDocumentDto : EntityDto<int?>
    {

		[StringLength(DocumentConsts.MaxUrlLength, MinimumLength = DocumentConsts.MinUrlLength)]
		public string Url { get; set; }
		
		
		[Required]
		[StringLength(DocumentConsts.MaxNameLength, MinimumLength = DocumentConsts.MinNameLength)]
		public string Name { get; set; }
		
		
		[StringLength(DocumentConsts.MaxDescriptionLength, MinimumLength = DocumentConsts.MinDescriptionLength)]
		public string Description { get; set; }
		
		
		 public int? SysRefId { get; set; }
		 
		 		 public int? ProductId { get; set; }
		 
		 		 public int? ServiceId { get; set; }
		 
		 
    }
}