using System.Threading.Tasks;
using QiProcureDemo.ApprovalRequests;
using QiProcureDemo.Authorization.Users;
using QiProcureDemo.Chat;
using QiProcureDemo.Teams;

namespace QiProcureDemo.Approvals
{
    public interface IApprovalEmailer
    {
        /// <summary>
        /// Send email activation link to user's email address.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="link">Email activation link</param>
        /// <param name="plainPassword">
        /// <param name="plainPassword">
        /// Can be set to user's plain password to include it in the email.
        /// </param>
        Task SendEmailApprovalAsync(ApprovalRequest approvalRequest, User user, string link);

    }
}
