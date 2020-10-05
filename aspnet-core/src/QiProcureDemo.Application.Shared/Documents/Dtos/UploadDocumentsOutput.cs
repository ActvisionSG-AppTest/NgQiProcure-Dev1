using Abp.Web.Models;

namespace QiProcureDemo.Documents.Dto
{
    public class UploadDocumentsOutput : ErrorInfo
    {
        public string Name { get; set; }

        public int SysRefId{ get; set; }

        public string FileToken { get; set; }

        public int ProductId { get; set; }

        public int ServiceId { get; set; }

        public string FileExt { get; set; }

        public UploadDocumentsOutput()
        {
            
        }

        public UploadDocumentsOutput(ErrorInfo error)
        {
            Code = error.Code;
            Details = error.Details;
            Message = error.Message;
            ValidationErrors = error.ValidationErrors;
        }
    }
}