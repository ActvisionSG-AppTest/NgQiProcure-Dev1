using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.ProductImages
{
    public class UpdateProductImagesInput
    {
        [Required]
        [MaxLength(400)]
        public string FileToken { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int ProductId { get; set; }

        public string FileName { get; set; }

        public string Description { get; set; }

        public string FileType { get; set; }

        public byte[] Bytes { get; set; }
        
    }
}