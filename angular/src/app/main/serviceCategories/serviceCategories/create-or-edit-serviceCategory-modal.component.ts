import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ServiceCategoriesCustomServiceProxy, CreateOrEditServiceCategoryDto } from '@shared/service-proxies/service-custom-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { ServiceCategoryServiceLookupTableModalComponent } from './serviceCategory-service-lookup-table-modal.component';
import { ServiceCategoryCategoryLookupTableModalComponent } from './serviceCategory-category-lookup-table-modal.component';

@Component({
    selector: 'createOrEditServiceCategoryModal',
    templateUrl: './create-or-edit-serviceCategory-modal.component.html'
})
export class CreateOrEditServiceCategoryModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('serviceCategoryServiceLookupTableModal', { static: true }) serviceCategoryServiceLookupTableModal: ServiceCategoryServiceLookupTableModalComponent;
    @ViewChild('serviceCategoryCategoryLookupTableModal', { static: true }) serviceCategoryCategoryLookupTableModal: ServiceCategoryCategoryLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    serviceCategory: CreateOrEditServiceCategoryDto = new CreateOrEditServiceCategoryDto();

    serviceName = '';
    categoryName = '';


    constructor(
        injector: Injector,
        private _serviceCategoriesServiceProxy: ServiceCategoriesCustomServiceProxy
    ) {
        super(injector);
    }

    show(serviceCategoryId?: number): void {

        if (!serviceCategoryId) {
            this.serviceCategory = new CreateOrEditServiceCategoryDto();
            this.serviceCategory.id = serviceCategoryId;
            this.serviceName = '';
            this.categoryName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._serviceCategoriesServiceProxy.getServiceCategoryForEdit(serviceCategoryId).subscribe(result => {
                this.serviceCategory = result.serviceCategory;

                this.serviceName = result.serviceName;
                this.categoryName = result.categoryName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;

			
            this._serviceCategoriesServiceProxy.createOrEdit(this.serviceCategory)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectServiceModal() {
        this.serviceCategoryServiceLookupTableModal.id = this.serviceCategory.serviceId;
        this.serviceCategoryServiceLookupTableModal.displayName = this.serviceName;
        this.serviceCategoryServiceLookupTableModal.show();
    }
    openSelectCategoryModal() {
        this.serviceCategoryCategoryLookupTableModal.id = this.serviceCategory.categoryId;
        this.serviceCategoryCategoryLookupTableModal.displayName = this.categoryName;
        this.serviceCategoryCategoryLookupTableModal.show();
    }


    setServiceIdNull() {
        this.serviceCategory.serviceId = null;
        this.serviceName = '';
    }
    setCategoryIdNull() {
        this.serviceCategory.categoryId = null;
        this.categoryName = '';
    }


    getNewServiceId() {
        this.serviceCategory.serviceId = this.serviceCategoryServiceLookupTableModal.id;
        this.serviceName = this.serviceCategoryServiceLookupTableModal.displayName;
    }
    getNewCategoryId() {
        this.serviceCategory.categoryId = this.serviceCategoryCategoryLookupTableModal.id;
        this.categoryName = this.serviceCategoryCategoryLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
