import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetServicePriceForViewDto, ServicePriceDto } from '@shared/service-proxies/service-custom-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewServicePriceModal',
    templateUrl: './view-servicePrice-modal.component.html'
})
export class ViewServicePriceModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetServicePriceForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetServicePriceForViewDto();
        this.item.servicePrice = new ServicePriceDto();
    }

    show(item: GetServicePriceForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
