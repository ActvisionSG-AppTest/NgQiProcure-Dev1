import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DocumentsCustomServiceProxy, DocumentDto  } from '@shared/service-proxies/service-custom-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditDocumentModalComponent } from './create-or-edit-document-modal.component';
import { ViewDocumentModalComponent } from './view-document-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './documents.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class DocumentsComponent extends AppComponentBase {

    @ViewChild('createOrEditDocumentModal', { static: true }) createOrEditDocumentModal: CreateOrEditDocumentModalComponent;
    @ViewChild('viewDocumentModalComponent', { static: true }) viewDocumentModal: ViewDocumentModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    urlFilter = '';
    nameFilter = '';
    descriptionFilter = '';
        sysRefTenantIdFilter = '';
        productNameFilter = '';
        serviceNameFilter = '';




    constructor(
        injector: Injector,
        private _documentsServiceProxy: DocumentsCustomServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getDocuments(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._documentsServiceProxy.getAll(
            this.filterText,
            this.urlFilter,
            this.nameFilter,
            this.descriptionFilter,
            this.sysRefTenantIdFilter,
            this.productNameFilter,
            this.serviceNameFilter,
            0,
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

    createDocument(): void {
        this.createOrEditDocumentModal.show();
    }

    deleteDocument(document: DocumentDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._documentsServiceProxy.delete(document.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._documentsServiceProxy.getDocumentsToExcel(
        this.filterText,
            this.urlFilter,
            this.nameFilter,
            this.descriptionFilter,
            this.sysRefTenantIdFilter,
            this.productNameFilter,
            this.serviceNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
