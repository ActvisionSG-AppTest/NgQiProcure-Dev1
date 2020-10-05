import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { EmailsCustomServiceProxy, EmailDto  } from '@shared/service-proxies/service-custom-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditEmailModalComponent } from './create-or-edit-email-modal.component';
import { ViewEmailModalComponent } from './view-email-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './emails.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class EmailsComponent extends AppComponentBase {

    @ViewChild('createOrEditEmailModal', { static: true }) createOrEditEmailModal: CreateOrEditEmailModalComponent;
    @ViewChild('viewEmailModalComponent', { static: true }) viewEmailModal: ViewEmailModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxReferenceIdFilter : number;
		maxReferenceIdFilterEmpty : number;
		minReferenceIdFilter : number;
		minReferenceIdFilterEmpty : number;
    emailFromFilter = '';
    emailToFilter = '';
    emailCCFilter = '';
    emailBCCFilter = '';
    subjectFilter = '';
    bodyFilter = '';
    maxRequestDateFilter : moment.Moment;
		minRequestDateFilter : moment.Moment;
    maxSentDateFilter : moment.Moment;
		minSentDateFilter : moment.Moment;
        sysRefTenantIdFilter = '';
        sysStatusNameFilter = '';




    constructor(
        injector: Injector,
        private _emailsServiceProxy: EmailsCustomServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getEmails(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._emailsServiceProxy.getAll(
            this.filterText,
            this.maxReferenceIdFilter == null ? this.maxReferenceIdFilterEmpty: this.maxReferenceIdFilter,
            this.minReferenceIdFilter == null ? this.minReferenceIdFilterEmpty: this.minReferenceIdFilter,
            this.emailFromFilter,
            this.emailToFilter,
            this.emailCCFilter,
            this.emailBCCFilter,
            this.subjectFilter,
            this.bodyFilter,
            this.maxRequestDateFilter,
            this.minRequestDateFilter,
            this.maxSentDateFilter,
            this.minSentDateFilter,
            this.sysRefTenantIdFilter,
            this.sysStatusNameFilter,
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

    createEmail(): void {
        this.createOrEditEmailModal.show();
    }

    deleteEmail(email: EmailDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._emailsServiceProxy.delete(email.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._emailsServiceProxy.getEmailsToExcel(
        this.filterText,
            this.maxReferenceIdFilter == null ? this.maxReferenceIdFilterEmpty: this.maxReferenceIdFilter,
            this.minReferenceIdFilter == null ? this.minReferenceIdFilterEmpty: this.minReferenceIdFilter,
            this.emailFromFilter,
            this.emailToFilter,
            this.emailCCFilter,
            this.emailBCCFilter,
            this.subjectFilter,
            this.bodyFilter,
            this.maxRequestDateFilter,
            this.minRequestDateFilter,
            this.maxSentDateFilter,
            this.minSentDateFilter,
            this.sysRefTenantIdFilter,
            this.sysStatusNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
