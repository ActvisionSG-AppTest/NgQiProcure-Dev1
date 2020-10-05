import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AppCommonModule } from '@app/shared/common/app-common.module';

import { ApprovalRequestsComponent } from './approvalRequests/approvalRequests/approvalRequests.component';
import { ViewApprovalRequestModalComponent } from './approvalRequests/approvalRequests/view-approvalRequest-modal.component';
import { CreateOrEditApprovalRequestModalComponent } from './approvalRequests/approvalRequests/create-or-edit-approvalRequest-modal.component';
import { ApprovalRequestUserLookupTableModalComponent } from './approvalRequests/approvalRequests/approvalRequest-user-lookup-table-modal.component';

import { ApprovalRequestSysRefLookupTableModalComponent } from "./approvalRequests/approvalRequests/approvalRequest-sysRef-lookup-table-modal.component";
import { ApprovalRequestSysStatusLookupTableModalComponent } from "./approvalRequests/approvalRequests/approvalRequest-sysStatus-lookup-table-modal.component";

import { ProjectInstructionsComponent } from "./projectInstructions/projectInstructions/projectInstructions.component";
import { ViewProjectInstructionModalComponent } from "./projectInstructions/projectInstructions/view-projectInstruction-modal.component";
import { CreateOrEditProjectInstructionModalComponent } from "./projectInstructions/projectInstructions/create-or-edit-projectInstruction-modal.component";
import { ProjectInstructionProjectLookupTableModalComponent } from "./projectInstructions/projectInstructions/projectInstruction-project-lookup-table-modal.component";

import { EmailsComponent } from "./emails/emails/emails.component";
import { ViewEmailModalComponent } from "./emails/emails/view-email-modal.component";
import { CreateOrEditEmailModalComponent } from "./emails/emails/create-or-edit-email-modal.component";
import { EmailSysRefLookupTableModalComponent } from "./emails/emails/email-sysRef-lookup-table-modal.component";
import { EmailSysStatusLookupTableModalComponent } from "./emails/emails/email-sysStatus-lookup-table-modal.component";

import { ApprovalsComponent } from "./approvals/approvals/approvals.component";
import { ViewApprovalModalComponent } from "./approvals/approvals/view-approval-modal.component";
import { CreateOrEditApprovalModalComponent } from "./approvals/approvals/create-or-edit-approval-modal.component";
import { ApprovalSysRefLookupTableModalComponent } from "./approvals/approvals/approval-sysRef-lookup-table-modal.component";
import { ApprovalTeamLookupTableModalComponent } from "./approvals/approvals/approval-team-lookup-table-modal.component";
import { ApprovalProjectLookupTableModalComponent } from "./approvals/approvals/approval-project-lookup-table-modal.component";
import { ApprovalAccountLookupTableModalComponent } from "./approvals/approvals/approval-account-lookup-table-modal.component";
import { ApprovalUserLookupTableModalComponent } from "./approvals/approvals/approval-user-lookup-table-modal.component";
import { ApprovalSysStatusLookupTableModalComponent } from "./approvals/approvals/approval-sysStatus-lookup-table-modal.component";

import { ProjectsComponent } from "./projects/projects/projects.component";
import { ViewProjectModalComponent } from "./projects/projects/view-project-modal.component";
import { CreateOrEditProjectModalComponent } from "./projects/projects/create-or-edit-project-modal.component";
import { ProjectAccountLookupTableModalComponent } from "./projects/projects/project-account-lookup-table-modal.component";
import { ProjectTeamLookupTableModalComponent } from "./projects/projects/project-team-lookup-table-modal.component";
import { ProjectSysStatusLookupTableModalComponent } from "./projects/projects/project-sysStatus-lookup-table-modal.component";

import { AccountsComponent } from "./accounts/accounts/accounts.component";
import { ViewAccountModalComponent } from "./accounts/accounts/view-account-modal.component";
import { CreateOrEditAccountModalComponent } from "./accounts/accounts/create-or-edit-account-modal.component";
import { AccountTeamLookupTableModalComponent } from "./accounts/accounts/account-team-lookup-table-modal.component";
import { AccountSysStatusLookupTableModalComponent } from "./accounts/accounts/account-sysStatus-lookup-table-modal.component";

import { TeamReferenceTypeLookupTableModalComponent } from "./teams/teams/team-referenceType-lookup-table-modal.component";

