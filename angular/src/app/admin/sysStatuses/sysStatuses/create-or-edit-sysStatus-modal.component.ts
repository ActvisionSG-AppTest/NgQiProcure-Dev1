import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { SysStatusesCustomServiceProxy, CreateOrEditSysStatusDto, SysRefDto, SysRefsCustomServiceProxy } from '@shared/service-proxies/service-custom-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { SysStatusesSysRefLookupTableModalComponent } from './sysStatus-sysRef-lookup-table-modal.component';

@Component({
    selector: 'createOrEditSysStatusModal',
    templateUrl: './create-or-edit-sysStatus-modal.component.html'
})
export class CreateOrEditSysStatusModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('sysStatusesSysRefLookupTableModal', { static: true }) sysStatusesSysRefLookupTableModal: SysStatusesSysRefLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    sysStatus: CreateOrEditSysStatusDto = new CreateOrEditSysStatusDto();

    sysRefTenantId = '';
    sysRefName = "";

    constructor(
        injector: Injector,
        private _sysStatusesServiceProxy: SysStatusesCustomServiceProxy,
        private _sysRefsServiceProxy: SysRefsCustomServiceProxy
    ) {
        super(injector);
    }

    show(sysStatusId?: number): void {

        if (!sysStatusId) {
            this.sysStatus = new CreateOrEditSysStatusDto();
            this.sysStatus.id = sysStatusId;
            this.sysRefTenantId = '';

            this.active = true;
            this.modal.show();
        } else {
            this._sysStatusesServiceProxy.getSysStatusForEdit(sysStatusId).subscribe(result => {
                this.sysStatus = result.sysStatus;
                this.sysRefTenantId = result.sysRefTenantId;

                if (this.sysStatus.sysRefId != null){
                    this._sysRefsServiceProxy.getSysRefForEdit(this.sysStatus.sysRefId ).subscribe(result => {
                        this.sysRefTenantId = result.sysRef.refCode;
                    });
                }
                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

            //check for duplicate status of the same system reference
            
            if (this.sysStatus.id == null){
                


                
            }
            this._sysStatusesServiceProxy.createOrEdit(this.sysStatus)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectSysRefModal() {

        this.sysStatusesSysRefLookupTableModal.id = this.sysStatus.sysRefId;
        this.sysStatusesSysRefLookupTableModal.displayName = this.sysRefTenantId;
        this.sysStatusesSysRefLookupTableModal.showByRefType("Status");
    }


    setSysRefIdNull() {
        this.sysStatus.sysRefId = null;
        this.sysRefTenantId = '';
    }


    getNewSysRefId() {
        this.sysStatus.sysRefId = this.sysStatusesSysRefLookupTableModal.id;
        this.sysRefTenantId = this.sysStatusesSysRefLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
