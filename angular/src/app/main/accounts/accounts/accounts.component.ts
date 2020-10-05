import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AccountsCustomServiceProxy, AccountDto  } from '@shared/service-proxies/service-custom-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditAccountModalComponent } from './create-or-edit-account-modal.component';
import { ViewAccountModalComponent } from './view-account-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './accounts.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class AccountsComponent extends AppComponentBase {

    @ViewChild('createOrEditAccountModal', { static: true }) createOrEditAccountModal: CreateOrEditAccountModalComponent;
    @ViewChild('viewAccountModalComponent', { static: true }) viewAccountModal: ViewAccountModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    nameFilter = '';
    descriptionFilter = '';
    isPersonalFilter = -1;
    isActiveFilter = -1;
    remarkFilter = '';
    codeFilter = '';
    emailFilter = '';
    userNameFilter = '';
    passwordFilter = '';
        teamNameFilter = '';
        sysStatusNameFilter = '';




    constructor(
        injector: Injector,
        private _accountsServiceProxy: AccountsCustomServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getAccounts(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._accountsServiceProxy.getAll(
            this.filterText,
            this.nameFilter,
            this.descriptionFilter,
            this.isPersonalFilter,
            this.isActiveFilter,
            this.remarkFilter,
            this.codeFilter,
            this.emailFilter,
            this.userNameFilter,
            this.passwordFilter,
            this.teamNameFilter,
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

    createAccount(): void {
        this.createOrEditAccountModal.show();
    }

    deleteAccount(account: AccountDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._accountsServiceProxy.delete(account.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._accountsServiceProxy.getAccountsToExcel(
        this.filterText,
            this.nameFilter,
            this.descriptionFilter,
            this.isPersonalFilter,
            this.isActiveFilter,
            this.remarkFilter,
            this.codeFilter,
            this.emailFilter,
            this.userNameFilter,
            this.passwordFilter,
            this.teamNameFilter,
            this.sysStatusNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
