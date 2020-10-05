import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetServiceImageForViewDto, ServiceImageDto } from '@shared/service-proxies/service-custom-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewServiceImageModal',
    templateUrl: './view-serviceImage-modal.component.html'
})
export class ViewServiceImageModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetServiceImageForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetServiceImageForViewDto();
        this.item.serviceImage = new ServiceImageDto();
    }

    show(item: GetServiceImageForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
