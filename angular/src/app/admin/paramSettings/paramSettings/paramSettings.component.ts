import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ParamSettingsCustomServiceProxy, ParamSettingDto  } from '@shared/service-proxies/service-custom-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditParamSettingModalComponent } from './create-or-edit-paramSetting-modal.component';
import { ViewParamSettingModalComponent } from './view-paramSetting-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({    
    templateUrl: './paramSettings.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ParamSettingsComponent extends AppComponentBase {

    @ViewChild('createOrEditParamSettingModal', { static: true }) createOrEditParamSettingModal: CreateOrEditParamSettingModalComponent;
    @ViewChild('viewParamSettingModalComponent', { static: true }) viewParamSettingModal: ViewParamSettingModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    nameFilter = '';
    valueFilter = '';
    descriptionFilter = '';




    constructor(
        injector: Injector,
        private _paramSettingsServiceProxy: ParamSettingsCustomServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getParamSettings(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._paramSettingsServiceProxy.getAll(
            this.filterText,
            this.nameFilter,
            this.valueFilter,
            this.descriptionFilter,
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

    createParamSetting(): void {
        this.createOrEditParamSettingModal.show();
    }

    deleteParamSetting(paramSetting: ParamSettingDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._paramSettingsServiceProxy.delete(paramSetting.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._paramSettingsServiceProxy.getParamSettingsToExcel(
        this.filterText,
            this.nameFilter,
            this.valueFilter,
            this.descriptionFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
