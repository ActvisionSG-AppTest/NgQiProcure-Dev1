import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetReferenceTypeForViewDto, ReferenceTypeDto } from '@shared/service-proxies/service-custom-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewReferenceTypeModal',
    templateUrl: './view-referenceType-modal.component.html'
})
export class ViewReferenceTypeModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetReferenceTypeForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetReferenceTypeForViewDto();
        this.item.referenceType = new ReferenceTypeDto();
    }

    show(item: GetReferenceTypeForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
