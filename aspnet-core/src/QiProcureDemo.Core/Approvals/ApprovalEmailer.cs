using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Localization;
using Abp.Net.Mail;
using QiProcureDemo.Chat;
using QiProcureDemo.Editions;
using QiProcureDemo.Localization;
using QiProcureDemo.MultiTenancy;
using System.Net.Mail;
using System.Web;
using Abp.Runtime.Security;
using QiProcureDemo.Net.Emailing;
using QiProcureDemo.ApprovalRequests;
using QiProcureDemo.Authorization.Users;

namespace QiProcureDemo.Approvals
{
    /// <summary>
    /// Used to send email to users.
    /// </summary>
    public class ApprovalEmailer : QiProcureDemoServiceBase, IApprovalEmailer, ITransientDependency
    {
        private readonly IEmailTemplateProvider _emailTemplateProvider;
        private readonly IEmailSender _emailSender;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly ICurrentUnitOfWorkProvider _unitOfWorkProvider;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ISettingManager _settingManager;
        private readonly EditionManager _editionManager;

        // used for styling action links on email messages.
        private string _emailButtonStyle =
            "padding-left: 30px; padding-right: 30px; padding-top: 12px; padding-bottom: 12px; color: #ffffff; background-color: #00bb77; font-size: 14pt; text-decoration: none;";
        private string _emailButtonColor = "#00bb77";

        public ApprovalEmailer(
           IEmailTemplateProvider emailTemplateProvider,
           IEmailSender emailSender,
           IRepository<Tenant> tenantRepository,
           ICurrentUnitOfWorkProvider unitOfWorkProvider,
           IUnitOfWorkManager unitOfWorkManager,
           ISettingManager settingManager,
           EditionManager editionManager)
        {
            _emailTemplateProvider = emailTemplateProvider;
            _emailSender = emailSender;
            _tenantRepository = tenantRepository;
            _unitOfWorkProvider = unitOfWorkProvider;
            _unitOfWorkManager = unitOfWorkManager;
            _settingManager = settingManager;
            _editionManager = editionManager;
        }

        [UnitOfWork]
        public virtual async Task SendEmailApprovalAsync(ApprovalRequest approvalRequest, User user, string link)
        {
            /*                throw new Exception("EmailConfirmationCode should be set in order to send email activation link.");*/

            /*            link = link.Replace("{userId}", user.Id.ToString());
                        link = link.Replace("{confirmationCode}", Uri.EscapeDataString(user.EmailConfirmationCode));
            */
            /*          if (user.TenantId.HasValue)
                      {
                          link = link.Replace("{tenantId}", user.TenantId.ToString());
                      }
          */
            /*            link = EncryptQueryParameters(link);
            */
            /*            var tenancyName = GetTenancyNameOrNull(user.TenantId);
            */

            var emailTemplate = GetTitleAndSubTitle(1, L("EmailApproval_Title"), L("EmailApproval_SubTitle"));


            var mailMessage = new StringBuilder();

            mailMessage.AppendLine("<b>" + L("NameSurname") + "</b>: " + user.Name + "<br />");


            mailMessage.AppendLine("<b>" + L("UserName") + "</b>: " + user.Surname + "<br />");


            mailMessage.AppendLine("<br />");
            mailMessage.AppendLine(L("EmailApproval_ClickTheLinkBelowToApprove") + "<br /><br />");
            mailMessage.AppendLine("<a style=\"" + _emailButtonStyle + "\" bg-color=\"" + _emailButtonColor + "\" href=\"" + link + "\">" + L("ApproveReject") + "</a>");
            mailMessage.AppendLine("<br />");
            mailMessage.AppendLine("<br />");
            mailMessage.AppendLine("<br />");
            mailMessage.AppendLine("<span style=\"font-size: 9pt;\">" + L("EmailMessage_CopyTheLinkBelowToYourBrowser") + "</span><br />");
            mailMessage.AppendLine("<span style=\"font-size: 8pt;\">" + link + "</span>");

            await ReplaceBodyAndSend(user.EmailAddress, L("EmailApproval_Subject"), emailTemplate, mailMessage);
        }

        private StringBuilder GetTitleAndSubTitle(int? tenantId, string title, string subTitle)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetDefaultTemplate(tenantId));
            emailTemplate.Replace("{EMAIL_TITLE}", title);
            emailTemplate.Replace("{EMAIL_SUB_TITLE}", subTitle);

            return emailTemplate;
        }

        private async Task ReplaceBodyAndSend(string emailAddress, string subject, StringBuilder emailTemplate, StringBuilder mailMessage)
        {
            emailTemplate.Replace("{EMAIL_BODY}", mailMessage.ToString());
            await _emailSender.SendAsync(new MailMessage
            {
                To = { emailAddress },
                Subject = subject,
                Body = emailTemplate.ToString(),
                IsBodyHtml = true
            });
        }
    }
}
