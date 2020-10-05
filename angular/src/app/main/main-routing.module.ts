import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ApprovalRequestsComponent } from './approvalRequests/approvalRequests/approvalRequests.component';
import { ProjectInstructionsComponent } from './projectInstructions/projectInstructions/projectInstructions.component';
import { EmailsComponent } from './emails/emails/emails.component';
import { ApprovalsComponent } from './approvals/approvals/approvals.component';
import { ProjectsComponent } from './projects/projects/projects.component';
import { AccountsComponent } from './accounts/accounts/accounts.component';
import { TeamMembersComponent } from './teamMembers/teamMembers/teamMembers.component';
import { TeamsComponent } from './teams/teams/teams.component';
import { DocumentsComponent } from './documents/documents/documents.component';
import { ServicePricesComponent } from './servicePrices/servicePrices/servicePrices.component';
import { ServiceImagesComponent } from './serviceImages/serviceImages/serviceImages.component';
import { ServiceCategoriesComponent } from './serviceCategories/serviceCategories/serviceCategories.component';
import { ServicesComponent } from './services/services/services.component';
import { ProductPricesComponent } from './productPrices/productPrices/productPrices.component';
import { ProductCategoriesComponent } from './productCategories/productCategories/productCategories.component';
import { ProductImagesComponent } from './productImages/productImages/productImages.component';
import { ProductsComponent } from './products/products/products.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                children: [
                    { path: 'approvalRequests/approvalRequests', component: ApprovalRequestsComponent, data: { permission: 'Pages.ApprovalRequests' }  },
                    { path: 'projectInstructions/projectInstructions', component: ProjectInstructionsComponent, data: { permission: 'Pages.ProjectInstructions' }  },
                    { path: 'emails/emails', component: EmailsComponent, data: { permission: 'Pages.Emails' }  },
                    { path: 'approvals/approvals', component: ApprovalsComponent, data: { permission: 'Pages.Approvals' }  },
                    { path: 'projects/projects', component: ProjectsComponent, data: { permission: 'Pages.Projects' }  },
                    { path: 'accounts/accounts', component: AccountsComponent, data: { permission: 'Pages.Accounts' }  },
                    { path: 'teamMembers/teamMembers', component: TeamMembersComponent, data: { permission: 'Pages.TeamMembers' }  },
                    { path: 'teams/teams', component: TeamsComponent, data: { permission: 'Pages.Teams' }  },
                    { path: 'documents/documents', component: DocumentsComponent, data: { permission: 'Pages.Documents' }  },
                    { path: 'servicePrices/servicePrices', component: ServicePricesComponent, data: { permission: 'Pages.Management_ServicePrices' }  },
                    { path: 'serviceImages/serviceImages', component: ServiceImagesComponent, data: { permission: 'Pages.Management_ServiceImages' }  },
                    { path: 'servicePrices/servicePrices', component: ServicePricesComponent, data: { permission: 'Pages.Management_ServicePrices' }  },
                    { path: 'serviceCategories/serviceCategories', component: ServiceCategoriesComponent, data: { permission: 'Pages.Management_ServiceCategories' }  },
                    { path: 'services/services', component: ServicesComponent, data: { permission: 'Pages.Management_Services' }  },
                    { path: 'productPrices/productPrices', component: ProductPricesComponent, data: { permission: 'Pages.Management_ProductPrices' }  },
                    { path: 'productCategories/productCategories', component: ProductCategoriesComponent, data: { permission: 'Pages.Management_ProductCategories' }  },
                    { path: 'productImages/productImages', component: ProductImagesComponent, data: { permission: 'Pages.Management_ProductImages' }  },
                    { path: 'products/products', component: ProductsComponent, data: { permission: 'Pages.Management_Products' }  },
                    { path: 'dashboard', component: DashboardComponent, data: { permission: 'Pages.Tenant.Dashboard' } },
                    { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
                    { path: '**', redirectTo: 'dashboard' }
                ]
            }
        ])
    ],
    exports: [
        RouterModule
    ]
})
export class MainRoutingModule { }
