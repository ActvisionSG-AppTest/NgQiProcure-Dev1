﻿import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductsCustomServiceProxy, ProductDto  } from '@shared/service-proxies/service-custom-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditProductModalComponent } from './create-or-edit-product-modal.component';

import { ViewProductModalComponent } from './view-product-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './products.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ProductsComponent extends AppComponentBase {

    @ViewChild('createOrEditProductModal', { static: true }) createOrEditProductModal: CreateOrEditProductModalComponent;
    @ViewChild('viewProductModalComponent', { static: true }) viewProductModal: ViewProductModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    nameFilter = '';
    descriptionFilter = '';
    maxStockFilter : number;
	maxStockFilterEmpty : number;
	minStockFilter : number;
	minStockFilterEmpty : number;
    uomFilter = '';
    isApprovedFilter = -1;
    isActiveFilter = -1;
    categoryNameFilter = '';

    constructor(
        injector: Injector,
        private _productsServiceProxy: ProductsCustomServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
    ) {
        super(injector);
    }

    getProducts(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._productsServiceProxy.getAll(
            this.filterText,
            this.nameFilter,
            this.descriptionFilter,
            this.maxStockFilter == null ? this.maxStockFilterEmpty: this.maxStockFilter,
            this.minStockFilter == null ? this.minStockFilterEmpty: this.minStockFilter,
            this.uomFilter,
            this.isApprovedFilter,
            this.isActiveFilter,
            this.categoryNameFilter,
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

    createProduct(): void {
        this.createOrEditProductModal.show();
    }

    deleteProduct(product: ProductDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._productsServiceProxy.delete(product.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._productsServiceProxy.getProductsToExcel(
        this.filterText,
            this.nameFilter,
            this.descriptionFilter,
            this.maxStockFilter == null ? this.maxStockFilterEmpty: this.maxStockFilter,
            this.minStockFilter == null ? this.minStockFilterEmpty: this.minStockFilter,
            this.uomFilter,
            this.isApprovedFilter,
            this.isActiveFilter,
            this.categoryNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
