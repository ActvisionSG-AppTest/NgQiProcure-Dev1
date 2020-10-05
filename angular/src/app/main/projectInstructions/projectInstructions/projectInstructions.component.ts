import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProjectInstructionsCustomServiceProxy, ProjectInstructionDto  } from '@shared/service-proxies/service-custom-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditProjectInstructionModalComponent } from './create-or-edit-projectInstruction-modal.component';
import { ViewProjectInstructionModalComponent } from './view-projectInstruction-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './projectInstructions.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ProjectInstructionsComponent extends AppComponentBase {

    @ViewChild('createOrEditProjectInstructionModal', { static: true }) createOrEditProjectInstructionModal: CreateOrEditProjectInstructionModalComponent;
    @ViewChild('viewProjectInstructionModalComponent', { static: true }) viewProjectInstructionModal: ViewProjectInstructionModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxInstructionNoFilter : number;
		maxInstructionNoFilterEmpty : number;
		minInstructionNoFilter : number;
		minInstructionNoFilterEmpty : number;
    instructionsFilter = '';
    remarksFilter = '';
    isActiveFilter = -1;
        projectNameFilter = '';




    constructor(
        injector: Injector,
        private _projectInstructionsServiceProxy: ProjectInstructionsCustomServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getProjectInstructions(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._projectInstructionsServiceProxy.getAll(
            this.filterText,
            this.maxInstructionNoFilter == null ? this.maxInstructionNoFilterEmpty: this.maxInstructionNoFilter,
            this.minInstructionNoFilter == null ? this.minInstructionNoFilterEmpty: this.minInstructionNoFilter,
            this.instructionsFilter,
            this.remarksFilter,
            this.isActiveFilter,
            this.projectNameFilter,
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

    createProjectInstruction(): void {
        this.createOrEditProjectInstructionModal.show();
    }

    deleteProjectInstruction(projectInstruction: ProjectInstructionDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._projectInstructionsServiceProxy.delete(projectInstruction.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._projectInstructionsServiceProxy.getProjectInstructionsToExcel(
        this.filterText,
            this.maxInstructionNoFilter == null ? this.maxInstructionNoFilterEmpty: this.maxInstructionNoFilter,
            this.minInstructionNoFilter == null ? this.minInstructionNoFilterEmpty: this.minInstructionNoFilter,
            this.instructionsFilter,
            this.remarksFilter,
            this.isActiveFilter,
            this.projectNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
