using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.Localization.Dto
{
    public class CreateOrUpdateLanguageInput
    {
        [Required]
        public ApplicationLanguageEditDto Language { get; set; }
    }
}