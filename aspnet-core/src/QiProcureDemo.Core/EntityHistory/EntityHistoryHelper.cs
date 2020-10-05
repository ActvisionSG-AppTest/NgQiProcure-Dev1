using QiProcureDemo.ProductPrices;
using System;
using System.Linq;
using Abp.Organizations;
using QiProcureDemo.Authorization.Roles;
using QiProcureDemo.MultiTenancy;

namespace QiProcureDemo.EntityHistory
{
    public static class EntityHistoryHelper
    {
        public const string EntityHistoryConfigurationName = "EntityHistory";

        public static readonly Type[] HostSideTrackedTypes =
        {
            typeof(ProductPrice),
            typeof(OrganizationUnit), typeof(Role), typeof(Tenant)
        };

        public static readonly Type[] TenantSideTrackedTypes =
        {
            typeof(ProductPrice),
            typeof(OrganizationUnit), typeof(Role)
        };

        public static readonly Type[] TrackedTypes =
            HostSideTrackedTypes
                .Concat(TenantSideTrackedTypes)
                .GroupBy(type => type.FullName)
                .Select(types => types.First())
                .ToArray();
    }
}
