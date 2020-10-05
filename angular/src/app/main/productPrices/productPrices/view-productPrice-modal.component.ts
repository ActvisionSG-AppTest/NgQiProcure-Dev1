import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetProductPriceForViewDto, ProductPriceDto } from '@shared/service-proxies/service-custom-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewProductPriceModal',
    templateUrl: './view-productPrice-modal.component.html'
})
export class ViewProductPriceModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetProductPriceForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetProductPriceForViewDto();
        this.item.productPrice = new ProductPriceDto();
    }

    show(item: GetProductPriceForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
