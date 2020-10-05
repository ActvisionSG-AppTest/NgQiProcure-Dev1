using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;

namespace QiProcureDemo.Authorization
{
    /// <summary>
    /// Application's authorization provider.
    /// Defines permissions for the application.
    /// See <see cref="AppPermissions"/> for all permission names.
    /// </summary>
    public class AppAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public AppAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public AppAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

            var approvalRequests = pages.CreateChildPermission(AppPermissions.Pages_ApprovalRequests, L("ApprovalRequests"));
            approvalRequests.CreateChildPermission(AppPermissions.Pages_ApprovalRequests_Create, L("CreateNewApprovalRequest"));
            approvalRequests.CreateChildPermission(AppPermissions.Pages_ApprovalRequests_Edit, L("EditApprovalRequest"));
            approvalRequests.CreateChildPermission(AppPermissions.Pages_ApprovalRequests_Delete, L("DeleteApprovalRequest"));



            var projectInstructions = pages.CreateChildPermission(AppPermissions.Pages_ProjectInstructions, L("ProjectInstructions"));
            projectInstructions.CreateChildPermission(AppPermissions.Pages_ProjectInstructions_Create, L("CreateNewProjectInstruction"));
            projectInstructions.CreateChildPermission(AppPermissions.Pages_ProjectInstructions_Edit, L("EditProjectInstruction"));
            projectInstructions.CreateChildPermission(AppPermissions.Pages_ProjectInstructions_Delete, L("DeleteProjectInstruction"));



            var emails = pages.CreateChildPermission(AppPermissions.Pages_Emails, L("Emails"));
            emails.CreateChildPermission(AppPermissions.Pages_Emails_Create, L("CreateNewEmail"));
            emails.CreateChildPermission(AppPermissions.Pages_Emails_Edit, L("EditEmail"));
            emails.CreateChildPermission(AppPermissions.Pages_Emails_Delete, L("DeleteEmail"));



            var approvals = pages.CreateChildPermission(AppPermissions.Pages_Approvals, L("Approvals"));
            approvals.CreateChildPermission(AppPermissions.Pages_Approvals_Create, L("CreateNewApproval"));
            approvals.CreateChildPermission(AppPermissions.Pages_Approvals_Edit, L("EditApproval"));
            approvals.CreateChildPermission(AppPermissions.Pages_Approvals_Delete, L("DeleteApproval"));



            var projects = pages.CreateChildPermission(AppPermissions.Pages_Projects, L("Projects"));
            projects.CreateChildPermission(AppPermissions.Pages_Projects_Create, L("CreateNewProject"));
            projects.CreateChildPermission(AppPermissions.Pages_Projects_Edit, L("EditProject"));
            projects.CreateChildPermission(AppPermissions.Pages_Projects_Delete, L("DeleteProject"));



            var accounts = pages.CreateChildPermission(AppPermissions.Pages_Accounts, L("Accounts"));
            accounts.CreateChildPermission(AppPermissions.Pages_Accounts_Create, L("CreateNewAccount"));
            accounts.CreateChildPermission(AppPermissions.Pages_Accounts_Edit, L("EditAccount"));
            accounts.CreateChildPermission(AppPermissions.Pages_Accounts_Delete, L("DeleteAccount"));



            var teamMembers = pages.CreateChildPermission(AppPermissions.Pages_TeamMembers, L("TeamMembers"));
            teamMembers.CreateChildPermission(AppPermissions.Pages_TeamMembers_Create, L("CreateNewTeamMember"));
            teamMembers.CreateChildPermission(AppPermissions.Pages_TeamMembers_Edit, L("EditTeamMember"));
            teamMembers.CreateChildPermission(AppPermissions.Pages_TeamMembers_Delete, L("DeleteTeamMember"));



            var teams = pages.CreateChildPermission(AppPermissions.Pages_Teams, L("Teams"));
            teams.CreateChildPermission(AppPermissions.Pages_Teams_Create, L("CreateNewTeam"));
            teams.CreateChildPermission(AppPermissions.Pages_Teams_Edit, L("EditTeam"));
            teams.CreateChildPermission(AppPermissions.Pages_Teams_Delete, L("DeleteTeam"));



