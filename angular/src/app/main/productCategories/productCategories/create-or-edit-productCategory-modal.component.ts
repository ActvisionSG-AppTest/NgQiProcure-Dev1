import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ProductCategoriesCustomServiceProxy, CreateOrEditProductCategoryDto } from '@shared/service-proxies/service-custom-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { ProductCategoryProductLookupTableModalComponent } from './productCategory-product-lookup-table-modal.component';
import { ProductCategoryCategoryLookupTableModalComponent } from './productCategory-category-lookup-table-modal.component';

@Component({
    selector: 'createOrEditProductCategoryModal',
    templateUrl: './create-or-edit-productCategory-modal.component.html'
})
export class CreateOrEditProductCategoryModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('productCategoryProductLookupTableModal', { static: true }) productCategoryProductLookupTableModal: ProductCategoryProductLookupTableModalComponent;
    @ViewChild('productCategoryCategoryLookupTableModal', { static: true }) productCategoryCategoryLookupTableModal: ProductCategoryCategoryLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    productCategory: CreateOrEditProductCategoryDto = new CreateOrEditProductCategoryDto();

    productName = '';
    categoryName = '';


    constructor(
        injector: Injector,
        private _productCategoriesServiceProxy: ProductCategoriesCustomServiceProxy
    ) {
        super(injector);
    }

    show(productCategoryId?: number): void {

        if (!productCategoryId) {
            this.productCategory = new CreateOrEditProductCategoryDto();
            this.productCategory.id = productCategoryId;
            this.productName = '';
            this.categoryName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._productCategoriesServiceProxy.getProductCategoryForEdit(productCategoryId).subscribe(result => {
                this.productCategory = result.productCategory;

                this.productName = result.productName;
                this.categoryName = result.categoryName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;

			
            this._productCategoriesServiceProxy.createOrEdit(this.productCategory)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectProductModal() {
        this.productCategoryProductLookupTableModal.id = this.productCategory.productId;
        this.productCategoryProductLookupTableModal.displayName = this.productName;
        this.productCategoryProductLookupTableModal.show();
    }
    openSelectCategoryModal() {
        this.productCategoryCategoryLookupTableModal.id = this.productCategory.categoryId;
        this.productCategoryCategoryLookupTableModal.displayName = this.categoryName;
        this.productCategoryCategoryLookupTableModal.show();
    }


    setProductIdNull() {
        this.productCategory.productId = null;
        this.productName = '';
    }
    setCategoryIdNull() {
        this.productCategory.categoryId = null;
        this.categoryName = '';
    }


    getNewProductId() {
        this.productCategory.productId = this.productCategoryProductLookupTableModal.id;
        this.productName = this.productCategoryProductLookupTableModal.displayName;
    }
    getNewCategoryId() {
        this.productCategory.categoryId = this.productCategoryCategoryLookupTableModal.id;
        this.categoryName = this.productCategoryCategoryLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
