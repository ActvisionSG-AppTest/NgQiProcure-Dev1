import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TeamsCustomServiceProxy, TeamDto  } from '@shared/service-proxies/service-custom-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditTeamModalComponent } from './create-or-edit-team-modal.component';
import { ViewTeamModalComponent } from './view-team-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './teams.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class TeamsComponent extends AppComponentBase {

    @ViewChild('createOrEditTeamModal', { static: true }) createOrEditTeamModal: CreateOrEditTeamModalComponent;
    @ViewChild('viewTeamModalComponent', { static: true }) viewTeamModal: ViewTeamModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    nameFilter = '';
    descriptionFilter = '';
    isActiveFilter = -1;
    remarkFilter = '';
    sysStatusNameFilter = '';
    referenceTypeNameFilter = "";

    constructor(
        injector: Injector,
        private _teamsServiceProxy: TeamsCustomServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getTeams(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._teamsServiceProxy.getAll(
            this.filterText,
            this.nameFilter,
            this.descriptionFilter,
            this.isActiveFilter,
            this.remarkFilter,
            this.sysStatusNameFilter,
            this.referenceTypeNameFilter,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createTeam(): void {
        this.createOrEditTeamModal.show();
    }

    deleteTeam(team: TeamDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._teamsServiceProxy.delete(team.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._teamsServiceProxy.getTeamsToExcel(
        this.filterText,
            this.nameFilter,
            this.descriptionFilter,
            this.isActiveFilter,
            this.remarkFilter,
            this.sysStatusNameFilter,
            this.referenceTypeNameFilter
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