import { TeamMembersComponent } from "./teamMembers/teamMembers/teamMembers.component";
import { ViewTeamMemberModalComponent } from "./teamMembers/teamMembers/view-teamMember-modal.component";
import { CreateOrEditTeamMemberModalComponent } from "./teamMembers/teamMembers/create-or-edit-teamMember-modal.component";
import { TeamMemberTeamLookupTableModalComponent } from "./teamMembers/teamMembers/teamMember-team-lookup-table-modal.component";
import { TeamMemberUserLookupTableModalComponent } from "./teamMembers/teamMembers/teamMember-user-lookup-table-modal.component";
import { TeamMemberSysRefLookupTableModalComponent } from "./teamMembers/teamMembers/teamMember-sysRef-lookup-table-modal.component";
import { TeamMemberSysStatusLookupTableModalComponent } from "./teamMembers/teamMembers/teamMember-sysStatus-lookup-table-modal.component";

import { TeamsComponent } from "./teams/teams/teams.component";
import { ViewTeamModalComponent } from "./teams/teams/view-team-modal.component";
import { CreateOrEditTeamModalComponent } from "./teams/teams/create-or-edit-team-modal.component";
import { TeamSysStatusLookupTableModalComponent } from "./teams/teams/team-sysStatus-lookup-table-modal.component";

import { DocumentsComponent } from "./documents/documents/documents.component";
import { ViewDocumentModalComponent } from "./documents/documents/view-document-modal.component";
import { CreateOrEditDocumentModalComponent } from "./documents/documents/create-or-edit-document-modal.component";
import { DocumentProductLookupTableModalComponent } from "./documents/documents/document-product-lookup-table-modal.component";
import { DocumentServiceLookupTableModalComponent } from "./documents/documents/document-service-lookup-table-modal.component";

import { DocumentSysRefLookupTableModalComponent } from "./documents/documents/document-sysRef-lookup-table-modal.component";

import { ServicePricesComponent } from "./servicePrices/servicePrices/servicePrices.component";
import { ViewServicePriceModalComponent } from "./servicePrices/servicePrices/view-servicePrice-modal.component";
import { CreateOrEditServicePriceModalComponent } from "./servicePrices/servicePrices/create-or-edit-servicePrice-modal.component";
import { ServicePriceServiceLookupTableModalComponent } from "./servicePrices/servicePrices/servicePrice-service-lookup-table-modal.component";

import { ServiceImagesComponent } from "./serviceImages/serviceImages/serviceImages.component";
import { ViewServiceImageModalComponent } from "./serviceImages/serviceImages/view-serviceImage-modal.component";
import { CreateOrEditServiceImageModalComponent } from "./serviceImages/serviceImages/create-or-edit-serviceImage-modal.component";
import { ServiceImageServiceLookupTableModalComponent } from "./serviceImages/serviceImages/serviceImage-service-lookup-table-modal.component";

import { ServiceCategoriesComponent } from "./serviceCategories/serviceCategories/serviceCategories.component";
import { ViewServiceCategoryModalComponent } from "./serviceCategories/serviceCategories/view-serviceCategory-modal.component";
import { CreateOrEditServiceCategoryModalComponent } from "./serviceCategories/serviceCategories/create-or-edit-serviceCategory-modal.component";
import { ServiceCategoryServiceLookupTableModalComponent } from "./serviceCategories/serviceCategories/serviceCategory-service-lookup-table-modal.component";
import { ServiceCategoryCategoryLookupTableModalComponent } from "./serviceCategories/serviceCategories/serviceCategory-category-lookup-table-modal.component";

import { ServicesComponent } from "./services/services/services.component";
import { ViewServiceModalComponent } from "./services/services/view-service-modal.component";
import { CreateOrEditServiceModalComponent } from "./services/services/create-or-edit-service-modal.component";
import { ServiceCategoryLookupTableModalComponent } from "./services/services/service-category-lookup-table-modal.component";
import { ServiceSysRefLookupTableModalComponent } from "./services/services/service-sysRef-lookup-table-modal.component";

import { ProductPricesComponent } from "./productPrices/productPrices/productPrices.component";
import { ViewProductPriceModalComponent } from "./productPrices/productPrices/view-productPrice-modal.component";
import { CreateOrEditProductPriceModalComponent } from "./productPrices/productPrices/create-or-edit-productPrice-modal.component";
import { ProductPriceProductLookupTableModalComponent } from "./productPrices/productPrices/productPrice-product-lookup-table-modal.component";

