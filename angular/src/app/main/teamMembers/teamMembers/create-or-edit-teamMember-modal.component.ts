import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { TeamMembersCustomServiceProxy, CreateOrEditTeamMemberDto } from '@shared/service-proxies/service-custom-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { TeamMemberTeamLookupTableModalComponent } from './teamMember-team-lookup-table-modal.component';
import { TeamMemberUserLookupTableModalComponent } from './teamMember-user-lookup-table-modal.component';
import { TeamMemberSysRefLookupTableModalComponent } from './teamMember-sysRef-lookup-table-modal.component';
import { TeamMemberSysStatusLookupTableModalComponent } from './teamMember-sysStatus-lookup-table-modal.component';

@Component({
    selector: 'createOrEditTeamMemberModal',
    templateUrl: './create-or-edit-teamMember-modal.component.html'
})
export class CreateOrEditTeamMemberModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('teamMemberTeamLookupTableModal', { static: true }) teamMemberTeamLookupTableModal: TeamMemberTeamLookupTableModalComponent;
    @ViewChild('teamMemberUserLookupTableModal', { static: true }) teamMemberUserLookupTableModal: TeamMemberUserLookupTableModalComponent;
    @ViewChild('teamMemberSysRefLookupTableModal', { static: true }) teamMemberSysRefLookupTableModal: TeamMemberSysRefLookupTableModalComponent;
    @ViewChild('teamMemberSysStatusLookupTableModal', { static: true }) teamMemberSysStatusLookupTableModal: TeamMemberSysStatusLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    teamMember: CreateOrEditTeamMemberDto = new CreateOrEditTeamMemberDto();

    teamName = '';
    userName = '';
    sysRefTenantId = '';
    sysStatusName = '';


    constructor(
        injector: Injector,
        private _teamMembersServiceProxy: TeamMembersCustomServiceProxy
    ) {
        super(injector);
    }

    show(teamMemberId?: number): void {

        if (!teamMemberId) {
            this.teamMember = new CreateOrEditTeamMemberDto();
            this.teamMember.id = teamMemberId;
            this.teamName = '';
            this.userName = '';
            this.sysRefTenantId = '';
            this.sysStatusName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._teamMembersServiceProxy.getTeamMemberForEdit(teamMemberId).subscribe(result => {
                this.teamMember = result.teamMember;

                this.teamName = result.teamName;
                this.userName = result.userName;
                this.sysRefTenantId = result.sysRefTenantId;
                this.sysStatusName = result.sysStatusName;

                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
            this._teamMembersServiceProxy.createOrEdit(this.teamMember)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectTeamModal() {
        this.teamMemberTeamLookupTableModal.id = this.teamMember.teamId;
        this.teamMemberTeamLookupTableModal.displayName = this.teamName;
        this.teamMemberTeamLookupTableModal.show();
    }
    openSelectUserModal() {
        this.teamMemberUserLookupTableModal.id = this.teamMember.userId;
        this.teamMemberUserLookupTableModal.displayName = this.userName;
        this.teamMemberUserLookupTableModal.show();
    }
    openSelectSysRefModal() {
        this.teamMemberSysRefLookupTableModal.id = this.teamMember.sysRefId;
        this.teamMemberSysRefLookupTableModal.displayName = this.sysRefTenantId;
        this.teamMemberSysRefLookupTableModal.show();
    }
    openSelectSysStatusModal() {
        this.teamMemberSysStatusLookupTableModal.id = this.teamMember.sysStatusId;
        this.teamMemberSysStatusLookupTableModal.displayName = this.sysStatusName;
        this.teamMemberSysStatusLookupTableModal.show();
    }


    setTeamIdNull() {
        this.teamMember.teamId = null;
        this.teamName = '';
    }
    setUserIdNull() {
        this.teamMember.userId = null;
        this.userName = '';
    }
    setSysRefIdNull() {
        this.teamMember.sysRefId = null;
        this.sysRefTenantId = '';
    }
    setSysStatusIdNull() {
        this.teamMember.sysStatusId = null;
        this.sysStatusName = '';
    }


    getNewTeamId() {
        this.teamMember.teamId = this.teamMemberTeamLookupTableModal.id;
        this.teamName = this.teamMemberTeamLookupTableModal.displayName;
    }
    getNewUserId() {
        this.teamMember.userId = this.teamMemberUserLookupTableModal.id;
        this.userName = this.teamMemberUserLookupTableModal.displayName;
    }
    getNewSysRefId() {
        this.teamMember.sysRefId = this.teamMemberSysRefLookupTableModal.id;
        this.sysRefTenantId = this.teamMemberSysRefLookupTableModal.displayName;
    }
    getNewSysStatusId() {
        this.teamMember.sysStatusId = this.teamMemberSysStatusLookupTableModal.id;
        this.sysStatusName = this.teamMemberSysStatusLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
