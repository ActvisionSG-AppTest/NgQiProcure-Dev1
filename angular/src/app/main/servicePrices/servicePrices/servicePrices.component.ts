import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ServicePricesCustomServiceProxy, ServicePriceDto  } from '@shared/service-proxies/service-custom-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditServicePriceModalComponent } from './create-or-edit-servicePrice-modal.component';
import { ViewServicePriceModalComponent } from './view-servicePrice-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './servicePrices.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ServicePricesComponent extends AppComponentBase {

    @ViewChild('createOrEditServicePriceModal', { static: true }) createOrEditServicePriceModal: CreateOrEditServicePriceModalComponent;
    @ViewChild('viewServicePriceModalComponent', { static: true }) viewServicePriceModal: ViewServicePriceModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxPriceFilter : number;
	maxPriceFilterEmpty : number;
	minPriceFilter : number;
	minPriceFilterEmpty : number;
    maxValidityFilter : moment.Moment;
	minValidityFilter : moment.Moment;
    serviceNameFilter = '';

    constructor(
        injector: Injector,
        private _servicePricesServiceProxy: ServicePricesCustomServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getServicePrices(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._servicePricesServiceProxy.getAll(
            this.filterText,
            this.maxPriceFilter == null ? this.maxPriceFilterEmpty: this.maxPriceFilter,
            this.minPriceFilter == null ? this.minPriceFilterEmpty: this.minPriceFilter,
            this.maxValidityFilter,
            this.minValidityFilter,
            this.serviceNameFilter,
            0,
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

    createServicePrice(): void {
        this.createOrEditServicePriceModal.show();
    }

    deleteServicePrice(servicePrice: ServicePriceDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._servicePricesServiceProxy.delete(servicePrice.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._servicePricesServiceProxy.getServicePricesToExcel(
        this.filterText,
            this.maxPriceFilter == null ? this.maxPriceFilterEmpty: this.maxPriceFilter,
            this.minPriceFilter == null ? this.minPriceFilterEmpty: this.minPriceFilter,
            this.maxValidityFilter,
            this.minValidityFilter,
            this.serviceNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
