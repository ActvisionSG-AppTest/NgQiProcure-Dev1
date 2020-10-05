import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ProjectInstructionsCustomServiceProxy, CreateOrEditProjectInstructionDto } from '@shared/service-proxies/service-custom-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { ProjectInstructionProjectLookupTableModalComponent } from './projectInstruction-project-lookup-table-modal.component';

@Component({
    selector: 'createOrEditProjectInstructionModal',
    templateUrl: './create-or-edit-projectInstruction-modal.component.html'
})
export class CreateOrEditProjectInstructionModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('projectInstructionProjectLookupTableModal', { static: true }) projectInstructionProjectLookupTableModal: ProjectInstructionProjectLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    projectInstruction: CreateOrEditProjectInstructionDto = new CreateOrEditProjectInstructionDto();

    projectName = '';


    constructor(
        injector: Injector,
        private _projectInstructionsServiceProxy: ProjectInstructionsCustomServiceProxy
    ) {
        super(injector);
    }

    show(projectInstructionId?: number): void {

        if (!projectInstructionId) {
            this.projectInstruction = new CreateOrEditProjectInstructionDto();
            this.projectInstruction.id = projectInstructionId;
            this.projectName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._projectInstructionsServiceProxy.getProjectInstructionForEdit(projectInstructionId).subscribe(result => {
                this.projectInstruction = result.projectInstruction;

                this.projectName = result.projectName;

                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
            this._projectInstructionsServiceProxy.createOrEdit(this.projectInstruction)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectProjectModal() {
        this.projectInstructionProjectLookupTableModal.id = this.projectInstruction.projectId;
        this.projectInstructionProjectLookupTableModal.displayName = this.projectName;
        this.projectInstructionProjectLookupTableModal.show();
    }


    setProjectIdNull() {
        this.projectInstruction.projectId = null;
        this.projectName = '';
    }


    getNewProjectId() {
        this.projectInstruction.projectId = this.projectInstructionProjectLookupTableModal.id;
        this.projectName = this.projectInstructionProjectLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
