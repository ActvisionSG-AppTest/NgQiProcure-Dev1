using System.Reflection;
using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace QiProcureDemo.Localization
{
    public static class QiProcureDemoLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    QiProcureDemoConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(QiProcureDemoLocalizationConfigurer).GetAssembly(),
                        "QiProcureDemo.Localization.QiProcureDemo"
                    )
                )
            );
        }
    }
}