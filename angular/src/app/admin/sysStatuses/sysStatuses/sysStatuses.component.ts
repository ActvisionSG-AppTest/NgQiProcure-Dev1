import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SysStatusesCustomServiceProxy, SysStatusDto  } from '@shared/service-proxies/service-custom-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditSysStatusModalComponent } from './create-or-edit-sysStatus-modal.component';
import { ViewSysStatusModalComponent } from './view-sysStatus-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './sysStatuses.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class SysStatusesComponent extends AppComponentBase {

    @ViewChild('createOrEditSysStatusModal', { static: true }) createOrEditSysStatusModal: CreateOrEditSysStatusModalComponent;
    @ViewChild('viewSysStatusModalComponent', { static: true }) viewSysStatusModal: ViewSysStatusModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxCodeFilter : number;
		maxCodeFilterEmpty : number;
		minCodeFilter : number;
		minCodeFilterEmpty : number;
    nameFilter = '';
    descriptionFilter = '';
        sysRefTenantIdFilter = '';




    constructor(
        injector: Injector,
        private _sysStatusesCustomServiceProxy: SysStatusesCustomServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getsysStatuses(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._sysStatusesCustomServiceProxy.getCustomAll(
            this.filterText,
            this.maxCodeFilter == null ? this.maxCodeFilterEmpty: this.maxCodeFilter,
            this.minCodeFilter == null ? this.minCodeFilterEmpty: this.minCodeFilter,
            this.nameFilter,
            this.descriptionFilter,
            this.sysRefTenantIdFilter,
            "",
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

    createSysStatus(): void {
        this.createOrEditSysStatusModal.show();
    }

    deleteSysStatus(sysStatus: SysStatusDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._sysStatusesCustomServiceProxy.delete(sysStatus.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._sysStatusesCustomServiceProxy.getSysStatusesToExcel(
        this.filterText,
            this.maxCodeFilter == null ? this.maxCodeFilterEmpty: this.maxCodeFilter,
            this.minCodeFilter == null ? this.minCodeFilterEmpty: this.minCodeFilter,
            this.nameFilter,
            this.descriptionFilter,
            this.sysRefTenantIdFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
