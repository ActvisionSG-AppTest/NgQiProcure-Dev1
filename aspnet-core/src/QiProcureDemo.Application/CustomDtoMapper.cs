using QiProcureDemo.ApprovalRequests.Dtos;
using QiProcureDemo.ApprovalRequests;
using QiProcureDemo.ProjectInstructions.Dtos;
using QiProcureDemo.ProjectInstructions;
using QiProcureDemo.Emails.Dtos;
using QiProcureDemo.Emails;
using QiProcureDemo.Approvals.Dtos;
using QiProcureDemo.Approvals;
using QiProcureDemo.Projects.Dtos;
using QiProcureDemo.Projects;
using QiProcureDemo.Accounts.Dtos;
using QiProcureDemo.Accounts;
using QiProcureDemo.TeamMembers.Dtos;
using QiProcureDemo.TeamMembers;
using QiProcureDemo.Teams.Dtos;
using QiProcureDemo.Teams;
using QiProcureDemo.SysStatuses.Dtos;
using QiProcureDemo.SysStatuses;
using QiProcureDemo.ParamSettings.Dtos;
using QiProcureDemo.ParamSettings;
using QiProcureDemo.Documents.Dtos;
using QiProcureDemo.Documents;
using QiProcureDemo.ServiceImages.Dtos;
using QiProcureDemo.ServiceImages;
using QiProcureDemo.ServicePrices.Dtos;
using QiProcureDemo.ServicePrices;
using QiProcureDemo.ServiceCategories.Dtos;
using QiProcureDemo.ServiceCategories;
using QiProcureDemo.Services.Dtos;
using QiProcureDemo.Services;
using QiProcureDemo.SysRefs.Dtos;
using QiProcureDemo.SysRefs;
using QiProcureDemo.ReferenceTypes.Dtos;
using QiProcureDemo.ReferenceTypes;
using QiProcureDemo.ProductPrices.Dtos;
using QiProcureDemo.ProductPrices;
using QiProcureDemo.ProductCategories.Dtos;
using QiProcureDemo.ProductCategories;
using QiProcureDemo.ProductImages.Dtos;
using QiProcureDemo.ProductImages;
using QiProcureDemo.Products.Dtos;
using QiProcureDemo.Products;
using QiProcureDemo.Categories.Dtos;
using QiProcureDemo.Categories;										  						   
using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.DynamicEntityParameters;
using Abp.EntityHistory;
using Abp.Localization;
using Abp.Notifications;
using Abp.Organizations;
using Abp.UI.Inputs;
using Abp.Webhooks;
using AutoMapper;
using QiProcureDemo.Auditing.Dto;
using QiProcureDemo.Authorization.Accounts.Dto;
using QiProcureDemo.Authorization.Delegation;
using QiProcureDemo.Authorization.Permissions.Dto;
using QiProcureDemo.Authorization.Roles;
using QiProcureDemo.Authorization.Roles.Dto;
using QiProcureDemo.Authorization.Users;
using QiProcureDemo.Authorization.Users.Delegation.Dto;
using QiProcureDemo.Authorization.Users.Dto;
using QiProcureDemo.Authorization.Users.Importing.Dto;
using QiProcureDemo.Authorization.Users.Profile.Dto;
using QiProcureDemo.Chat;
using QiProcureDemo.Chat.Dto;
using QiProcureDemo.DynamicEntityParameters.Dto;
using QiProcureDemo.Editions;
using QiProcureDemo.Editions.Dto;
using QiProcureDemo.Friendships;
using QiProcureDemo.Friendships.Cache;
using QiProcureDemo.Friendships.Dto;
using QiProcureDemo.Localization.Dto;
using QiProcureDemo.MultiTenancy;
using QiProcureDemo.MultiTenancy.Dto;
using QiProcureDemo.MultiTenancy.HostDashboard.Dto;
using QiProcureDemo.MultiTenancy.Payments;
using QiProcureDemo.MultiTenancy.Payments.Dto;
using QiProcureDemo.Notifications.Dto;
using QiProcureDemo.Organizations.Dto;
using QiProcureDemo.Sessions.Dto;
using QiProcureDemo.WebHooks.Dto;

