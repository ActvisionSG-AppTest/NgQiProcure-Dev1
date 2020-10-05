using System.Threading.Tasks;
using QiProcureDemo.Security.Recaptcha;

namespace QiProcureDemo.Test.Base.Web
{
    public class FakeRecaptchaValidator : IRecaptchaValidator
    {
        public Task ValidateAsync(string captchaResponse)
        {
            return Task.CompletedTask;
        }
    }
}