            var documents = pages.CreateChildPermission(AppPermissions.Pages_Documents, L("Documents"));
            documents.CreateChildPermission(AppPermissions.Pages_Documents_Create, L("CreateNewDocument"));
            documents.CreateChildPermission(AppPermissions.Pages_Documents_Edit, L("EditDocument"));
            documents.CreateChildPermission(AppPermissions.Pages_Documents_Delete, L("DeleteDocument"));



            var management = pages.CreateChildPermission(AppPermissions.Pages_Management, L("Management"));

            var management_product = management.CreateChildPermission(AppPermissions.Pages_Management_Product, L("Management_Product"));

            var products = management_product.CreateChildPermission(AppPermissions.Pages_Management_Product_Products, L("Products"));
            products.CreateChildPermission(AppPermissions.Pages_Management_Product_Products_Create, L("CreateNewProduct"));
            products.CreateChildPermission(AppPermissions.Pages_Management_Product_Products_Edit, L("EditProduct"));
            products.CreateChildPermission(AppPermissions.Pages_Management_Product_Products_Delete, L("DeleteProduct"));

            var productPrices = management_product.CreateChildPermission(AppPermissions.Pages_Management_ProductPrices, L("ProductPrices"));
            productPrices.CreateChildPermission(AppPermissions.Pages_Management_ProductPrices_Create, L("CreateNewProductPrice"));
            productPrices.CreateChildPermission(AppPermissions.Pages_Management_ProductPrices_Edit, L("EditProductPrice"));
            productPrices.CreateChildPermission(AppPermissions.Pages_Management_ProductPrices_Delete, L("DeleteProductPrice"));

            var productCategories = management_product.CreateChildPermission(AppPermissions.Pages_Management_ProductCategories, L("ProductCategories"));
            productCategories.CreateChildPermission(AppPermissions.Pages_Management_ProductCategories_Create, L("CreateNewProductCategory"));
            productCategories.CreateChildPermission(AppPermissions.Pages_Management_ProductCategories_Edit, L("EditProductCategory"));
            productCategories.CreateChildPermission(AppPermissions.Pages_Management_ProductCategories_Delete, L("DeleteProductCategory"));

            var productImages = management_product.CreateChildPermission(AppPermissions.Pages_Management_ProductImages, L("ProductImages"));
            productImages.CreateChildPermission(AppPermissions.Pages_Management_ProductImages_Create, L("CreateNewProductImage"));
            productImages.CreateChildPermission(AppPermissions.Pages_Management_ProductImages_Edit, L("EditProductImage"));
            productImages.CreateChildPermission(AppPermissions.Pages_Management_ProductImages_Delete, L("DeleteProductImage"));

            var management_service = management.CreateChildPermission(AppPermissions.Pages_Management_Service, L("Management_Service"));

            var services = management_service.CreateChildPermission(AppPermissions.Pages_Management_Service_Services, L("Services"));
            services.CreateChildPermission(AppPermissions.Pages_Management_Service_Services_Create, L("CreateNewService"));
            services.CreateChildPermission(AppPermissions.Pages_Management_Service_Services_Edit, L("EditService"));
            services.CreateChildPermission(AppPermissions.Pages_Management_Service_Services_Delete, L("DeleteService"));

            var serviceImages = management_service.CreateChildPermission(AppPermissions.Pages_Management_ServiceImages, L("ServiceImages"));
            serviceImages.CreateChildPermission(AppPermissions.Pages_Management_ServiceImages_Create, L("CreateNewServiceImage"));
            serviceImages.CreateChildPermission(AppPermissions.Pages_Management_ServiceImages_Edit, L("EditServiceImage"));
            serviceImages.CreateChildPermission(AppPermissions.Pages_Management_ServiceImages_Delete, L("DeleteServiceImage"));

