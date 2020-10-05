import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SysRefsCustomServiceProxy, SysRefDto  } from '@shared/service-proxies/service-custom-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditSysRefModalComponent } from './create-or-edit-sysRef-modal.component';
import { ViewSysRefModalComponent } from './view-sysRef-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './sysRefs.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class SysRefsComponent extends AppComponentBase {

    @ViewChild('createOrEditSysRefModal', { static: true }) createOrEditSysRefModal: CreateOrEditSysRefModalComponent;
    @ViewChild('viewSysRefModalComponent', { static: true }) viewSysRefModal: ViewSysRefModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    refCodeFilter = '';
    descriptionFilter = '';
    referenceTypeNameFilter = '';
    maxOrderNumberFilter:number; minOrderNumberFilter:number

    constructor(
        injector: Injector,
        private _sysRefsServiceProxy: SysRefsCustomServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getSysRefs(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._sysRefsServiceProxy.getCustomAll(
            this.filterText,
            this.refCodeFilter,
            this.descriptionFilter,      
            this.referenceTypeNameFilter,
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

    createSysRef(): void {
        this.createOrEditSysRefModal.show();
    }

    deleteSysRef(sysRef: SysRefDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._sysRefsServiceProxy.delete(sysRef.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._sysRefsServiceProxy.getSysRefsToExcel(
        this.filterText,
            this.refCodeFilter,
            this.descriptionFilter,
            this.maxOrderNumberFilter,
            this.minOrderNumberFilter,
            this.referenceTypeNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
