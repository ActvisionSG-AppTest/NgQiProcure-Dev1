import { Component, OnInit, ViewChild, Injector,EventEmitter,Output, Input } from '@angular/core';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { ParamSettingsCustomServiceProxy ,GetParamSettingForViewDto } from '@shared/service-proxies/service-custom-proxies';
import { NotifyService } from 'abp-ng2-module';
import { ActivatedRoute } from '@angular/router';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LazyLoadEvent } from 'primeng/api';

@Component({
  selector: 'qi-param-settings-listing',
  templateUrl: './param-settings-listing.component.html',
  styleUrls: ['./param-settings-listing.component.css']
})
export class ParamSettingsListingComponent extends AppComponentBase implements OnInit {

  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;

  @Output() paramsSetting : EventEmitter<GetParamSettingForViewDto[]> = new EventEmitter<GetParamSettingForViewDto[]>();

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
          this.paramsSetting.emit(result.items);
          this.primengTableHelper.totalRecordsCount = result.totalCount;
          this.primengTableHelper.records = result.items;
          this.primengTableHelper.hideLoadingIndicator();
      });
  }

  reloadPage(): void {
      this.paginator.changePage(this.paginator.getPage());
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
  ngOnInit() {
  }

}
