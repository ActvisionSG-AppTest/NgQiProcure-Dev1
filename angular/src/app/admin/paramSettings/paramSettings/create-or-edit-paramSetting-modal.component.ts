import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ParamSettingsCustomServiceProxy, CreateOrEditParamSettingDto } from '@shared/service-proxies/service-custom-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';

@Component({
    selector: 'createOrEditParamSettingModal',
    templateUrl: './create-or-edit-paramSetting-modal.component.html'
})
export class CreateOrEditParamSettingModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    paramSetting: CreateOrEditParamSettingDto = new CreateOrEditParamSettingDto();

    constructor(
        injector: Injector,
        private _paramSettingsServiceProxy: ParamSettingsCustomServiceProxy
    ) {
        super(injector);
    }

    show(paramSettingId?: number): void {

        if (!paramSettingId) {
            this.paramSetting = new CreateOrEditParamSettingDto();
            this.paramSetting.id = paramSettingId;

            this.active = true;
            this.modal.show();
        } else {
            this._paramSettingsServiceProxy.getParamSettingForEdit(paramSettingId).subscribe(result => {
                this.paramSetting = result.paramSetting;


                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
            this._paramSettingsServiceProxy.createOrEdit(this.paramSetting)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }







    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