namespace QiProcureDemo
{
    internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
           configuration.CreateMap<CreateOrEditApprovalRequestDto, ApprovalRequest>().ReverseMap();
            configuration.CreateMap<ApprovalRequestDto, ApprovalRequest>().ReverseMap();
            configuration.CreateMap<CreateOrEditProjectInstructionDto, ProjectInstruction>().ReverseMap();
            configuration.CreateMap<ProjectInstructionDto, ProjectInstruction>().ReverseMap();
            configuration.CreateMap<CreateOrEditEmailDto, Email>().ReverseMap();
            configuration.CreateMap<EmailDto, Email>().ReverseMap();
            configuration.CreateMap<CreateOrEditApprovalDto, Approval>().ReverseMap();
            configuration.CreateMap<ApprovalDto, Approval>().ReverseMap();
            configuration.CreateMap<CreateOrEditProjectDto, Project>().ReverseMap();
            configuration.CreateMap<ProjectDto, Project>().ReverseMap();
            configuration.CreateMap<CreateOrEditAccountDto, Account>().ReverseMap();
            configuration.CreateMap<AccountDto, Account>().ReverseMap();
            configuration.CreateMap<CreateOrEditTeamMemberDto, TeamMember>().ReverseMap();
            configuration.CreateMap<TeamMemberDto, TeamMember>().ReverseMap();
            configuration.CreateMap<CreateOrEditTeamDto, Team>().ReverseMap();
            configuration.CreateMap<TeamDto, Team>().ReverseMap();
            configuration.CreateMap<CreateOrEditSysStatusDto, SysStatus>().ReverseMap();
            configuration.CreateMap<SysStatusDto, SysStatus>().ReverseMap();
            configuration.CreateMap<CreateOrEditParamSettingDto, ParamSetting>().ReverseMap();
            configuration.CreateMap<ParamSettingDto, ParamSetting>().ReverseMap();
            configuration.CreateMap<CreateOrEditDocumentDto, Document>().ReverseMap();
            configuration.CreateMap<DocumentDto, Document>().ReverseMap();
            configuration.CreateMap<CreateOrEditServiceImageDto, ServiceImage>().ReverseMap();
            configuration.CreateMap<ServiceImageDto, ServiceImage>().ReverseMap();
            configuration.CreateMap<CreateOrEditServicePriceDto, ServicePrice>().ReverseMap();
            configuration.CreateMap<ServicePriceDto, ServicePrice>().ReverseMap();
            configuration.CreateMap<CreateOrEditServiceCategoryDto, ServiceCategory>().ReverseMap();
            configuration.CreateMap<ServiceCategoryDto, ServiceCategory>().ReverseMap();
            configuration.CreateMap<CreateOrEditServiceDto, Service>().ReverseMap();
            configuration.CreateMap<ServiceDto, Service>().ReverseMap();
            configuration.CreateMap<CreateOrEditSysRefDto, SysRef>().ReverseMap();
            configuration.CreateMap<SysRefDto, SysRef>().ReverseMap();
            configuration.CreateMap<CreateOrEditReferenceTypeDto, ReferenceType>().ReverseMap();
            configuration.CreateMap<ReferenceTypeDto, ReferenceType>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductPriceDto, ProductPrice>().ReverseMap();
            configuration.CreateMap<ProductPriceDto, ProductPrice>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductCategoryDto, ProductCategory>().ReverseMap();
            configuration.CreateMap<ProductCategoryDto, ProductCategory>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductImageDto, ProductImage>().ReverseMap();
            configuration.CreateMap<ProductImageDto, ProductImage>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductDto, Product>().ReverseMap();
            configuration.CreateMap<ProductDto, Product>().ReverseMap();
            configuration.CreateMap<CreateOrEditCategoryDto, Category>().ReverseMap();
            configuration.CreateMap<CategoryDto, Category>().ReverseMap();														  
            //Inputs
            configuration.CreateMap<CheckboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<SingleLineStringInputType, FeatureInputTypeDto>();
            configuration.CreateMap<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<IInputType, FeatureInputTypeDto>()
                .Include<CheckboxInputType, FeatureInputTypeDto>()
                .Include<SingleLineStringInputType, FeatureInputTypeDto>()
                .Include<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<ILocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>()
                .Include<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<LocalizableComboboxItem, LocalizableComboboxItemDto>();
            configuration.CreateMap<ILocalizableComboboxItem, LocalizableComboboxItemDto>()
                .Include<LocalizableComboboxItem, LocalizableComboboxItemDto>();

            //Chat
            configuration.CreateMap<ChatMessage, ChatMessageDto>();
            configuration.CreateMap<ChatMessage, ChatMessageExportDto>();

