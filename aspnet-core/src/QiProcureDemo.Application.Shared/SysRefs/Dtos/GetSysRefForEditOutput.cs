using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.SysRefs.Dtos
{
    public class GetSysRefForEditOutput
    {
		public CreateOrEditSysRefDto SysRef { get; set; }

		public string ReferenceTypeName { get; set;}


    }
}