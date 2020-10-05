import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetParamSettingForViewDto, ParamSettingDto } from '@shared/service-proxies/service-custom-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewParamSettingModal',
    templateUrl: './view-paramSetting-modal.component.html'
})
export class ViewParamSettingModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetParamSettingForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetParamSettingForViewDto();
        this.item.paramSetting = new ParamSettingDto();
    }

    show(item: GetParamSettingForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
