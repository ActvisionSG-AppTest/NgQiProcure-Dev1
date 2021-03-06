﻿import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetAccountForViewDto, AccountDto } from '@shared/service-proxies/service-custom-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewAccountModal',
    templateUrl: './view-account-modal.component.html'
})
export class ViewAccountModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetAccountForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetAccountForViewDto();
        this.item.account = new AccountDto();
    }

    show(item: GetAccountForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
