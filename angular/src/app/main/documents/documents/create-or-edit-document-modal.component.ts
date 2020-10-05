import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { DocumentsCustomServiceProxy, CreateOrEditDocumentDto } from '@shared/service-proxies/service-custom-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { DocumentSysRefLookupTableModalComponent } from './document-sysRef-lookup-table-modal.component';
import { DocumentProductLookupTableModalComponent } from './document-product-lookup-table-modal.component';
import { DocumentServiceLookupTableModalComponent } from './document-service-lookup-table-modal.component';

@Component({
    selector: 'createOrEditDocumentModal',
    templateUrl: './create-or-edit-document-modal.component.html'
})
export class CreateOrEditDocumentModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('documentSysRefLookupTableModal', { static: true }) documentSysRefLookupTableModal: DocumentSysRefLookupTableModalComponent;
    @ViewChild('documentProductLookupTableModal', { static: true }) documentProductLookupTableModal: DocumentProductLookupTableModalComponent;
    @ViewChild('documentServiceLookupTableModal', { static: true }) documentServiceLookupTableModal: DocumentServiceLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    document: CreateOrEditDocumentDto = new CreateOrEditDocumentDto();

    sysRefTenantId = '';
    productName = '';
    serviceName = '';


    constructor(
        injector: Injector,
        private _documentsServiceProxy: DocumentsCustomServiceProxy
    ) {
        super(injector);
    }

    show(documentId?: number): void {

        if (!documentId) {
            this.document = new CreateOrEditDocumentDto();
            this.document.id = documentId;
            this.sysRefTenantId = '';
            this.productName = '';
            this.serviceName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._documentsServiceProxy.getDocumentForEdit(documentId).subscribe(result => {
                this.document = result.document;

                this.sysRefTenantId = result.sysRefTenantId;
                this.productName = result.productName;
                this.serviceName = result.serviceName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;

			
            this._documentsServiceProxy.createOrEdit(this.document)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectSysRefModal() {
        this.documentSysRefLookupTableModal.id = this.document.sysRefId;
        this.documentSysRefLookupTableModal.displayName = this.sysRefTenantId;
        this.documentSysRefLookupTableModal.show();
    }
    openSelectProductModal() {
        this.documentProductLookupTableModal.id = this.document.productId;
        this.documentProductLookupTableModal.displayName = this.productName;
        this.documentProductLookupTableModal.show();
    }
    openSelectServiceModal() {
        this.documentServiceLookupTableModal.id = this.document.serviceId;
        this.documentServiceLookupTableModal.displayName = this.serviceName;
        this.documentServiceLookupTableModal.show();
    }


    setSysRefIdNull() {
        this.document.sysRefId = null;
        this.sysRefTenantId = '';
    }
    setProductIdNull() {
        this.document.productId = null;
        this.productName = '';
    }
    setServiceIdNull() {
        this.document.serviceId = null;
        this.serviceName = '';
    }


    getNewSysRefId() {
        this.document.sysRefId = this.documentSysRefLookupTableModal.id;
        this.sysRefTenantId = this.documentSysRefLookupTableModal.displayName;
    }
    getNewProductId() {
        this.document.productId = this.documentProductLookupTableModal.id;
        this.productName = this.documentProductLookupTableModal.displayName;
    }
    getNewServiceId() {
        this.document.serviceId = this.documentServiceLookupTableModal.id;
        this.serviceName = this.documentServiceLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
