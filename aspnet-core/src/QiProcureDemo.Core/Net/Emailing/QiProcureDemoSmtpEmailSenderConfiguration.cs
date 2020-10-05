using Abp.Configuration;
using Abp.Net.Mail;
using Abp.Net.Mail.Smtp;
using Abp.Runtime.Security;

namespace QiProcureDemo.Net.Emailing
{
    public class QiProcureDemoSmtpEmailSenderConfiguration : SmtpEmailSenderConfiguration
    {
        public QiProcureDemoSmtpEmailSenderConfiguration(ISettingManager settingManager) : base(settingManager)
        {

        }

        public override string Password => SimpleStringCipher.Instance.Decrypt(GetNotEmptySettingValue(EmailSettingNames.Smtp.Password));
    }
}