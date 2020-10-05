import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductPricesCustomServiceProxy, ProductPriceDto  } from '@shared/service-proxies/service-custom-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditProductPriceModalComponent } from './create-or-edit-productPrice-modal.component';
import { ViewProductPriceModalComponent } from './view-productPrice-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { EntityTypeHistoryModalComponent } from '@app/shared/common/entityHistory/entity-type-history-modal.component';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    selector: 'ProductPrices',
    templateUrl: './productPrices.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ProductPricesComponent extends AppComponentBase {

    @ViewChild('createOrEditProductPriceModal', { static: true }) createOrEditProductPriceModal: CreateOrEditProductPriceModalComponent;
    @ViewChild('viewProductPriceModalComponent', { static: true }) viewProductPriceModal: ViewProductPriceModalComponent;
    @ViewChild('entityTypeHistoryModal', { static: true }) entityTypeHistoryModal: EntityTypeHistoryModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxPriceFilter : number;
		maxPriceFilterEmpty : number;
		minPriceFilter : number;
		minPriceFilterEmpty : number;
    maxvalidityFilter : moment.Moment;
		minvalidityFilter : moment.Moment;
        productNameFilter = '';


    _entityTypeFullName = 'QiProcureDemo.ProductPrices.ProductPrice';
    entityHistoryEnabled = false;

    constructor(
        injector: Injector,
        private _productPricesServiceProxy: ProductPricesCustomServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.entityHistoryEnabled = this.setIsEntityHistoryEnabled();
    }

    private setIsEntityHistoryEnabled(): boolean {
        let customSettings = (abp as any).custom;
        return customSettings.EntityHistory && customSettings.EntityHistory.isEnabled && _.filter(customSettings.EntityHistory.enabledEntities, entityType => entityType === this._entityTypeFullName).length === 1;
    }

    getProductPrices(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._productPricesServiceProxy.getAll(
            this.filterText,
            this.maxPriceFilter == null ? this.maxPriceFilterEmpty: this.maxPriceFilter,
            this.minPriceFilter == null ? this.minPriceFilterEmpty: this.minPriceFilter,
            this.maxvalidityFilter,
            this.minvalidityFilter,
            this.productNameFilter,
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

    createProductPrice(): void {
        this.createOrEditProductPriceModal.show();
    }

    showHistory(productPrice: ProductPriceDto): void {
        this.entityTypeHistoryModal.show({
            entityId: productPrice.id.toString(),
            entityTypeFullName: this._entityTypeFullName,
            entityTypeDescription: ''
        });
    }

    deleteProductPrice(productPrice: ProductPriceDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._productPricesServiceProxy.delete(productPrice.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._productPricesServiceProxy.getProductPricesToExcel(
        this.filterText,
            this.maxPriceFilter == null ? this.maxPriceFilterEmpty: this.maxPriceFilter,
            this.minPriceFilter == null ? this.minPriceFilterEmpty: this.minPriceFilter,
            this.maxvalidityFilter,
            this.minvalidityFilter,
            this.productNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
