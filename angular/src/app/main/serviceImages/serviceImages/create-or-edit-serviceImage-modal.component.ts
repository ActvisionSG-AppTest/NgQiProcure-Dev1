import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ServiceImagesCustomServiceProxy, CreateOrEditServiceImageDto } from '@shared/service-proxies/service-custom-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { ServiceImageServiceLookupTableModalComponent } from './serviceImage-service-lookup-table-modal.component';

@Component({
    selector: 'createOrEditServiceImageModal',
    templateUrl: './create-or-edit-serviceImage-modal.component.html'
})
export class CreateOrEditServiceImageModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('serviceImageServiceLookupTableModal', { static: true }) serviceImageServiceLookupTableModal: ServiceImageServiceLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    serviceImage: CreateOrEditServiceImageDto = new CreateOrEditServiceImageDto();

    serviceName = '';


    constructor(
        injector: Injector,
        private _serviceImagesServiceProxy: ServiceImagesCustomServiceProxy
    ) {
        super(injector);
    }

    show(serviceImageId?: number): void {

        if (!serviceImageId) {
            this.serviceImage = new CreateOrEditServiceImageDto();
            this.serviceImage.id = serviceImageId;
            this.serviceName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._serviceImagesServiceProxy.getServiceImageForEdit(serviceImageId).subscribe(result => {
                this.serviceImage = result.serviceImage;

                this.serviceName = result.serviceName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;

			
            this._serviceImagesServiceProxy.createOrEdit(this.serviceImage)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectServiceModal() {
        this.serviceImageServiceLookupTableModal.id = this.serviceImage.serviceId;
        this.serviceImageServiceLookupTableModal.displayName = this.serviceName;
        this.serviceImageServiceLookupTableModal.show();
    }


    setServiceIdNull() {
        this.serviceImage.serviceId = null;
        this.serviceName = '';
    }


    getNewServiceId() {
        this.serviceImage.serviceId = this.serviceImageServiceLookupTableModal.id;
        this.serviceName = this.serviceImageServiceLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