            var servicePrices = management_service.CreateChildPermission(AppPermissions.Pages_Management_ServicePrices, L("ServicePrices"));
            servicePrices.CreateChildPermission(AppPermissions.Pages_Management_ServicePrices_Create, L("CreateNewServicePrice"));
            servicePrices.CreateChildPermission(AppPermissions.Pages_Management_ServicePrices_Edit, L("EditServicePrice"));
            servicePrices.CreateChildPermission(AppPermissions.Pages_Management_ServicePrices_Delete, L("DeleteServicePrice"));

            var serviceCategories = management_service.CreateChildPermission(AppPermissions.Pages_Management_ServiceCategories, L("ServiceCategories"));
            serviceCategories.CreateChildPermission(AppPermissions.Pages_Management_ServiceCategories_Create, L("CreateNewServiceCategory"));
            serviceCategories.CreateChildPermission(AppPermissions.Pages_Management_ServiceCategories_Edit, L("EditServiceCategory"));
            serviceCategories.CreateChildPermission(AppPermissions.Pages_Management_ServiceCategories_Delete, L("DeleteServiceCategory"));




            pages.CreateChildPermission(AppPermissions.Pages_DemoUiComponents, L("DemoUiComponents"));

            var administration = pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

           /* var sysRefs = administration.CreateChildPermission(AppPermissions.Pages_Administration_SysRefs, L("SysRefs"));
            sysRefs.CreateChildPermission(AppPermissions.Pages_Administration_SysRefs_Create, L("CreateNewSysRef"));
            sysRefs.CreateChildPermission(AppPermissions.Pages_Administration_SysRefs_Edit, L("EditSysRef"));
            sysRefs.CreateChildPermission(AppPermissions.Pages_Administration_SysRefs_Delete, L("DeleteSysRef"));*/



            var SysStatuses = administration.CreateChildPermission(AppPermissions.Pages_Administration_SysStatuses, L("SysStatuses"));
            SysStatuses.CreateChildPermission(AppPermissions.Pages_Administration_SysStatuses_Create, L("CreateNewSysStatus"));
            SysStatuses.CreateChildPermission(AppPermissions.Pages_Administration_SysStatuses_Edit, L("EditSysStatus"));
            SysStatuses.CreateChildPermission(AppPermissions.Pages_Administration_SysStatuses_Delete, L("DeleteSysStatus"));



            var paramSettings = administration.CreateChildPermission(AppPermissions.Pages_Administration_ParamSettings, L("ParamSettings"));
            paramSettings.CreateChildPermission(AppPermissions.Pages_Administration_ParamSettings_Create, L("CreateNewParamSetting"));
            paramSettings.CreateChildPermission(AppPermissions.Pages_Administration_ParamSettings_Edit, L("EditParamSetting"));
            paramSettings.CreateChildPermission(AppPermissions.Pages_Administration_ParamSettings_Delete, L("DeleteParamSetting"));



            var administration_reference = administration.CreateChildPermission(AppPermissions.Pages_Administration_Reference, L("Administration_Reference"));

            var sysRefs = administration_reference.CreateChildPermission(AppPermissions.Pages_Administration_Reference_SysRefs, L("SysRefs"));
            sysRefs.CreateChildPermission(AppPermissions.Pages_Administration_Reference_SysRefs_Create, L("CreateNewSysRef"));
            sysRefs.CreateChildPermission(AppPermissions.Pages_Administration_Reference_SysRefs_Edit, L("EditSysRef"));
            sysRefs.CreateChildPermission(AppPermissions.Pages_Administration_Reference_SysRefs_Delete, L("DeleteSysRef"));



            var referenceTypes = administration_reference.CreateChildPermission(AppPermissions.Pages_Administration_Reference_ReferenceTypes, L("ReferenceTypes"));
            referenceTypes.CreateChildPermission(AppPermissions.Pages_Administration_Reference_ReferenceTypes_Create, L("CreateNewReferenceType"));
            referenceTypes.CreateChildPermission(AppPermissions.Pages_Administration_Reference_ReferenceTypes_Edit, L("EditReferenceType"));
            referenceTypes.CreateChildPermission(AppPermissions.Pages_Administration_Reference_ReferenceTypes_Delete, L("DeleteReferenceType"));



