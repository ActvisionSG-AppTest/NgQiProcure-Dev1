using Abp.Web.Models;

namespace QiProcureDemo.ServiceImages.Dto
{
    public class UploadServiceImagesOutput : ErrorInfo
    {
        public string FileName { get; set; }

        public string FileType { get; set; }

        public string FileToken { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int ServiceId { get; set; }

        public UploadServiceImagesOutput()
        {
            
        }

        public UploadServiceImagesOutput(ErrorInfo error)
        {
            Code = error.Code;
            Details = error.Details;
            Message = error.Message;
            ValidationErrors = error.ValidationErrors;
        }
    }
}