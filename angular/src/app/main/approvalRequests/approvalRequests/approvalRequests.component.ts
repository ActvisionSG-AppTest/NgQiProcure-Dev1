import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ApprovalRequestsCustomServiceProxy, ApprovalRequestDto  } from '@shared/service-proxies/service-custom-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditApprovalRequestModalComponent } from './create-or-edit-approvalRequest-modal.component';
import { ViewApprovalRequestModalComponent } from './view-approvalRequest-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './approvalRequests.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ApprovalRequestsComponent extends AppComponentBase {

    @ViewChild('createOrEditApprovalRequestModal', { static: true }) createOrEditApprovalRequestModal: CreateOrEditApprovalRequestModalComponent;
    @ViewChild('viewApprovalRequestModalComponent', { static: true }) viewApprovalRequestModal: ViewApprovalRequestModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxReferenceIdFilter : number;
		maxReferenceIdFilterEmpty : number;
		minReferenceIdFilter : number;
		minReferenceIdFilterEmpty : number;
    maxOrderNoFilter : number;
		maxOrderNoFilterEmpty : number;
		minOrderNoFilter : number;
		minOrderNoFilterEmpty : number;
    maxRankNoFilter : number;
		maxRankNoFilterEmpty : number;
		minRankNoFilter : number;
		minRankNoFilterEmpty : number;
    maxAmountFilter : number;
		maxAmountFilterEmpty : number;
		minAmountFilter : number;
		minAmountFilterEmpty : number;
    remarkFilter = '';
        sysRefTenantIdFilter = '';
        sysStatusNameFilter = '';
        userNameFilter = '';




    constructor(
        injector: Injector,
        private _approvalRequestsServiceProxy: ApprovalRequestsCustomServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getApprovalRequests(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._approvalRequestsServiceProxy.getAll(
            this.filterText,
            this.maxReferenceIdFilter == null ? this.maxReferenceIdFilterEmpty: this.maxReferenceIdFilter,
            this.minReferenceIdFilter == null ? this.minReferenceIdFilterEmpty: this.minReferenceIdFilter,
            this.maxOrderNoFilter == null ? this.maxOrderNoFilterEmpty: this.maxOrderNoFilter,
            this.minOrderNoFilter == null ? this.minOrderNoFilterEmpty: this.minOrderNoFilter,
            this.maxRankNoFilter == null ? this.maxRankNoFilterEmpty: this.maxRankNoFilter,
            this.minRankNoFilter == null ? this.minRankNoFilterEmpty: this.minRankNoFilter,
            this.maxAmountFilter == null ? this.maxAmountFilterEmpty: this.maxAmountFilter,
            this.minAmountFilter == null ? this.minAmountFilterEmpty: this.minAmountFilter,
            this.remarkFilter,
            this.sysRefTenantIdFilter,
            this.sysStatusNameFilter,
            this.userNameFilter,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createApprovalRequest(): void {
        this.createOrEditApprovalRequestModal.show();
    }

    deleteApprovalRequest(approvalRequest: ApprovalRequestDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._approvalRequestsServiceProxy.delete(approvalRequest.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._approvalRequestsServiceProxy.getApprovalRequestsToExcel(
        this.filterText,
            this.maxReferenceIdFilter == null ? this.maxReferenceIdFilterEmpty: this.maxReferenceIdFilter,
            this.minReferenceIdFilter == null ? this.minReferenceIdFilterEmpty: this.minReferenceIdFilter,
            this.maxOrderNoFilter == null ? this.maxOrderNoFilterEmpty: this.maxOrderNoFilter,
            this.minOrderNoFilter == null ? this.minOrderNoFilterEmpty: this.minOrderNoFilter,
            this.maxRankNoFilter == null ? this.maxRankNoFilterEmpty: this.maxRankNoFilter,
            this.minRankNoFilter == null ? this.minRankNoFilterEmpty: this.minRankNoFilter,
            this.maxAmountFilter == null ? this.maxAmountFilterEmpty: this.maxAmountFilter,
            this.minAmountFilter == null ? this.minAmountFilterEmpty: this.minAmountFilter,
            this.remarkFilter,
            this.sysRefTenantIdFilter,
            this.sysStatusNameFilter,
            this.userNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
