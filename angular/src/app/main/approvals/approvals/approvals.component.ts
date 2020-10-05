import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ApprovalsCustomServiceProxy, ApprovalDto  } from '@shared/service-proxies/service-custom-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditApprovalModalComponent } from './create-or-edit-approval-modal.component';
import { ViewApprovalModalComponent } from './view-approval-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './approvals.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ApprovalsComponent extends AppComponentBase {

    @ViewChild('createOrEditApprovalModal', { static: true }) createOrEditApprovalModal: CreateOrEditApprovalModalComponent;
    @ViewChild('viewApprovalModalComponent', { static: true }) viewApprovalModal: ViewApprovalModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxRankNoFilter : number;
		maxRankNoFilterEmpty : number;
		minRankNoFilter : number;
		minRankNoFilterEmpty : number;
        maxAmountFilter : number;
		maxAmountFilterEmpty : number;
		minAmountFilter : number;
		minAmountFilterEmpty : number;
        sysRefTenantIdFilter = '';
        teamNameFilter = '';
        projectNameFilter = '';
        accountNameFilter = '';
        userNameFilter = '';
        sysStatusNameFilter = '';
    constructor(
        injector: Injector,
        private _approvalsCustomServiceProxy: ApprovalsCustomServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getApprovals(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._approvalsCustomServiceProxy.getAll(
            this.filterText,
            this.maxRankNoFilter == null ? this.maxRankNoFilterEmpty: this.maxRankNoFilter,
            this.minRankNoFilter == null ? this.minRankNoFilterEmpty: this.minRankNoFilter,
            this.maxAmountFilter == null ? this.maxAmountFilterEmpty: this.maxAmountFilter,
            this.minAmountFilter == null ? this.minAmountFilterEmpty: this.minAmountFilter,
            this.sysRefTenantIdFilter,
            this.teamNameFilter,
            this.projectNameFilter,
            this.accountNameFilter,
            this.userNameFilter,
            this.sysStatusNameFilter,
            undefined,
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

    createApproval(): void {
        this.createOrEditApprovalModal.show();
    }

    deleteApproval(approval: ApprovalDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._approvalsCustomServiceProxy.delete(approval.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._approvalsCustomServiceProxy.getApprovalsToExcel(
        this.filterText,
            this.maxRankNoFilter == null ? this.maxRankNoFilterEmpty: this.maxRankNoFilter,
            this.minRankNoFilter == null ? this.minRankNoFilterEmpty: this.minRankNoFilter,
            this.maxAmountFilter == null ? this.maxAmountFilterEmpty: this.maxAmountFilter,
            this.minAmountFilter == null ? this.minAmountFilterEmpty: this.minAmountFilter,
            this.sysRefTenantIdFilter,
            this.teamNameFilter,
            this.projectNameFilter,
            this.accountNameFilter,
            this.userNameFilter,
            this.sysStatusNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
