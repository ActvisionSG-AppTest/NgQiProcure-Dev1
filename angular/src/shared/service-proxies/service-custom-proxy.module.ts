import { NgModule } from '@angular/core';
import * as ApiServiceCustomProxies from './service-custom-proxies';
import { RefreshTokenService, AbpHttpInterceptor } from 'abp-ng2-module';
import { ZeroRefreshTokenService } from '@account/auth/zero-refresh-token.service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';

@NgModule({
    providers: [
        ApiServiceCustomProxies.AccountsCustomServiceProxy,           
        ApiServiceCustomProxies.ApprovalsCustomServiceProxy,       
        ApiServiceCustomProxies.ApprovalRequestsCustomServiceProxy,  
        ApiServiceCustomProxies.CategoriesCustomServiceByServiceProxy,  
        ApiServiceCustomProxies.CommonQueryServiceProxy,  
        ApiServiceCustomProxies.CategoriesCustomServiceByProductProxy,          
        ApiServiceCustomProxies.CategoriesCustomServiceProxy,
        ApiServiceCustomProxies.DocumentsCustomServiceProxy, 
        ApiServiceCustomProxies.EmailsCustomServiceProxy, 
        ApiServiceCustomProxies.ParamSettingsCustomServiceProxy, 
        ApiServiceCustomProxies.ProductsCustomServiceProxy,
        ApiServiceCustomProxies.ProductCategoriesCustomServiceProxy,  
        ApiServiceCustomProxies.ProductImagesCustomServiceProxy, 
        ApiServiceCustomProxies.ProjectsCustomServiceProxy,
        ApiServiceCustomProxies.ProjectInstructionsCustomServiceProxy,
        ApiServiceCustomProxies.ProductPricesCustomServiceProxy,   
        ApiServiceCustomProxies.ReferenceTypesCustomServiceProxy,
        ApiServiceCustomProxies.ServicesCustomServiceProxy,
        ApiServiceCustomProxies.ServiceCategoriesCustomServiceProxy,  
        ApiServiceCustomProxies.ServiceImagesCustomServiceProxy,        
        ApiServiceCustomProxies.ServicePricesCustomServiceProxy,        
        ApiServiceCustomProxies.SysRefsCustomServiceProxy,  
        ApiServiceCustomProxies.SysStatusesCustomServiceProxy,       
        ApiServiceCustomProxies.TeamMembersCustomServiceProxy,     
        ApiServiceCustomProxies.TeamMembersCustomServiceProxy,
        ApiServiceCustomProxies.TeamsCustomServiceProxy,
        ApiServiceCustomProxies.UserCustomServiceProxy,     
        { provide: RefreshTokenService, useClass: ZeroRefreshTokenService },
        { provide: HTTP_INTERCEPTORS, useClass: AbpHttpInterceptor, multi: true }             
    ]
})
export class ServiceCustomProxyModule { }
