import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetEmailForViewDto, EmailDto } from '@shared/service-proxies/service-custom-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewEmailModal',
    templateUrl: './view-email-modal.component.html'
})
export class ViewEmailModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetEmailForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetEmailForViewDto();
        this.item.email = new EmailDto();
    }

    show(item: GetEmailForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
