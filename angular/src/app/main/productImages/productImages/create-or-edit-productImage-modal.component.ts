import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ProductImagesCustomServiceProxy, CreateOrEditProductImageDto } from '@shared/service-proxies/service-custom-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { ProductImageProductLookupTableModalComponent } from './productImage-product-lookup-table-modal.component';

@Component({
    selector: 'createOrEditProductImageModal',
    templateUrl: './create-or-edit-productImage-modal.component.html'
})
export class CreateOrEditProductImageModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('productImageProductLookupTableModal', { static: true }) productImageProductLookupTableModal: ProductImageProductLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    productImage: CreateOrEditProductImageDto = new CreateOrEditProductImageDto();

    productName = '';


    constructor(
        injector: Injector,
        private _productImagesServiceProxy: ProductImagesCustomServiceProxy,
    ) {
        super(injector);
    }

    show(productImageId?: number): void {

        if (!productImageId) {
            this.productImage = new CreateOrEditProductImageDto();
            this.productImage.id = productImageId;
            this.productName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._productImagesServiceProxy.getProductImageForEdit(productImageId).subscribe(result => {
                this.productImage = result.productImage;

                this.productName = result.productName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;

			
            this._productImagesServiceProxy.createOrEdit(this.productImage)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectProductModal() {
        this.productImageProductLookupTableModal.id = this.productImage.productId;
        this.productImageProductLookupTableModal.displayName = this.productName;
        this.productImageProductLookupTableModal.show();
    }


    setProductIdNull() {
        this.productImage.productId = null;
        this.productName = '';
    }


    getNewProductId() {
        this.productImage.productId = this.productImageProductLookupTableModal.id;
        this.productName = this.productImageProductLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
