using System.Globalization;

namespace QiProcureDemo.Localization
{
    public interface IApplicationCulturesProvider
    {
        CultureInfo[] GetAllCultures();
    }
}