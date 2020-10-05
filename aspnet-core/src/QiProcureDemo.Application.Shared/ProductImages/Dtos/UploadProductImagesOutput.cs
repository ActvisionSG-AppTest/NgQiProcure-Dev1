using Abp.Web.Models;

namespace QiProcureDemo.ProductImages.Dto
{
    public class UploadProductImagesOutput : ErrorInfo
    {
        public string FileName { get; set; }

        public string FileType { get; set; }

        public string FileToken { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int ProductId { get; set; }

        public UploadProductImagesOutput()
        {
            
        }

        public UploadProductImagesOutput(ErrorInfo error)
        {
            Code = error.Code;
            Details = error.Details;
            Message = error.Message;
            ValidationErrors = error.ValidationErrors;
        }
    }
}