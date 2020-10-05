import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetProjectInstructionForViewDto, ProjectInstructionDto } from '@shared/service-proxies/service-custom-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewProjectInstructionModal',
    templateUrl: './view-projectInstruction-modal.component.html'
})
export class ViewProjectInstructionModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetProjectInstructionForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetProjectInstructionForViewDto();
        this.item.projectInstruction = new ProjectInstructionDto();
    }

    show(item: GetProjectInstructionForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
