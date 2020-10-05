import { PermissionCheckerService } from 'abp-ng2-module';
import { AppSessionService } from '@shared/common/session/app-session.service';

import { Injectable } from '@angular/core';
import { AppMenu } from './app-menu';
import { AppMenuItem } from './app-menu-item';

@Injectable()
export class AppNavigationService {

    constructor(
        private _permissionCheckerService: PermissionCheckerService,
        private _appSessionService: AppSessionService
    ) {

    }

    getMenu(): AppMenu {
        return new AppMenu('MainMenu', 'MainMenu', [
            new AppMenuItem('Dashboard', 'Pages.Administration.Host.Dashboard', 'flaticon-line-graph', '/app/admin/hostDashboard'),
            new AppMenuItem('Dashboard', 'Pages.Tenant.Dashboard', 'flaticon-line-graph', '/app/main/dashboard'),
            new AppMenuItem('Tenants', 'Pages.Tenants', 'flaticon-list-3', '/app/admin/tenants'),
            new AppMenuItem('Editions', 'Pages.Editions', 'flaticon-app', '/app/admin/editions'),        

            new AppMenuItem('Management', '', 'flaticon-more', '', [],[
                new AppMenuItem('Products', '', 'flaticon-more', '',[], [
                    new AppMenuItem('Products', 'Pages.Management_Products', 'flaticon-folder', '/app/main/products/products'),            
                    new AppMenuItem('ProductImages', 'Pages.Management_ProductImages', 'flaticon2-image-file', '/app/main/productImages/productImages'),            
                    new AppMenuItem('ProductCategories', 'Pages.Management_ProductCategories', 'flaticon-layers', '/app/main/productCategories/productCategories'),
                    new AppMenuItem('ProductPrices', 'Pages.Management_ProductPrices', 'flaticon-price-tag', '/app/main/productPrices/productPrices')
                ]),
                new AppMenuItem('Services', '', 'flaticon-more', '',[], [
                    new AppMenuItem('Services', 'Pages.Management_Services', 'flaticon-folder', '/app/main/services/services'),            
                    new AppMenuItem('ServiceImages', 'Pages.Management_ServiceImages', 'flaticon-more', '/app/main/serviceImages/serviceImages'),        
                    new AppMenuItem('ServiceCategories', 'Pages.Management_ServiceCategories', 'flaticon-more', '/app/main/serviceCategories/serviceCategories'),            
                    new AppMenuItem('ServicePrices', 'Pages.Management_ServicePrices', 'flaticon-more', '/app/main/servicePrices/servicePrices')            
                ])    
            ]),
            
            new AppMenuItem('ServicePrices', 'Pages.ServicePrices', 'flaticon-more', '/app/main/servicePrices/servicePrices'),            
            new AppMenuItem('Documents', 'Pages.Documents', 'flaticon-more', '/app/main/documents/documents'),            
            new AppMenuItem('Teams', 'Pages.Teams', 'flaticon-more', '/app/main/teams/teams'),            
            new AppMenuItem('TeamMembers', 'Pages.TeamMembers', 'flaticon-more', '/app/main/teamMembers/teamMembers'),            
            new AppMenuItem('Accounts', 'Pages.Accounts', 'flaticon-more', '/app/main/accounts/accounts'),            
            new AppMenuItem('Projects', 'Pages.Projects', 'flaticon-more', '/app/main/projects/projects'),            
            new AppMenuItem('Approvals', 'Pages.Approvals', 'flaticon-more', '/app/main/approvals/approvals'),            
            new AppMenuItem('Emails', 'Pages.Emails', 'flaticon-more', '/app/main/emails/emails'),            
            new AppMenuItem('ProjectInstructions', 'Pages.ProjectInstructions', 'flaticon-more', '/app/main/projectInstructions/projectInstructions'),            
            new AppMenuItem('ApprovalRequests', 'Pages.ApprovalRequests', 'flaticon-more', '/app/main/approvalRequests/approvalRequests'),            
            
            new AppMenuItem('Administration', '', 'flaticon-interface-8', '', [], [
                new AppMenuItem('OrganizationUnits', 'Pages.Administration.OrganizationUnits', 'flaticon-map', '/app/admin/organization-units'),
                new AppMenuItem('Roles', 'Pages.Administration.Roles', 'flaticon-suitcase', '/app/admin/roles'),
                new AppMenuItem('Users', 'Pages.Administration.Users', 'flaticon-users', '/app/admin/users'),
                new AppMenuItem('Languages', 'Pages.Administration.Languages', 'flaticon-tabs', '/app/admin/languages', ['/app/admin/languages/{name}/texts']),
                new AppMenuItem('AuditLogs', 'Pages.Administration.AuditLogs', 'flaticon-folder-1', '/app/admin/auditLogs'),
                new AppMenuItem('Maintenance', 'Pages.Administration.Host.Maintenance', 'flaticon-lock', '/app/admin/maintenance'),
                new AppMenuItem('Subscription', 'Pages.Administration.Tenant.SubscriptionManagement', 'flaticon-refresh', '/app/admin/subscription-management'),
                new AppMenuItem('VisualSettings', 'Pages.Administration.UiCustomization', 'flaticon-medical', '/app/admin/ui-customization'),
                new AppMenuItem('WebhookSubscriptions', 'Pages.Administration.WebhookSubscription', 'flaticon2-world', '/app/admin/webhook-subscriptions'),
                new AppMenuItem('DynamicParameters', '', 'flaticon-interface-8', '', [], [
                    new AppMenuItem('Definitions', 'Pages.Administration.DynamicParameters', '', '/app/admin/dynamic-parameter'),
                    new AppMenuItem('EntityDynamicParameters', 'Pages.Administration.EntityDynamicParameters', '', '/app/admin/entity-dynamic-parameter'),
                ]),
                new AppMenuItem('Settings', 'Pages.Administration.Host.Settings', 'flaticon-settings', '/app/admin/hostSettings'),
                new AppMenuItem('Settings', 'Pages.Administration.Tenant.Settings', 'flaticon-settings', '/app/admin/tenantSettings'),
                
                new AppMenuItem('SysRefs', 'Pages.Administration.SysRefs', 'flaticon-more', '/app/admin/sysRefs/sysRefs'),            
                new AppMenuItem('ReferenceTypes', 'Pages.Administration.ReferenceTypes', 'flaticon-more', '/app/admin/referenceTypes/referenceTypes'),                
                new AppMenuItem('SysStatuses', 'Pages.Administration.SysStatuses', 'flaticon-more', '/app/admin/sysStatuses/sysStatuses'),                
                new AppMenuItem('ParamSettings', 'Pages.Administration.ParamSettings', 'flaticon-more', '/app/admin/paramSettings/paramSettings'),
                new AppMenuItem('Categories', 'Pages.Administration.Categories', 'flaticon-more', '/app/admin/categories/categories'),            
                new AppMenuItem('References', '', 'flaticon-more', '',[], [
                    new AppMenuItem('SysRefs', 'Pages.Administration_Reference.SysRefs', 'flaticon-more', '/app/admin/sysRefs/sysRefs'),            
                    new AppMenuItem('ReferenceTypes', 'Pages.Administration_Reference.ReferenceTypes', 'flaticon-more', '/app/admin/referenceTypes/referenceTypes')        
                ])
            ]),

            
            new AppMenuItem('DemoUiComponents', 'Pages.DemoUiComponents', 'flaticon-shapes', '/app/admin/demo-ui-components')
        ]);
    }

