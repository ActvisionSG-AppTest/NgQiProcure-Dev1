import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TeamMembersCustomServiceProxy, TeamMemberDto  } from '@shared/service-proxies/service-custom-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditTeamMemberModalComponent } from './create-or-edit-teamMember-modal.component';
import { ViewTeamMemberModalComponent } from './view-teamMember-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './teamMembers.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class TeamMembersComponent extends AppComponentBase {

    @ViewChild('createOrEditTeamMemberModal', { static: true }) createOrEditTeamMemberModal: CreateOrEditTeamMemberModalComponent;
    @ViewChild('viewTeamMemberModalComponent', { static: true }) viewTeamMemberModal: ViewTeamMemberModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    remarkFilter = '';
    maxReportingTeamMemberIdFilter : number;
		maxReportingTeamMemberIdFilterEmpty : number;
		minReportingTeamMemberIdFilter : number;
		minReportingTeamMemberIdFilterEmpty : number;
        teamNameFilter = '';
        userNameFilter = '';
        sysRefTenantIdFilter = '';
        sysStatusNameFilter = '';




    constructor(
        injector: Injector,
        private _teamMembersServiceProxy: TeamMembersCustomServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getTeamMembers(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._teamMembersServiceProxy.getAll(
            this.filterText,
            this.remarkFilter,
            this.maxReportingTeamMemberIdFilter == null ? this.maxReportingTeamMemberIdFilterEmpty: this.maxReportingTeamMemberIdFilter,
            this.minReportingTeamMemberIdFilter == null ? this.minReportingTeamMemberIdFilterEmpty: this.minReportingTeamMemberIdFilter,
            this.teamNameFilter,
            this.userNameFilter,
            this.sysRefTenantIdFilter,
            this.sysStatusNameFilter,
            undefined,
            undefined,
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

    createTeamMember(): void {
        this.createOrEditTeamMemberModal.show();
    }

    deleteTeamMember(teamMember: TeamMemberDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._teamMembersServiceProxy.delete(teamMember.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._teamMembersServiceProxy.getTeamMembersToExcel(
        this.filterText,
            this.remarkFilter,
            this.maxReportingTeamMemberIdFilter == null ? this.maxReportingTeamMemberIdFilterEmpty: this.maxReportingTeamMemberIdFilter,
            this.minReportingTeamMemberIdFilter == null ? this.minReportingTeamMemberIdFilterEmpty: this.minReportingTeamMemberIdFilter,
            this.teamNameFilter,
            this.userNameFilter,
            this.sysRefTenantIdFilter,
            this.sysStatusNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
