import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetDocumentForViewDto, DocumentDto } from '@shared/service-proxies/service-custom-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewDocumentModal',
    templateUrl: './view-document-modal.component.html'
})
export class ViewDocumentModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetDocumentForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetDocumentForViewDto();
        this.item.document = new DocumentDto();
    }

    show(item: GetDocumentForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