    checkChildMenuItemPermission(menuItem): boolean {

        for (let i = 0; i < menuItem.items.length; i++) {
            let subMenuItem = menuItem.items[i];

            if (subMenuItem.permissionName === '' || subMenuItem.permissionName === null) {
                if (subMenuItem.route) {
                    return true;
                }
            } else if (this._permissionCheckerService.isGranted(subMenuItem.permissionName)) {
                return true;
            }

            if (subMenuItem.items && subMenuItem.items.length) {
                let isAnyChildItemActive = this.checkChildMenuItemPermission(subMenuItem);
                if (isAnyChildItemActive) {
                    return true;
                }
            }
        }
        return false;
    }

    showMenuItem(menuItem: AppMenuItem): boolean {
        if (menuItem.permissionName === 'Pages.Administration.Tenant.SubscriptionManagement' && this._appSessionService.tenant && !this._appSessionService.tenant.edition) {
            return false;
        }

        let hideMenuItem = false;

        if (menuItem.requiresAuthentication && !this._appSessionService.user) {
            hideMenuItem = true;
        }

        if (menuItem.permissionName && !this._permissionCheckerService.isGranted(menuItem.permissionName)) {
            hideMenuItem = true;
        }

        if (this._appSessionService.tenant || !abp.multiTenancy.ignoreFeatureCheckForHostUsers) {
            if (menuItem.hasFeatureDependency() && !menuItem.featureDependencySatisfied()) {
                hideMenuItem = true;
            }
        }

        if (!hideMenuItem && menuItem.items && menuItem.items.length) {
            return this.checkChildMenuItemPermission(menuItem);
        }

        return !hideMenuItem;
    }

    /**
     * Returns all menu items recursively
     */
    getAllMenuItems(): AppMenuItem[] {
        let menu = this.getMenu();
        let allMenuItems: AppMenuItem[] = [];
        menu.items.forEach(menuItem => {
            allMenuItems = allMenuItems.concat(this.getAllMenuItemsRecursive(menuItem));
        });

        return allMenuItems;
    }

    private getAllMenuItemsRecursive(menuItem: AppMenuItem): AppMenuItem[] {
        if (!menuItem.items) {
            return [menuItem];
        }

        let menuItems = [menuItem];
        menuItem.items.forEach(subMenu => {
            menuItems = menuItems.concat(this.getAllMenuItemsRecursive(subMenu));
        });

        return menuItems;
    }
}
