using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.Documents
{
    public class UpdateDocumentsInput
    {
        [Required]
        [MaxLength(400)]
        public string FileToken { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public int ProductId { get; set; }

        public int ServiceId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string FileExt { get; set; }
        public int SysRefId { get; set; }

        public byte[] Bytes { get; set; }
        
    }
}