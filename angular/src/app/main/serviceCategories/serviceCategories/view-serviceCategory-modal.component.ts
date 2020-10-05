import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetServiceCategoryForViewDto, ServiceCategoryDto } from '@shared/service-proxies/service-custom-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewServiceCategoryModal',
    templateUrl: './view-serviceCategory-modal.component.html'
})
export class ViewServiceCategoryModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetServiceCategoryForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetServiceCategoryForViewDto();
        this.item.serviceCategory = new ServiceCategoryDto();
    }

    show(item: GetServiceCategoryForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