            var categories = administration.CreateChildPermission(AppPermissions.Pages_Administration_Categories, L("Categories"));
            categories.CreateChildPermission(AppPermissions.Pages_Administration_Categories_Create, L("CreateNewCategory"));
            categories.CreateChildPermission(AppPermissions.Pages_Administration_Categories_Edit, L("EditCategory"));
            categories.CreateChildPermission(AppPermissions.Pages_Administration_Categories_Delete, L("DeleteCategory"));


            var roles = administration.CreateChildPermission(AppPermissions.Pages_Administration_Roles, L("Roles"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Create, L("CreatingNewRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Edit, L("EditingRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Delete, L("DeletingRole"));

            var users = administration.CreateChildPermission(AppPermissions.Pages_Administration_Users, L("Users"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Create, L("CreatingNewUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Edit, L("EditingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Delete, L("DeletingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangePermissions, L("ChangingPermissions"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Impersonation, L("LoginForUsers"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Unlock, L("Unlock"));

            var languages = administration.CreateChildPermission(AppPermissions.Pages_Administration_Languages, L("Languages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Create, L("CreatingNewLanguage"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Edit, L("EditingLanguage"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Delete, L("DeletingLanguages"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeTexts, L("ChangingTexts"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs, L("AuditLogs"));

            var organizationUnits = administration.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits, L("OrganizationUnits"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree, L("ManagingOrganizationTree"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers, L("ManagingMembers"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageRoles, L("ManagingRoles"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_UiCustomization, L("VisualSettings"));

            var webhooks = administration.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription, L("Webhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Create, L("CreatingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Edit, L("EditingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_ChangeActivity, L("ChangingWebhookActivity"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Detail, L("DetailingSubscription"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ListSendAttempts, L("ListingSendAttempts"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ResendWebhook, L("ResendingWebhook"));

            var dynamicParameters = administration.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameters, L("DynamicParameters"));
            dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameters_Create, L("CreatingDynamicParameters"));
            dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameters_Edit, L("EditingDynamicParameters"));
            dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameters_Delete, L("DeletingDynamicParameters"));

            var dynamicParameterValues = dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameterValue, L("DynamicParameterValue"));
            dynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameterValue_Create, L("CreatingDynamicParameterValue"));
            dynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameterValue_Edit, L("EditingDynamicParameterValue"));
            dynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameterValue_Delete, L("DeletingDynamicParameterValue"));

            var entityDynamicParameters = dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameters, L("EntityDynamicParameters"));
            entityDynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameters_Create, L("CreatingEntityDynamicParameters"));
            entityDynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameters_Edit, L("EditingEntityDynamicParameters"));
            entityDynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameters_Delete, L("DeletingEntityDynamicParameters"));

            var entityDynamicParameterValues = dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameterValue, L("EntityDynamicParameterValue"));
            entityDynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameterValue_Create, L("CreatingEntityDynamicParameterValue"));
            entityDynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameterValue_Edit, L("EditingEntityDynamicParameterValue"));
            entityDynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameterValue_Delete, L("DeletingEntityDynamicParameterValue"));

            //TENANT-SPECIFIC PERMISSIONS

            pages.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Tenant);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_SubscriptionManagement, L("Subscription"), multiTenancySides: MultiTenancySides.Tenant);

            //HOST-SPECIFIC PERMISSIONS

            var editions = pages.CreateChildPermission(AppPermissions.Pages_Editions, L("Editions"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Create, L("CreatingNewEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Edit, L("EditingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Delete, L("DeletingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_MoveTenantsToAnotherEdition, L("MoveTenantsToAnotherEdition"), multiTenancySides: MultiTenancySides.Host);

            var tenants = pages.CreateChildPermission(AppPermissions.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Create, L("CreatingNewTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Edit, L("EditingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_ChangeFeatures, L("ChangingFeatures"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Delete, L("DeletingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Impersonation, L("LoginForTenants"), multiTenancySides: MultiTenancySides.Host);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Host);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Maintenance, L("Maintenance"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_HangfireDashboard, L("HangfireDashboard"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Host);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, QiProcureDemoConsts.LocalizationSourceName);
        }
    }
}
