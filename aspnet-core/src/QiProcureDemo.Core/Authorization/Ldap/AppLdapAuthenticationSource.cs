using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using QiProcureDemo.Authorization.Users;
using QiProcureDemo.MultiTenancy;

namespace QiProcureDemo.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}