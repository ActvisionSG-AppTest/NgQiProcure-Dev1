import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { AccountsCustomServiceProxy, CreateOrEditAccountDto } from '@shared/service-proxies/service-custom-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { AccountTeamLookupTableModalComponent } from './account-team-lookup-table-modal.component';
import { AccountSysStatusLookupTableModalComponent } from './account-sysStatus-lookup-table-modal.component';

@Component({
    selector: 'createOrEditAccountModal',
    templateUrl: './create-or-edit-account-modal.component.html'
})
export class CreateOrEditAccountModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('accountTeamLookupTableModal', { static: true }) accountTeamLookupTableModal: AccountTeamLookupTableModalComponent;
    @ViewChild('accountSysStatusLookupTableModal', { static: true }) accountSysStatusLookupTableModal: AccountSysStatusLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    account: CreateOrEditAccountDto = new CreateOrEditAccountDto();

    teamName = '';
    sysStatusName = '';


    constructor(
        injector: Injector,
        private _accountsServiceProxy: AccountsCustomServiceProxy
    ) {
        super(injector);
    }

    show(accountId?: number): void {

        if (!accountId) {
            this.account = new CreateOrEditAccountDto();
            this.account.id = accountId;
            this.teamName = '';
            this.sysStatusName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._accountsServiceProxy.getAccountForEdit(accountId).subscribe(result => {
                this.account = result.account;

                this.teamName = result.teamName;
                this.sysStatusName = result.sysStatusName;

                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
            this._accountsServiceProxy.createOrEdit(this.account)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectTeamModal() {
        this.accountTeamLookupTableModal.id = this.account.teamId;
        this.accountTeamLookupTableModal.displayName = this.teamName;
        this.accountTeamLookupTableModal.show();
    }
    openSelectSysStatusModal() {
        this.accountSysStatusLookupTableModal.id = this.account.sysStatusId;
        this.accountSysStatusLookupTableModal.displayName = this.sysStatusName;
        this.accountSysStatusLookupTableModal.show();
    }


    setTeamIdNull() {
        this.account.teamId = null;
        this.teamName = '';
    }
    setSysStatusIdNull() {
        this.account.sysStatusId = null;
        this.sysStatusName = '';
    }


    getNewTeamId() {
        this.account.teamId = this.accountTeamLookupTableModal.id;
        this.teamName = this.accountTeamLookupTableModal.displayName;
    }
    getNewSysStatusId() {
        this.account.sysStatusId = this.accountSysStatusLookupTableModal.id;
        this.sysStatusName = this.accountSysStatusLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
