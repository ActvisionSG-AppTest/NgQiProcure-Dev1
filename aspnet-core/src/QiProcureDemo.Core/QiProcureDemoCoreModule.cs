﻿using System;
using Abp.AspNetZeroCore;
using Abp.AspNetZeroCore.Timing;
using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Modules;
using Abp.Net.Mail;
using Abp.Reflection.Extensions;
using Abp.Timing;
using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries.Xml;
using Abp.Localization.Sources;
using Abp.MailKit;
using Abp.Net.Mail.Smtp;
using Abp.Zero;
using Abp.Zero.Configuration;
using Abp.Zero.Ldap;
using Abp.Zero.Ldap.Configuration;
using Castle.MicroKernel.Registration;
using MailKit.Security;
using QiProcureDemo.Authorization.Delegation;
using QiProcureDemo.Authorization.Ldap;
using QiProcureDemo.Authorization.Roles;
using QiProcureDemo.Authorization.Users;
using QiProcureDemo.Chat;
using QiProcureDemo.Configuration;
using QiProcureDemo.DashboardCustomization.Definitions;
using QiProcureDemo.Debugging;
using QiProcureDemo.DynamicEntityParameters;
using QiProcureDemo.Features;
using QiProcureDemo.Friendships;
using QiProcureDemo.Friendships.Cache;
using QiProcureDemo.Localization;
using QiProcureDemo.MultiTenancy;
using QiProcureDemo.Net.Emailing;
using QiProcureDemo.Notifications;
using QiProcureDemo.WebHooks;

namespace QiProcureDemo
{
    [DependsOn(
        typeof(QiProcureDemoCoreSharedModule),
        typeof(AbpZeroCoreModule),
        typeof(AbpZeroLdapModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpAspNetZeroCoreModule),
        typeof(AbpMailKitModule))]
    public class QiProcureDemoCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            //workaround for issue: https://github.com/aspnet/EntityFrameworkCore/issues/9825
            //related github issue: https://github.com/aspnet/EntityFrameworkCore/issues/10407
            AppContext.SetSwitch("Microsoft.EntityFrameworkCore.Issue9825", true);

            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            //Declare entity types
            Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
            Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
            Configuration.Modules.Zero().EntityTypes.User = typeof(User);

            QiProcureDemoLocalizationConfigurer.Configure(Configuration.Localization);

            //Adding feature providers
            Configuration.Features.Providers.Add<AppFeatureProvider>();

            //Adding setting providers
            Configuration.Settings.Providers.Add<AppSettingProvider>();

            //Adding notification providers
            Configuration.Notifications.Providers.Add<AppNotificationProvider>();

            //Adding webhook definition providers
            Configuration.Webhooks.Providers.Add<AppWebhookDefinitionProvider>();
            Configuration.Webhooks.TimeoutDuration = TimeSpan.FromMinutes(1);
            Configuration.Webhooks.IsAutomaticSubscriptionDeactivationEnabled = false;

            //Enable this line to create a multi-tenant application.
            Configuration.MultiTenancy.IsEnabled = QiProcureDemoConsts.MultiTenancyEnabled;

            //Enable LDAP authentication 
            //Configuration.Modules.ZeroLdap().Enable(typeof(AppLdapAuthenticationSource));

            //Twilio - Enable this line to activate Twilio SMS integration
            //Configuration.ReplaceService<ISmsSender,TwilioSmsSender>();

            //Adding DynamicEntityParameters definition providers
            Configuration.DynamicEntityParameters.Providers.Add<AppDynamicEntityParameterDefinitionProvider>();

            // MailKit configuration
            Configuration.Modules.AbpMailKit().SecureSocketOption = SecureSocketOptions.Auto;
            Configuration.ReplaceService<IMailKitSmtpBuilder, QiProcureDemoMailKitSmtpBuilder>(DependencyLifeStyle.Transient);

            //Configure roles
            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

            if (DebugHelper.IsDebug)
            {
                //Disabling email sending in debug mode
                Configuration.ReplaceService<IEmailSender, NullEmailSender>(DependencyLifeStyle.Transient);
            }

            Configuration.ReplaceService(typeof(IEmailSenderConfiguration), () =>
            {
                Configuration.IocManager.IocContainer.Register(
                    Component.For<IEmailSenderConfiguration, ISmtpEmailSenderConfiguration>()
                             .ImplementedBy<QiProcureDemoSmtpEmailSenderConfiguration>()
                             .LifestyleTransient()
                );
            });

            Configuration.Caching.Configure(FriendCacheItem.CacheName, cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromMinutes(30);
            });

            IocManager.Register<DashboardConfiguration>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(QiProcureDemoCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.RegisterIfNot<IChatCommunicator, NullChatCommunicator>();
            IocManager.Register<IUserDelegationConfiguration, UserDelegationConfiguration>();

            IocManager.Resolve<ChatUserStateWatcher>().Initialize();
            IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
        }
    }
}