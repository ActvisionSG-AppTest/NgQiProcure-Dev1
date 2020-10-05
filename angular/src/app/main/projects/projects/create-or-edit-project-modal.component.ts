import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ProjectsCustomServiceProxy, CreateOrEditProjectDto } from '@shared/service-proxies/service-custom-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { ProjectAccountLookupTableModalComponent } from './project-account-lookup-table-modal.component';
import { ProjectTeamLookupTableModalComponent } from './project-team-lookup-table-modal.component';
import { ProjectSysStatusLookupTableModalComponent } from './project-sysStatus-lookup-table-modal.component';

@Component({
    selector: 'createOrEditProjectModal',
    templateUrl: './create-or-edit-project-modal.component.html'
})
export class CreateOrEditProjectModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('projectAccountLookupTableModal', { static: true }) projectAccountLookupTableModal: ProjectAccountLookupTableModalComponent;
    @ViewChild('projectTeamLookupTableModal', { static: true }) projectTeamLookupTableModal: ProjectTeamLookupTableModalComponent;
    @ViewChild('projectSysStatusLookupTableModal', { static: true }) projectSysStatusLookupTableModal: ProjectSysStatusLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    project: CreateOrEditProjectDto = new CreateOrEditProjectDto();

    startDate: Date;
    endDate: Date;
    accountName = '';
    teamName = '';
    sysStatusName = '';


    constructor(
        injector: Injector,
        private _projectsServiceProxy: ProjectsCustomServiceProxy
    ) {
        super(injector);
    }

    show(projectId?: number): void {
    this.startDate = null;
    this.endDate = null;

        if (!projectId) {
            this.project = new CreateOrEditProjectDto();
            this.project.id = projectId;
            this.accountName = '';
            this.teamName = '';
            this.sysStatusName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._projectsServiceProxy.getProjectForEdit(projectId).subscribe(result => {
                this.project = result.project;

                if (this.project.startDate) {
					this.startDate = this.project.startDate.toDate();
                }
                if (this.project.endDate) {
					this.endDate = this.project.endDate.toDate();
                }
                this.accountName = result.accountName;
                this.teamName = result.teamName;
                this.sysStatusName = result.sysStatusName;

                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
        if (this.startDate) {
            if (!this.project.startDate) {
                this.project.startDate = moment(this.startDate).startOf('day');
            }
            else {
                this.project.startDate = moment(this.startDate);
            }
        }
        else {
            this.project.startDate = null;
        }
        if (this.endDate) {
            if (!this.project.endDate) {
                this.project.endDate = moment(this.endDate).startOf('day');
            }
            else {
                this.project.endDate = moment(this.endDate);
            }
        }
        else {
            this.project.endDate = null;
        }
            this._projectsServiceProxy.createOrEdit(this.project)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectAccountModal() {
        this.projectAccountLookupTableModal.id = this.project.accountId;
        this.projectAccountLookupTableModal.displayName = this.accountName;
        this.projectAccountLookupTableModal.show();
    }
    openSelectTeamModal() {
        this.projectTeamLookupTableModal.id = this.project.teamId;
        this.projectTeamLookupTableModal.displayName = this.teamName;
        this.projectTeamLookupTableModal.show();
    }
    openSelectSysStatusModal() {
        this.projectSysStatusLookupTableModal.id = this.project.sysStatusId;
        this.projectSysStatusLookupTableModal.displayName = this.sysStatusName;
        this.projectSysStatusLookupTableModal.show();
    }


    setAccountIdNull() {
        this.project.accountId = null;
        this.accountName = '';
    }
    setTeamIdNull() {
        this.project.teamId = null;
        this.teamName = '';
    }
    setSysStatusIdNull() {
        this.project.sysStatusId = null;
        this.sysStatusName = '';
    }


    getNewAccountId() {
        this.project.accountId = this.projectAccountLookupTableModal.id;
        this.accountName = this.projectAccountLookupTableModal.displayName;
    }
    getNewTeamId() {
        this.project.teamId = this.projectTeamLookupTableModal.id;
        this.teamName = this.projectTeamLookupTableModal.displayName;
    }
    getNewSysStatusId() {
        this.project.sysStatusId = this.projectSysStatusLookupTableModal.id;
        this.sysStatusName = this.projectSysStatusLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
