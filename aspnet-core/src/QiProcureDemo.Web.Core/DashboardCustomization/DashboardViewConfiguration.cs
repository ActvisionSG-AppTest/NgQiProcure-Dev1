using System.Collections.Generic;


namespace QiProcureDemo.Web.DashboardCustomization
{
    public class DashboardViewConfiguration
    {
        public Dictionary<string, WidgetViewDefinition> WidgetViewDefinitions { get; } = new Dictionary<string, WidgetViewDefinition>();

        public Dictionary<string, WidgetFilterViewDefinition> WidgetFilterViewDefinitions { get; } = new Dictionary<string, WidgetFilterViewDefinition>();

        public DashboardViewConfiguration()
        {
            var jsAndCssFileRoot = "/Areas/App/Views/CustomizableDashboard/Widgets/";
            var viewFileRoot = "~/Areas/App/Views/Shared/Components/CustomizableDashboard/Widgets/";

            #region FilterViewDefinitions

            WidgetFilterViewDefinitions.Add(QiProcureDemoDashboardCustomizationConsts.Filters.FilterDateRangePicker,
                new WidgetFilterViewDefinition(
                    QiProcureDemoDashboardCustomizationConsts.Filters.FilterDateRangePicker,
                    viewFileRoot + "DateRangeFilter.cshtml",
                    jsAndCssFileRoot + "DateRangeFilter/DateRangeFilter.min.js",
                    jsAndCssFileRoot + "DateRangeFilter/DateRangeFilter.min.css")
            );
            
            //add your filters iew definitions here
            #endregion

            #region WidgetViewDefinitions

            #region TenantWidgets

            WidgetViewDefinitions.Add(QiProcureDemoDashboardCustomizationConsts.Widgets.Tenant.DailySales,
                new WidgetViewDefinition(
                    QiProcureDemoDashboardCustomizationConsts.Widgets.Tenant.DailySales,
                    viewFileRoot + "DailySales.cshtml",
                    jsAndCssFileRoot + "DailySales/DailySales.min.js",
                    jsAndCssFileRoot + "DailySales/DailySales.min.css"));

            WidgetViewDefinitions.Add(QiProcureDemoDashboardCustomizationConsts.Widgets.Tenant.GeneralStats,
                new WidgetViewDefinition(
                    QiProcureDemoDashboardCustomizationConsts.Widgets.Tenant.GeneralStats,
                    viewFileRoot + "GeneralStats.cshtml",
                    jsAndCssFileRoot + "GeneralStats/GeneralStats.min.js",
                    jsAndCssFileRoot + "GeneralStats/GeneralStats.min.css"));

            WidgetViewDefinitions.Add(QiProcureDemoDashboardCustomizationConsts.Widgets.Tenant.ProfitShare,
                new WidgetViewDefinition(
                    QiProcureDemoDashboardCustomizationConsts.Widgets.Tenant.ProfitShare,
                    viewFileRoot + "ProfitShare.cshtml",
                    jsAndCssFileRoot + "ProfitShare/ProfitShare.min.js",
                    jsAndCssFileRoot + "ProfitShare/ProfitShare.min.css"));
  
            WidgetViewDefinitions.Add(QiProcureDemoDashboardCustomizationConsts.Widgets.Tenant.MemberActivity,
                new WidgetViewDefinition(
                    QiProcureDemoDashboardCustomizationConsts.Widgets.Tenant.MemberActivity,
                    viewFileRoot + "MemberActivity.cshtml",
                    jsAndCssFileRoot + "MemberActivity/MemberActivity.min.js",
                    jsAndCssFileRoot + "MemberActivity/MemberActivity.min.css"));

            WidgetViewDefinitions.Add(QiProcureDemoDashboardCustomizationConsts.Widgets.Tenant.RegionalStats,
                new WidgetViewDefinition(
                    QiProcureDemoDashboardCustomizationConsts.Widgets.Tenant.RegionalStats,
                    viewFileRoot + "RegionalStats.cshtml",
                    jsAndCssFileRoot + "RegionalStats/RegionalStats.min.js",
                    jsAndCssFileRoot + "RegionalStats/RegionalStats.min.css",
                    12,
                    10));

            WidgetViewDefinitions.Add(QiProcureDemoDashboardCustomizationConsts.Widgets.Tenant.SalesSummary,
                new WidgetViewDefinition(
                    QiProcureDemoDashboardCustomizationConsts.Widgets.Tenant.SalesSummary,
                    viewFileRoot + "SalesSummary.cshtml",
                    jsAndCssFileRoot + "SalesSummary/SalesSummary.min.js",
                    jsAndCssFileRoot + "SalesSummary/SalesSummary.min.css",
                    6,
                    10));

            WidgetViewDefinitions.Add(QiProcureDemoDashboardCustomizationConsts.Widgets.Tenant.TopStats,
                new WidgetViewDefinition(
                    QiProcureDemoDashboardCustomizationConsts.Widgets.Tenant.TopStats,
                    viewFileRoot + "TopStats.cshtml",
                    jsAndCssFileRoot + "TopStats/TopStats.min.js",
                    jsAndCssFileRoot + "TopStats/TopStats.min.css",
                    12,
                    10));

            //add your tenant side widget definitions here
            #endregion

            #region HostWidgets

            WidgetViewDefinitions.Add(QiProcureDemoDashboardCustomizationConsts.Widgets.Host.IncomeStatistics,
                new WidgetViewDefinition(
                    QiProcureDemoDashboardCustomizationConsts.Widgets.Host.IncomeStatistics,
                    viewFileRoot + "IncomeStatistics.cshtml",
                    jsAndCssFileRoot + "IncomeStatistics/IncomeStatistics.min.js",
                    jsAndCssFileRoot + "IncomeStatistics/IncomeStatistics.min.css"));

            WidgetViewDefinitions.Add(QiProcureDemoDashboardCustomizationConsts.Widgets.Host.TopStats,
                new WidgetViewDefinition(
                    QiProcureDemoDashboardCustomizationConsts.Widgets.Host.TopStats,
                    viewFileRoot + "HostTopStats.cshtml",
                    jsAndCssFileRoot + "HostTopStats/HostTopStats.min.js",
                    jsAndCssFileRoot + "HostTopStats/HostTopStats.min.css"));

            WidgetViewDefinitions.Add(QiProcureDemoDashboardCustomizationConsts.Widgets.Host.EditionStatistics,
                new WidgetViewDefinition(
                    QiProcureDemoDashboardCustomizationConsts.Widgets.Host.EditionStatistics,
                    viewFileRoot + "EditionStatistics.cshtml",
                    jsAndCssFileRoot + "EditionStatistics/EditionStatistics.min.js",
                    jsAndCssFileRoot + "EditionStatistics/EditionStatistics.min.css"));

            WidgetViewDefinitions.Add(QiProcureDemoDashboardCustomizationConsts.Widgets.Host.SubscriptionExpiringTenants,
                new WidgetViewDefinition(
                    QiProcureDemoDashboardCustomizationConsts.Widgets.Host.SubscriptionExpiringTenants,
                    viewFileRoot + "SubscriptionExpiringTenants.cshtml",
                    jsAndCssFileRoot + "SubscriptionExpiringTenants/SubscriptionExpiringTenants.min.js",
                    jsAndCssFileRoot + "SubscriptionExpiringTenants/SubscriptionExpiringTenants.min.css",
                    6,
                    10));

            WidgetViewDefinitions.Add(QiProcureDemoDashboardCustomizationConsts.Widgets.Host.RecentTenants,
                new WidgetViewDefinition(
                    QiProcureDemoDashboardCustomizationConsts.Widgets.Host.RecentTenants,
                    viewFileRoot + "RecentTenants.cshtml",
                    jsAndCssFileRoot + "RecentTenants/RecentTenants.min.js",
                    jsAndCssFileRoot + "RecentTenants/RecentTenants.min.css"));

            //add your host side widgets definitions here
            #endregion

            #endregion
        }
    }
}
