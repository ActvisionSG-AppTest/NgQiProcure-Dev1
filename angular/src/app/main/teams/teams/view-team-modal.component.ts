import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetTeamForViewDto, TeamDto } from '@shared/service-proxies/service-custom-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewTeamModal',
    templateUrl: './view-team-modal.component.html'
})
export class ViewTeamModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetTeamForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetTeamForViewDto();
        this.item.team = new TeamDto();
    }

    show(item: GetTeamForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
