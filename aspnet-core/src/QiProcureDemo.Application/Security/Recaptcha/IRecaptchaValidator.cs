using System.Threading.Tasks;

namespace QiProcureDemo.Security.Recaptcha
{
    public interface IRecaptchaValidator
    {
        Task ValidateAsync(string captchaResponse);
    }
}