import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductImagesCustomServiceProxy, ProductImageDto  } from '@shared/service-proxies/service-custom-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditProductImageModalComponent } from './create-or-edit-productImage-modal.component';
import { ViewProductImageModalComponent } from './view-productImage-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './productImages.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ProductImagesComponent extends AppComponentBase {

    @ViewChild('createOrEditProductImageModal', { static: true }) createOrEditProductImageModal: CreateOrEditProductImageModalComponent;
    @ViewChild('viewProductImageModalComponent', { static: true }) viewProductImageModal: ViewProductImageModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    descriptionFilter = '';
    urlFilter = '';
    isMainFilter = -1;
    isApprovedFilter = -1;
    productNameFilter = '';

    constructor(
        injector: Injector,
        private _productImagesServiceProxy: ProductImagesCustomServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getProductImages(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._productImagesServiceProxy.getAll(
            this.filterText,
            this.descriptionFilter,
            this.urlFilter,
            this.isMainFilter,
            this.isApprovedFilter,
            this.productNameFilter,
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

    createProductImage(): void {
        this.createOrEditProductImageModal.show();
    }

    deleteProductImage(productImage: ProductImageDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._productImagesServiceProxy.delete(productImage.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._productImagesServiceProxy.getProductImagesToExcel(
        this.filterText,
            this.descriptionFilter,
            this.urlFilter,
            this.isMainFilter,
            this.isApprovedFilter,
            this.productNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
