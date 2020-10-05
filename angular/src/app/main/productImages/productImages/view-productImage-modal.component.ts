﻿import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetProductImageForViewDto, ProductImageDto } from '@shared/service-proxies/service-custom-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewProductImageModal',
    templateUrl: './view-productImage-modal.component.html'
})
export class ViewProductImageModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetProductImageForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetProductImageForViewDto();
        this.item.productImage = new ProductImageDto();
    }

    show(item: GetProductImageForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
