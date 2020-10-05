import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetSysStatusForViewDto, SysStatusDto } from '@shared/service-proxies/service-custom-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewSysStatusModal',
    templateUrl: './view-sysStatus-modal.component.html'
})
export class ViewSysStatusModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetSysStatusForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetSysStatusForViewDto();
        this.item.sysStatus = new SysStatusDto();
    }

    show(item: GetSysStatusForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
