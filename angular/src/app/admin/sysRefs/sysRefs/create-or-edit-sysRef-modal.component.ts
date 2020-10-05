import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { SysRefsCustomServiceProxy, CreateOrEditSysRefDto } from '@shared/service-proxies/service-custom-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { SysRefReferenceTypeLookupTableModalComponent } from './sysRef-referenceType-lookup-table-modal.component';

@Component({
    selector: 'createOrEditSysRefModal',
    templateUrl: './create-or-edit-sysRef-modal.component.html'
})
export class CreateOrEditSysRefModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('sysRefReferenceTypeLookupTableModal', { static: true }) sysRefReferenceTypeLookupTableModal: SysRefReferenceTypeLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    sysRef: CreateOrEditSysRefDto = new CreateOrEditSysRefDto();

    referenceTypeName = '';
    refCode = '';

    description = '';

    constructor(
        injector: Injector,
        private _sysRefsServiceProxy: SysRefsCustomServiceProxy
    ) {
        super(injector);
    }

    show(sysRefId?: number): void {

        if (!sysRefId) {
            this.sysRef = new CreateOrEditSysRefDto();
            this.sysRef.id = sysRefId;
            this.referenceTypeName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._sysRefsServiceProxy.getSysRefForEdit(sysRefId).subscribe(result => {
                this.sysRef = result.sysRef;
                this.referenceTypeName = result.referenceTypeName;                
                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;

            this._sysRefsServiceProxy.createOrEdit(this.sysRef)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectReferenceTypeModal() {
        this.sysRefReferenceTypeLookupTableModal.id = this.sysRef.referenceTypeId;
        this.sysRefReferenceTypeLookupTableModal.displayName = this.referenceTypeName;
        this.sysRefReferenceTypeLookupTableModal.show();
    }


    setReferenceTypeIdNull() {
        this.sysRef.referenceTypeId = null;
        this.referenceTypeName = '';
    }


    getNewReferenceTypeId() {
        this.sysRef.referenceTypeId = this.sysRefReferenceTypeLookupTableModal.id;
        this.referenceTypeName = this.sysRefReferenceTypeLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
