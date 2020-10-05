import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ServicesCustomServiceProxy, ServiceDto  } from '@shared/service-proxies/service-custom-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditServiceModalComponent } from './create-or-edit-service-modal.component';
import { ViewServiceModalComponent } from './view-service-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './services.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ServicesComponent extends AppComponentBase {

    @ViewChild('createOrEditServiceModal', { static: true }) createOrEditServiceModal: CreateOrEditServiceModalComponent;
    @ViewChild('viewServiceModalComponent', { static: true }) viewServiceModal: ViewServiceModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    nameFilter = '';
    descriptionFilter = '';
    maxDurationFilter : number;
		maxDurationFilterEmpty : number;
		minDurationFilter : number;
		minDurationFilterEmpty : number;
    isApprovedFilter = -1;
    isActiveFilter = -1;
    remarkFilter = '';
        categoryNameFilter = '';
        sysRefRefCodeFilter = '';




    constructor(
        injector: Injector,
        private _servicesServiceProxy: ServicesCustomServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getServices(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._servicesServiceProxy.getAll(
            this.filterText,
            this.nameFilter,
            this.descriptionFilter,
            this.maxDurationFilter == null ? this.maxDurationFilterEmpty: this.maxDurationFilter,
            this.minDurationFilter == null ? this.minDurationFilterEmpty: this.minDurationFilter,
            this.isApprovedFilter,
            this.isActiveFilter,
            this.remarkFilter,
            this.categoryNameFilter,
            this.sysRefRefCodeFilter,
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

    createService(): void {
        this.createOrEditServiceModal.show();
    }

    deleteService(service: ServiceDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._servicesServiceProxy.delete(service.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._servicesServiceProxy.getServicesToExcel(
        this.filterText,
            this.nameFilter,
            this.descriptionFilter,
            this.maxDurationFilter == null ? this.maxDurationFilterEmpty: this.maxDurationFilter,
            this.minDurationFilter == null ? this.minDurationFilterEmpty: this.minDurationFilter,
            this.isApprovedFilter,
            this.isActiveFilter,
            this.remarkFilter,
            this.categoryNameFilter,
            this.sysRefRefCodeFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
