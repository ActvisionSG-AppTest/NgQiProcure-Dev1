import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ApprovalRequestsCustomServiceProxy, CreateOrEditApprovalRequestDto } from '@shared/service-proxies/service-custom-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { ApprovalRequestSysRefLookupTableModalComponent } from './approvalRequest-sysRef-lookup-table-modal.component';
import { ApprovalRequestSysStatusLookupTableModalComponent } from './approvalRequest-sysStatus-lookup-table-modal.component';
import { ApprovalRequestUserLookupTableModalComponent } from './approvalRequest-user-lookup-table-modal.component';

@Component({
    selector: 'createOrEditApprovalRequestModal',
    templateUrl: './create-or-edit-approvalRequest-modal.component.html'
})
export class CreateOrEditApprovalRequestModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('approvalRequestSysRefLookupTableModal', { static: true }) approvalRequestSysRefLookupTableModal: ApprovalRequestSysRefLookupTableModalComponent;
    @ViewChild('approvalRequestSysStatusLookupTableModal', { static: true }) approvalRequestSysStatusLookupTableModal: ApprovalRequestSysStatusLookupTableModalComponent;
    @ViewChild('approvalRequestUserLookupTableModal', { static: true }) approvalRequestUserLookupTableModal: ApprovalRequestUserLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    approvalRequest: CreateOrEditApprovalRequestDto = new CreateOrEditApprovalRequestDto();

    sysRefTenantId = '';
    sysStatusName = '';
    userName = '';


    constructor(
        injector: Injector,
        private _approvalRequestsServiceProxy: ApprovalRequestsCustomServiceProxy
    ) {
        super(injector);
    }

    show(approvalRequestId?: number): void {

        if (!approvalRequestId) {
            this.approvalRequest = new CreateOrEditApprovalRequestDto();
            this.approvalRequest.id = approvalRequestId;
            this.sysRefTenantId = '';
            this.sysStatusName = '';
            this.userName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._approvalRequestsServiceProxy.getApprovalRequestForEdit(approvalRequestId).subscribe(result => {
                this.approvalRequest = result.approvalRequest;

                this.sysRefTenantId = result.sysRefTenantId;
                this.sysStatusName = result.sysStatusName;
                this.userName = result.userName;

                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
            this._approvalRequestsServiceProxy.createOrEdit(this.approvalRequest)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectSysRefModal() {
        this.approvalRequestSysRefLookupTableModal.id = this.approvalRequest.sysRefId;
        this.approvalRequestSysRefLookupTableModal.displayName = this.sysRefTenantId;
        this.approvalRequestSysRefLookupTableModal.show();
    }
    openSelectSysStatusModal() {
        this.approvalRequestSysStatusLookupTableModal.id = this.approvalRequest.sysStatusId;
        this.approvalRequestSysStatusLookupTableModal.displayName = this.sysStatusName;
        this.approvalRequestSysStatusLookupTableModal.show();
    }
    openSelectUserModal() {
        this.approvalRequestUserLookupTableModal.id = this.approvalRequest.userId;
        this.approvalRequestUserLookupTableModal.displayName = this.userName;
        this.approvalRequestUserLookupTableModal.show();
    }


    setSysRefIdNull() {
        this.approvalRequest.sysRefId = null;
        this.sysRefTenantId = '';
    }
    setSysStatusIdNull() {
        this.approvalRequest.sysStatusId = null;
        this.sysStatusName = '';
    }
    setUserIdNull() {
        this.approvalRequest.userId = null;
        this.userName = '';
    }


    getNewSysRefId() {
        this.approvalRequest.sysRefId = this.approvalRequestSysRefLookupTableModal.id;
        this.sysRefTenantId = this.approvalRequestSysRefLookupTableModal.displayName;
    }
    getNewSysStatusId() {
        this.approvalRequest.sysStatusId = this.approvalRequestSysStatusLookupTableModal.id;
        this.sysStatusName = this.approvalRequestSysStatusLookupTableModal.displayName;
    }
    getNewUserId() {
        this.approvalRequest.userId = this.approvalRequestUserLookupTableModal.id;
        this.userName = this.approvalRequestUserLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
