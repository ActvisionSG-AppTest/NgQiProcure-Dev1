using System.Threading.Tasks;
using QiProcureDemo.Sessions.Dto;

namespace QiProcureDemo.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
