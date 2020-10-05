import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetTeamMemberForViewDto, TeamMemberDto } from '@shared/service-proxies/service-custom-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewTeamMemberModal',
    templateUrl: './view-teamMember-modal.component.html'
})
export class ViewTeamMemberModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetTeamMemberForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetTeamMemberForViewDto();
        this.item.teamMember = new TeamMemberDto();
    }

    show(item: GetTeamMemberForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
