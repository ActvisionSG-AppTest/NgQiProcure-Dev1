import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ServiceImagesCustomServiceProxy, ServiceImageDto  } from '@shared/service-proxies/service-custom-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditServiceImageModalComponent } from './create-or-edit-serviceImage-modal.component';
import { ViewServiceImageModalComponent } from './view-serviceImage-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './serviceImages.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ServiceImagesComponent extends AppComponentBase {

    @ViewChild('createOrEditServiceImageModal', { static: true }) createOrEditServiceImageModal: CreateOrEditServiceImageModalComponent;
    @ViewChild('viewServiceImageModalComponent', { static: true }) viewServiceImageModal: ViewServiceImageModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    descriptionFilter = '';
    urlFilter = '';
    isMainFilter = -1;
    isApprovedFilter = -1;
    maxBytesFilter : number;
		maxBytesFilterEmpty : number;
		minBytesFilter : number;
		minBytesFilterEmpty : number;
        serviceNameFilter = '';




    constructor(
        injector: Injector,
        private _serviceImagesServiceProxy: ServiceImagesCustomServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getServiceImages(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._serviceImagesServiceProxy.getAll(
            this.filterText,
            this.descriptionFilter,
            this.urlFilter,
            this.isMainFilter,
            this.isApprovedFilter,
            this.serviceNameFilter,
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

    createServiceImage(): void {
        this.createOrEditServiceImageModal.show();
    }

    deleteServiceImage(serviceImage: ServiceImageDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._serviceImagesServiceProxy.delete(serviceImage.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._serviceImagesServiceProxy.getServiceImagesToExcel(
        this.filterText,
            this.descriptionFilter,
            this.urlFilter,
            this.isMainFilter,
            this.isApprovedFilter,
            this.serviceNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
