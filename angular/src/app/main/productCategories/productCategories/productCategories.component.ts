﻿import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { ProductCategoriesCustomServiceProxy, ProductCategoryDto } from '@shared/service-proxies/service-custom-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CreateOrEditProductCategoryModalComponent } from './create-or-edit-productCategory-modal.component';
import { ViewProductCategoryModalComponent } from './view-productCategory-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './productCategories.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ProductCategoriesComponent extends AppComponentBase {

    @ViewChild('createOrEditProductCategoryModal', { static: true }) createOrEditProductCategoryModal: CreateOrEditProductCategoryModalComponent;
    @ViewChild('viewProductCategoryModalComponent', { static: true }) viewProductCategoryModal: ViewProductCategoryModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
        productNameFilter = '';
        categoryNameFilter = '';

    constructor(
        injector: Injector,
        private _productCategoriesServiceProxy: ProductCategoriesCustomServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService    ) {
        super(injector);
    
    }

    getProductCategories(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._productCategoriesServiceProxy.getAll(
            this.filterText,
            this.productNameFilter,
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

    createProductCategory(): void {
        this.createOrEditProductCategoryModal.show();
    }

    deleteProductCategory(productCategory: ProductCategoryDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._productCategoriesServiceProxy.delete(productCategory.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

}
