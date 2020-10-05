import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ApprovalsCustomServiceProxy, CreateOrEditApprovalDto } from '@shared/service-proxies/service-custom-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { ApprovalSysRefLookupTableModalComponent } from './approval-sysRef-lookup-table-modal.component';
import { ApprovalTeamLookupTableModalComponent } from './approval-team-lookup-table-modal.component';
import { ApprovalProjectLookupTableModalComponent } from './approval-project-lookup-table-modal.component';
import { ApprovalAccountLookupTableModalComponent } from './approval-account-lookup-table-modal.component';
import { ApprovalUserLookupTableModalComponent } from './approval-user-lookup-table-modal.component';
import { ApprovalSysStatusLookupTableModalComponent } from './approval-sysStatus-lookup-table-modal.component';

@Component({
    selector: 'createOrEditApprovalModal',
    templateUrl: './create-or-edit-approval-modal.component.html'
})
export class CreateOrEditApprovalModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('approvalSysRefLookupTableModal', { static: true }) approvalSysRefLookupTableModal: ApprovalSysRefLookupTableModalComponent;
    @ViewChild('approvalTeamLookupTableModal', { static: true }) approvalTeamLookupTableModal: ApprovalTeamLookupTableModalComponent;
    @ViewChild('approvalProjectLookupTableModal', { static: true }) approvalProjectLookupTableModal: ApprovalProjectLookupTableModalComponent;
    @ViewChild('approvalAccountLookupTableModal', { static: true }) approvalAccountLookupTableModal: ApprovalAccountLookupTableModalComponent;
    @ViewChild('approvalUserLookupTableModal', { static: true }) approvalUserLookupTableModal: ApprovalUserLookupTableModalComponent;
    @ViewChild('approvalSysStatusLookupTableModal', { static: true }) approvalSysStatusLookupTableModal: ApprovalSysStatusLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    approval: CreateOrEditApprovalDto = new CreateOrEditApprovalDto();

    sysRefTenantId = '';
    teamName = '';
    projectName = '';
    accountName = '';
    userName = '';
    sysStatusName = '';
    sysRefRefCode: string;

    constructor(
        injector: Injector,
        private _approvalsServiceProxy: ApprovalsCustomServiceProxy
    ) {
        super(injector);
    }

    show(approvalId?: number): void {

        if (!approvalId) {
            this.approval = new CreateOrEditApprovalDto();
            this.approval.id = approvalId;
            this.sysRefTenantId = '';
            this.teamName = '';
            this.projectName = '';
            this.accountName = '';
            this.userName = '';
            this.sysStatusName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._approvalsServiceProxy.getApprovalForEdit(approvalId).subscribe(result => {
                this.approval = result.approval;

                this.sysRefTenantId = result.sysRefTenantId;
                this.teamName = result.teamName;
                this.projectName = result.projectName;
                this.accountName = result.accountName;
                this.userName = result.userName;
                this.sysStatusName = result.sysStatusName;

                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

            if (this.approval.amount == null) {this.approval.amount = 0};
            if (this.approval.rankNo == null) {this.approval.rankNo = 0}

            this._approvalsServiceProxy.createOrEdit(this.approval)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectSysRefModal() {
        this.approvalSysRefLookupTableModal.id = this.approval.sysRefId;
        this.approvalSysRefLookupTableModal.displayName = this.sysRefRefCode;  
        this.approvalSysRefLookupTableModal.showByRefType("Module");

    }
    openSelectTeamModal() {
        this.approvalTeamLookupTableModal.id = this.approval.teamId;
        this.approvalTeamLookupTableModal.displayName = this.teamName;
        this.approvalTeamLookupTableModal.show();
    }
    openSelectProjectModal() {
        this.approvalProjectLookupTableModal.id = this.approval.projectId;
        this.approvalProjectLookupTableModal.displayName = this.projectName;
        this.approvalProjectLookupTableModal.show();
    }
    openSelectAccountModal() {
        this.approvalAccountLookupTableModal.id = this.approval.accountId;
        this.approvalAccountLookupTableModal.displayName = this.accountName;
        this.approvalAccountLookupTableModal.show();
    }
    openSelectUserModal() {
        this.approvalUserLookupTableModal.id = this.approval.userId;
        this.approvalUserLookupTableModal.displayName = this.userName;
        this.approvalUserLookupTableModal.show();
    }
    openSelectSysStatusModal() {
        this.approvalSysStatusLookupTableModal.id = this.approval.sysStatusId;
        this.approvalSysStatusLookupTableModal.displayName = this.sysStatusName;
        this.approvalSysStatusLookupTableModal.show();
    }


    setSysRefIdNull() {
        this.approval.sysRefId = null;
        this.sysRefTenantId = '';
    }
    setTeamIdNull() {
        this.approval.teamId = null;
        this.teamName = '';
    }
    setProjectIdNull() {
        this.approval.projectId = null;
        this.projectName = '';
    }
    setAccountIdNull() {
        this.approval.accountId = null;
        this.accountName = '';
    }
    setUserIdNull() {
        this.approval.userId = null;
        this.userName = '';
    }
    setSysStatusIdNull() {
        this.approval.sysStatusId = null;
        this.sysStatusName = '';
    }


    getNewSysRefId() {
        this.approval.sysRefId = this.approvalSysRefLookupTableModal.id;
        this.sysRefTenantId = this.approvalSysRefLookupTableModal.displayName;
    }
    getNewTeamId() {
        this.approval.teamId = this.approvalTeamLookupTableModal.id;
        this.teamName = this.approvalTeamLookupTableModal.displayName;
    }
    getNewProjectId() {
        this.approval.projectId = this.approvalProjectLookupTableModal.id;
        this.projectName = this.approvalProjectLookupTableModal.displayName;
    }
    getNewAccountId() {
        this.approval.accountId = this.approvalAccountLookupTableModal.id;
        this.accountName = this.approvalAccountLookupTableModal.displayName;
    }
    getNewUserId() {
        this.approval.userId = this.approvalUserLookupTableModal.id;
        this.userName = this.approvalUserLookupTableModal.displayName;
    }
    getNewSysStatusId() {
        this.approval.sysStatusId = this.approvalSysStatusLookupTableModal.id;
        this.sysStatusName = this.approvalSysStatusLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
