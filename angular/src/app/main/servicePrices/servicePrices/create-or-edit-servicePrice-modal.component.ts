import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ServicePricesCustomServiceProxy, CreateOrEditServicePriceDto } from '@shared/service-proxies/service-custom-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { ServicePriceServiceLookupTableModalComponent } from './servicePrice-service-lookup-table-modal.component';

@Component({
    selector: 'createOrEditServicePriceModal',
    templateUrl: './create-or-edit-servicePrice-modal.component.html'
})
export class CreateOrEditServicePriceModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('servicePriceServiceLookupTableModal', { static: true }) servicePriceServiceLookupTableModal: ServicePriceServiceLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    servicePrice: CreateOrEditServicePriceDto = new CreateOrEditServicePriceDto();

    serviceName = '';

    constructor(
        injector: Injector,
        private _servicePricesServiceProxy: ServicePricesCustomServiceProxy
    ) {
        super(injector);
    }

    show(servicePriceId?: number): void {
        
        if (!servicePriceId) {
            this.servicePrice = new CreateOrEditServicePriceDto();
            this.servicePrice.id = servicePriceId;
            this.servicePrice.validity = moment().startOf('day');
            this.serviceName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._servicePricesServiceProxy.getServicePriceForEdit(servicePriceId).subscribe(result => {
                this.servicePrice = result.servicePrice;
                this.serviceName = result.serviceName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;

			
            this._servicePricesServiceProxy.createOrEdit(this.servicePrice)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

se
    openSelectServiceModal() {
        this.servicePriceServiceLookupTableModal.id = this.servicePrice.serviceId;
        this.servicePriceServiceLookupTableModal.displayName = this.serviceName;
        this.servicePriceServiceLookupTableModal.show();
    }


    setServiceIdNull() {
        this.servicePrice.serviceId = null;
        this.serviceName = '';
    }


    getNewServiceId() {
        this.servicePrice.serviceId = this.servicePriceServiceLookupTableModal.id;
        this.serviceName = this.servicePriceServiceLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