import { ProductCategoriesComponent } from "./productCategories/productCategories/productCategories.component";
import { ViewProductCategoryModalComponent } from "./productCategories/productCategories/view-productCategory-modal.component";
import { CreateOrEditProductCategoryModalComponent } from "./productCategories/productCategories/create-or-edit-productCategory-modal.component";
import { ProductCategoryProductLookupTableModalComponent } from "./productCategories/productCategories/productCategory-product-lookup-table-modal.component";
import { ProductCategoryCategoryLookupTableModalComponent } from "./productCategories/productCategories/productCategory-category-lookup-table-modal.component";

import { ProductImagesComponent } from "./productImages/productImages/productImages.component";
import { ViewProductImageModalComponent } from "./productImages/productImages/view-productImage-modal.component";
import { CreateOrEditProductImageModalComponent } from "./productImages/productImages/create-or-edit-productImage-modal.component";
import { ProductImageProductLookupTableModalComponent } from "./productImages/productImages/productImage-product-lookup-table-modal.component";

import { ProductsComponent } from "./products/products/products.component";
import { ViewProductModalComponent } from "./products/products/view-product-modal.component";
import { CreateOrEditProductModalComponent } from "./products/products/create-or-edit-product-modal.component";
import { ProductCategoryLookupTableModalComponent } from "./products/products/product-category-lookup-table-modal.component";
import { AutoCompleteModule } from "primeng/autocomplete";
import { PaginatorModule } from "primeng/paginator";
import { EditorModule } from "primeng/editor";
import { InputMaskModule } from "primeng/inputmask";
import { FileUploadModule as PrimeNgFileUploadModule } from "primeng/fileupload";
import { PickListModule } from "primeng/picklist";
import { OrganizationChartModule } from "primeng/organizationchart";
import { NgxGalleryModule } from "ngx-gallery-9";
import { FileUploadModule } from "ng2-file-upload";
import { TableModule } from "primeng/table";
import { PhotoEditorComponent } from "./products/photo-editor/photo-editor.component";
import { ServicePricesListingComponent } from "./services/service-prices-listing/service-prices-listing.component";
import { ProductPricesListingComponent } from "./products/product-prices-listing/product-prices-listing.component";
import { ApprovalListComponent } from "@app/shared/common/approval-list/approval-list.component";

import { UtilsModule } from '@shared/utils/utils.module';
import { CountoModule } from 'angular2-counto';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { PopoverModule } from 'ngx-bootstrap/popover';
import { DashboardComponent } from './dashboard/dashboard.component';
import { MainRoutingModule } from './main-routing.module';
import { NgxChartsModule } from '@swimlane/ngx-charts';

import { PdfViewerModule } from "ng2-pdf-viewer";
import { FileSaverModule } from "ngx-filesaver";
import { NgxExtendedPdfViewerModule } from "ngx-extended-pdf-viewer";

import { BsDatepickerConfig, BsDaterangepickerConfig, BsLocaleService } from 'ngx-bootstrap/datepicker';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { NgxBootstrapDatePickerConfigService } from 'assets/ngx-bootstrap/ngx-bootstrap-datepicker-config.service';

NgxBootstrapDatePickerConfigService.registerNgxBootstrapDatePickerLocales();

