import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ProductPricesCustomServiceProxy, CreateOrEditProductPriceDto } from '@shared/service-proxies/service-custom-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { ProductPriceProductLookupTableModalComponent } from './productPrice-product-lookup-table-modal.component';

@Component({
    selector: 'createOrEditProductPriceModal',
    templateUrl: './create-or-edit-productPrice-modal.component.html'
})
export class CreateOrEditProductPriceModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('productPriceProductLookupTableModal', { static: true }) productPriceProductLookupTableModal: ProductPriceProductLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    productPrice: CreateOrEditProductPriceDto = new CreateOrEditProductPriceDto();

    productName = '';

    constructor(
        injector: Injector,
        private _productPricesServiceProxy: ProductPricesCustomServiceProxy
    ) {
        super(injector);
    }

    show(productPriceId?: number): void {
    

        if (!productPriceId) {

            this.productPrice = new CreateOrEditProductPriceDto();
            this.productPrice.id = productPriceId;
            this.productPrice.validity = moment().startOf('day');
            this.productName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._productPricesServiceProxy.getProductPriceForEdit(productPriceId).subscribe(result => {
                this.productPrice = result.productPrice;

                this.productName = result.productName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;

			
            this._productPricesServiceProxy.createOrEdit(this.productPrice)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectProductModal() {
        this.productPriceProductLookupTableModal.id = this.productPrice.productId;
        this.productPriceProductLookupTableModal.displayName = this.productName;
        this.productPriceProductLookupTableModal.show();
    }


    setProductIdNull() {
        this.productPrice.productId = null;
        this.productName = '';
    }


    getNewProductId() {
        this.productPrice.productId = this.productPriceProductLookupTableModal.id;
        this.productName = this.productPriceProductLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
