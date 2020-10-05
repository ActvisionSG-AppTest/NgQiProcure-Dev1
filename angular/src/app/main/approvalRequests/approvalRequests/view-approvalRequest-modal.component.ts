import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetApprovalRequestForViewDto, ApprovalRequestDto } from '@shared/service-proxies/service-custom-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewApprovalRequestModal',
    templateUrl: './view-approvalRequest-modal.component.html'
})
export class ViewApprovalRequestModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetApprovalRequestForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetApprovalRequestForViewDto();
        this.item.approvalRequest = new ApprovalRequestDto();
    }

    show(item: GetApprovalRequestForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