@NgModule({
    imports: [
        PrimeNgFileUploadModule,
        AutoCompleteModule,
        PaginatorModule,
        EditorModule,
        NgxGalleryModule,
        NgxExtendedPdfViewerModule,
        PickListModule,
        OrganizationChartModule,
        InputMaskModule,
        TableModule,
        FileUploadModule,
        FileSaverModule,
        PdfViewerModule,
        CommonModule,
        FormsModule,
        ModalModule,
        TabsModule,
        TooltipModule,
        AppCommonModule,
        UtilsModule,
        MainRoutingModule,
        CountoModule,
        NgxChartsModule,
        BsDatepickerModule.forRoot(),
        BsDropdownModule.forRoot(),
        PopoverModule.forRoot()
    ],
    declarations: [
       ApprovalRequestsComponent,
		ViewApprovalRequestModalComponent,		CreateOrEditApprovalRequestModalComponent,
    ApprovalRequestUserLookupTableModalComponent,
        ApprovalRequestsComponent,
        ViewApprovalRequestModalComponent,
        CreateOrEditApprovalRequestModalComponent,
        ApprovalRequestSysRefLookupTableModalComponent,
        ApprovalRequestSysStatusLookupTableModalComponent,
        ProjectInstructionsComponent,
        ViewProjectInstructionModalComponent,
        CreateOrEditProjectInstructionModalComponent,
        ProjectInstructionProjectLookupTableModalComponent,
        EmailsComponent,
        ViewEmailModalComponent,
        CreateOrEditEmailModalComponent,
        EmailSysRefLookupTableModalComponent,
        EmailSysStatusLookupTableModalComponent,
        ApprovalsComponent,
        ViewApprovalModalComponent,
        CreateOrEditApprovalModalComponent,
        ApprovalSysRefLookupTableModalComponent,
        ApprovalTeamLookupTableModalComponent,
        ApprovalProjectLookupTableModalComponent,
        ApprovalAccountLookupTableModalComponent,
        ApprovalUserLookupTableModalComponent,
        ApprovalSysStatusLookupTableModalComponent,
        ProjectsComponent,
        ViewProjectModalComponent,
        CreateOrEditProjectModalComponent,
        ProjectAccountLookupTableModalComponent,
        ProjectTeamLookupTableModalComponent,
        ProjectSysStatusLookupTableModalComponent,
        AccountsComponent,
        ViewAccountModalComponent,
        CreateOrEditAccountModalComponent,
        AccountTeamLookupTableModalComponent,
        AccountSysStatusLookupTableModalComponent,
        TeamsComponent,
        ViewTeamModalComponent,
        CreateOrEditTeamModalComponent,
        TeamReferenceTypeLookupTableModalComponent,
        TeamMembersComponent,
        ViewTeamMemberModalComponent,
        CreateOrEditTeamMemberModalComponent,
        TeamMemberTeamLookupTableModalComponent,
        TeamMemberUserLookupTableModalComponent,
        TeamMemberSysRefLookupTableModalComponent,
        TeamMemberSysStatusLookupTableModalComponent,
        TeamsComponent,
        ViewTeamModalComponent,
        CreateOrEditTeamModalComponent,
        TeamSysStatusLookupTableModalComponent,
        DocumentProductLookupTableModalComponent,
        DocumentServiceLookupTableModalComponent,
        DocumentsComponent,
        ViewDocumentModalComponent,
        CreateOrEditDocumentModalComponent,
        DocumentSysRefLookupTableModalComponent,
        ServicePricesComponent,
        ViewServicePriceModalComponent,
        CreateOrEditServicePriceModalComponent,
        ServicePriceServiceLookupTableModalComponent,
        ServiceImagesComponent,
        ViewServiceImageModalComponent,
        CreateOrEditServiceImageModalComponent,
        ServiceImageServiceLookupTableModalComponent,
        ServicePricesComponent,
        ViewServicePriceModalComponent,
        CreateOrEditServicePriceModalComponent,
        ServiceCategoriesComponent,
        ViewServiceCategoryModalComponent,
        CreateOrEditServiceCategoryModalComponent,
        ServiceCategoryServiceLookupTableModalComponent,
        ServiceCategoryCategoryLookupTableModalComponent,
        ServicesComponent,
        ViewServiceModalComponent,
        CreateOrEditServiceModalComponent,
        ServiceCategoryLookupTableModalComponent,
        ServiceSysRefLookupTableModalComponent,
        ServicesComponent,
        ViewServiceModalComponent,
        CreateOrEditServiceModalComponent,
        ProductPricesComponent,
        ViewProductPriceModalComponent,
        CreateOrEditProductPriceModalComponent,
        ProductPriceProductLookupTableModalComponent,
        ProductCategoriesComponent,
        ViewProductCategoryModalComponent,
        CreateOrEditProductCategoryModalComponent,
        ProductCategoryProductLookupTableModalComponent,
        ProductCategoryCategoryLookupTableModalComponent,
        ProductsComponent,
        ViewProductModalComponent,
        CreateOrEditProductModalComponent,
        ProductImagesComponent,
        ViewProductImageModalComponent,
        CreateOrEditProductImageModalComponent,
        ProductImageProductLookupTableModalComponent,
        ProductsComponent,
        ViewProductModalComponent,
        CreateOrEditProductModalComponent,
        ProductCategoryLookupTableModalComponent,
        DashboardComponent,
        PhotoEditorComponent,
        ServicePricesListingComponent,
        ProductPricesListingComponent,
        ApprovalListComponent
    ],
    providers: [
        { provide: BsDatepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerConfig },
        { provide: BsDaterangepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDaterangepickerConfig },
        { provide: BsLocaleService, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerLocale }
    ]
})
export class MainModule { }
