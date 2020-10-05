using Xamarin.Forms.Internals;

namespace QiProcureDemo.Behaviors
{
    [Preserve(AllMembers = true)]
    public interface IAction
    {
        bool Execute(object sender, object parameter);
    }
}