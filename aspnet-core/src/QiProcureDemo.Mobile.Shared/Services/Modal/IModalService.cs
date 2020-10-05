using System.Threading.Tasks;
using QiProcureDemo.Views;
using Xamarin.Forms;

namespace QiProcureDemo.Services.Modal
{
    public interface IModalService
    {
        Task ShowModalAsync(Page page);

        Task ShowModalAsync<TView>(object navigationParameter) where TView : IXamarinView;

        Task<Page> CloseModalAsync();
    }
}