            //Feature
            configuration.CreateMap<FlatFeatureSelectDto, Feature>().ReverseMap();
            configuration.CreateMap<Feature, FlatFeatureDto>();

            //Role
            configuration.CreateMap<RoleEditDto, Role>().ReverseMap();
            configuration.CreateMap<Role, RoleListDto>();
            configuration.CreateMap<UserRole, UserListRoleDto>();

            //Edition
            configuration.CreateMap<EditionEditDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<EditionCreateDto, SubscribableEdition>();
            configuration.CreateMap<EditionSelectDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<Edition, EditionInfoDto>().Include<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<SubscribableEdition, EditionListDto>();
            configuration.CreateMap<Edition, EditionEditDto>();
            configuration.CreateMap<Edition, SubscribableEdition>();
            configuration.CreateMap<Edition, EditionSelectDto>();


            //Payment
            configuration.CreateMap<SubscriptionPaymentDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPaymentListDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPayment, SubscriptionPaymentInfoDto>();

            //Permission
            configuration.CreateMap<Permission, FlatPermissionDto>();
            configuration.CreateMap<Permission, FlatPermissionWithLevelDto>();

            //Language
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageListDto>();
            configuration.CreateMap<NotificationDefinition, NotificationSubscriptionWithDisplayNameDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>()
                .ForMember(ldto => ldto.IsEnabled, options => options.MapFrom(l => !l.IsDisabled));

            //Tenant
            configuration.CreateMap<Tenant, RecentTenant>();
            configuration.CreateMap<Tenant, TenantLoginInfoDto>();
            configuration.CreateMap<Tenant, TenantListDto>();
            configuration.CreateMap<TenantEditDto, Tenant>().ReverseMap();
            configuration.CreateMap<CurrentTenantInfoDto, Tenant>().ReverseMap();

            //User
            configuration.CreateMap<User, UserEditDto>()
                .ForMember(dto => dto.Password, options => options.Ignore())
                .ReverseMap()
                .ForMember(user => user.Password, options => options.Ignore());
            configuration.CreateMap<User, UserLoginInfoDto>();
            configuration.CreateMap<User, UserListDto>();
            configuration.CreateMap<User, UserProfileListDto>();																
            configuration.CreateMap<User, ChatUserDto>();
            configuration.CreateMap<User, OrganizationUnitUserListDto>();
            configuration.CreateMap<Role, OrganizationUnitRoleListDto>();
            configuration.CreateMap<CurrentUserProfileEditDto, User>().ReverseMap();
            configuration.CreateMap<UserLoginAttemptDto, UserLoginAttempt>().ReverseMap();
            configuration.CreateMap<ImportUserDto, User>();

            //AuditLog
            configuration.CreateMap<AuditLog, AuditLogListDto>();
            configuration.CreateMap<EntityChange, EntityChangeListDto>();
            configuration.CreateMap<EntityPropertyChange, EntityPropertyChangeDto>();

            //Friendship
            configuration.CreateMap<Friendship, FriendDto>();
            configuration.CreateMap<FriendCacheItem, FriendDto>();

            //OrganizationUnit
            configuration.CreateMap<OrganizationUnit, OrganizationUnitDto>();

            //Webhooks
            configuration.CreateMap<WebhookSubscription, GetAllSubscriptionsOutput>();
            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOutput>()
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.WebhookName,
                    options => options.MapFrom(l => l.WebhookEvent.WebhookName))
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.Data,
                    options => options.MapFrom(l => l.WebhookEvent.Data));

            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOfWebhookEventOutput>();

            configuration.CreateMap<DynamicParameter, DynamicParameterDto>().ReverseMap();
            configuration.CreateMap<DynamicParameterValue, DynamicParameterValueDto>().ReverseMap();
            configuration.CreateMap<EntityDynamicParameter, EntityDynamicParameterDto>()
                .ForMember(dto => dto.DynamicParameterName,
                    options => options.MapFrom(entity => entity.DynamicParameter.ParameterName));
            configuration.CreateMap<EntityDynamicParameterDto, EntityDynamicParameter>();

            configuration.CreateMap<EntityDynamicParameterValue, EntityDynamicParameterValueDto>().ReverseMap();
            //User Delegations
            configuration.CreateMap<CreateUserDelegationDto, UserDelegation>();


            /* ADD YOUR OWN CUSTOM AUTOMAPPER MAPPINGS HERE */
        }
    }
}
