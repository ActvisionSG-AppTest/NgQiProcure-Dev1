import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ReferenceTypesCustomServiceProxy, ReferenceTypeDto  } from '@shared/service-proxies/service-custom-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditReferenceTypeModalComponent } from './create-or-edit-referenceType-modal.component';
import { ViewReferenceTypeModalComponent } from './view-referenceType-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './referenceTypes.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ReferenceTypesComponent extends AppComponentBase {

    @ViewChild('createOrEditReferenceTypeModal', { static: true }) createOrEditReferenceTypeModal: CreateOrEditReferenceTypeModalComponent;
    @ViewChild('viewReferenceTypeModalComponent', { static: true }) viewReferenceTypeModal: ViewReferenceTypeModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    nameFilter = '';
    referenceTypeCodeFilter = '';
    referenceTypeGroupFilter = "";

    constructor(
        injector: Injector,
        private _referenceTypesServiceProxy: ReferenceTypesCustomServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getReferenceTypes(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._referenceTypesServiceProxy.getAll(
            this.filterText,
            this.nameFilter,
            this.referenceTypeCodeFilter,
            this.referenceTypeGroupFilter,
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

    createReferenceType(): void {
        this.createOrEditReferenceTypeModal.show();
    }

    deleteReferenceType(referenceType: ReferenceTypeDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._referenceTypesServiceProxy.delete(referenceType.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._referenceTypesServiceProxy.getReferenceTypesToExcel(
        this.filterText,
        this.nameFilter,
        this.referenceTypeCodeFilter,
        this.referenceTypeGroupFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
