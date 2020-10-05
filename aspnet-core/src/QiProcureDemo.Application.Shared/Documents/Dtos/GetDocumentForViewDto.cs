namespace QiProcureDemo.Documents.Dtos
{
    public class GetDocumentForViewDto
    {
		public DocumentDto Document { get; set; }

		public string SysRefTenantId { get; set;}

		public string ProductName { get; set;}

		public string ServiceName { get; set;}


    }
}