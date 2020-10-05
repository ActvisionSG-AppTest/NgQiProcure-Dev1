import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ServiceCategoriesCustomServiceProxy, ServiceCategoryDto  } from '@shared/service-proxies/service-custom-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditServiceCategoryModalComponent } from './create-or-edit-serviceCategory-modal.component';
import { ViewServiceCategoryModalComponent } from './view-serviceCategory-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './serviceCategories.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ServiceCategoriesComponent extends AppComponentBase {

    @ViewChild('createOrEditServiceCategoryModal', { static: true }) createOrEditServiceCategoryModal: CreateOrEditServiceCategoryModalComponent;
    @ViewChild('viewServiceCategoryModalComponent', { static: true }) viewServiceCategoryModal: ViewServiceCategoryModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
        serviceNameFilter = '';
        categoryNameFilter = '';




    constructor(
        injector: Injector,
        private _serviceCategoriesServiceProxy: ServiceCategoriesCustomServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getServiceCategories(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._serviceCategoriesServiceProxy.getAll(
            this.filterText,
            this.serviceNameFilter,
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

    createServiceCategory(): void {
        this.createOrEditServiceCategoryModal.show();
    }

    deleteServiceCategory(serviceCategory: ServiceCategoryDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._serviceCategoriesServiceProxy.delete(serviceCategory.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._serviceCategoriesServiceProxy.getServiceCategoriesToExcel(
        this.filterText,
            this.serviceNameFilter,
            this.categoryNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
