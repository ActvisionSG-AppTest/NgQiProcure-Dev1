import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProjectsCustomServiceProxy, ProjectDto  } from '@shared/service-proxies/service-custom-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditProjectModalComponent } from './create-or-edit-project-modal.component';
import { ViewProjectModalComponent } from './view-project-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './projects.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ProjectsComponent extends AppComponentBase {

    @ViewChild('createOrEditProjectModal', { static: true }) createOrEditProjectModal: CreateOrEditProjectModalComponent;
    @ViewChild('viewProjectModalComponent', { static: true }) viewProjectModal: ViewProjectModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    nameFilter = '';
    descriptionFilter = '';
    maxStartDateFilter : moment.Moment;
		minStartDateFilter : moment.Moment;
    maxEndDateFilter : moment.Moment;
		minEndDateFilter : moment.Moment;
    isApproveFilter = -1;
    isActiveFilter = -1;
    publishFilter = -1;
    remarkFilter = '';
        accountNameFilter = '';
        teamNameFilter = '';
        sysStatusNameFilter = '';




    constructor(
        injector: Injector,
        private _projectsServiceProxy: ProjectsCustomServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getProjects(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._projectsServiceProxy.getAll(
            this.filterText,
            this.nameFilter,
            this.descriptionFilter,
            this.maxStartDateFilter,
            this.minStartDateFilter,
            this.maxEndDateFilter,
            this.minEndDateFilter,
            this.isApproveFilter,
            this.isActiveFilter,
            this.publishFilter,
            this.remarkFilter,
            this.accountNameFilter,
            this.teamNameFilter,
            this.sysStatusNameFilter,
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

    createProject(): void {
        this.createOrEditProjectModal.show();
    }

    deleteProject(project: ProjectDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._projectsServiceProxy.delete(project.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._projectsServiceProxy.getProjectsToExcel(
        this.filterText,
            this.nameFilter,
            this.descriptionFilter,
            this.maxStartDateFilter,
            this.minStartDateFilter,
            this.maxEndDateFilter,
            this.minEndDateFilter,
            this.isApproveFilter,
            this.isActiveFilter,
            this.publishFilter,
            this.remarkFilter,
            this.accountNameFilter,
            this.teamNameFilter,
            this.sysStatusNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
