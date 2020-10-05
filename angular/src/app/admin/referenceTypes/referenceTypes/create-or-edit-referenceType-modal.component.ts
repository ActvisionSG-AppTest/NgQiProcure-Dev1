import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ReferenceTypesCustomServiceProxy, CreateOrEditReferenceTypeDto } from '@shared/service-proxies/service-custom-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';

@Component({
    selector: 'createOrEditReferenceTypeModal',
    templateUrl: './create-or-edit-referenceType-modal.component.html'
})
export class CreateOrEditReferenceTypeModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    referenceType: CreateOrEditReferenceTypeDto = new CreateOrEditReferenceTypeDto();



    constructor(
        injector: Injector,
        private _referenceTypesServiceProxy: ReferenceTypesCustomServiceProxy
    ) {
        super(injector);
    }

    show(referenceTypeId?: number): void {

        if (!referenceTypeId) {
            this.referenceType = new CreateOrEditReferenceTypeDto();
            this.referenceType.id = referenceTypeId;

            this.active = true;
            this.modal.show();
        } else {
            this._referenceTypesServiceProxy.getReferenceTypeForEdit(referenceTypeId).subscribe(result => {
                this.referenceType = result.referenceType;


                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;

			
            this._referenceTypesServiceProxy.createOrEdit(this.referenceType)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }







    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
