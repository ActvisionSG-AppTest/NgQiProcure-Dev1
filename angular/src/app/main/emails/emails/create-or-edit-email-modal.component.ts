import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { EmailsCustomServiceProxy, CreateOrEditEmailDto } from '@shared/service-proxies/service-custom-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { EmailSysRefLookupTableModalComponent } from './email-sysRef-lookup-table-modal.component';
import { EmailSysStatusLookupTableModalComponent } from './email-sysStatus-lookup-table-modal.component';

@Component({
    selector: 'createOrEditEmailModal',
    templateUrl: './create-or-edit-email-modal.component.html'
})
export class CreateOrEditEmailModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('emailSysRefLookupTableModal', { static: true }) emailSysRefLookupTableModal: EmailSysRefLookupTableModalComponent;
    @ViewChild('emailSysStatusLookupTableModal', { static: true }) emailSysStatusLookupTableModal: EmailSysStatusLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    email: CreateOrEditEmailDto = new CreateOrEditEmailDto();

    sysRefTenantId = '';
    sysStatusName = '';


    constructor(
        injector: Injector,
        private _emailsServiceProxy: EmailsCustomServiceProxy
    ) {
        super(injector);
    }

    show(emailId?: number): void {

        if (!emailId) {
            this.email = new CreateOrEditEmailDto();
            this.email.id = emailId;
            this.email.requestDate = moment().startOf('day');
            this.email.sentDate = moment().startOf('day');
            this.sysRefTenantId = '';
            this.sysStatusName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._emailsServiceProxy.getEmailForEdit(emailId).subscribe(result => {
                this.email = result.email;

                this.sysRefTenantId = result.sysRefTenantId;
                this.sysStatusName = result.sysStatusName;

                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
            this._emailsServiceProxy.createOrEdit(this.email)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectSysRefModal() {
        this.emailSysRefLookupTableModal.id = this.email.sysRefId;
        this.emailSysRefLookupTableModal.displayName = this.sysRefTenantId;
        this.emailSysRefLookupTableModal.show();
    }
    openSelectSysStatusModal() {
        this.emailSysStatusLookupTableModal.id = this.email.sysStatusId;
        this.emailSysStatusLookupTableModal.displayName = this.sysStatusName;
        this.emailSysStatusLookupTableModal.show();
    }


    setSysRefIdNull() {
        this.email.sysRefId = null;
        this.sysRefTenantId = '';
    }
    setSysStatusIdNull() {
        this.email.sysStatusId = null;
        this.sysStatusName = '';
    }


    getNewSysRefId() {
        this.email.sysRefId = this.emailSysRefLookupTableModal.id;
        this.sysRefTenantId = this.emailSysRefLookupTableModal.displayName;
    }
    getNewSysStatusId() {
        this.email.sysStatusId = this.emailSysStatusLookupTableModal.id;
        this.sysStatusName = this.emailSysStatusLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
