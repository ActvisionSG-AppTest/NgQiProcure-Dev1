using System.Threading.Tasks;

namespace QiProcureDemo.Net.Sms
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}