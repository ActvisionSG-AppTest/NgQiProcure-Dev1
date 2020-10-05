import { Component, ViewEncapsulation, ViewChild, Injector, Input } from '@angular/core';
import { ServiceDto } from '@shared/service-proxies/service-custom-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import * as moment from 'moment';
import { ServicePricesCustomServiceProxy } from '@shared/service-proxies/service-custom-proxies';

@Component({
  selector: 'service-prices-listing',
  templateUrl: './service-prices-listing.component.html',
  styleUrls: ['./service-prices-listing.component.css'],
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class ServicePricesListingComponent extends AppComponentBase {
  @Input() service: ServiceDto;
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  
  advancedFiltersAreShown = false;
    filterText = '';
    maxPriceFilter : number;
		maxPriceFilterEmpty : number;
		minPriceFilter : number;
		minPriceFilterEmpty : number;
    maxValidityFilter : moment.Moment;
		minValidityFilter : moment.Moment;
    serviceNameFilter = '';

  constructor(
    injector: Injector,
    private _servicePricesCustomServiceProxy: ServicePricesCustomServiceProxy,
  ) { super(injector); }

  getServicePrices(event?: LazyLoadEvent) {
    
    if (this.service != null) {
      if (this.service.id != null){
        if (this.primengTableHelper.shouldResetPaging(event)) {
          this.paginator.changePage(0);
            return;
        }
  
        this.primengTableHelper.showLoadingIndicator();
  
        this._servicePricesCustomServiceProxy.GetByServiceId(
            this.service.id,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
      } 
    }

   
  }

  reloadPage(): void {
      this.paginator.changePage(this.paginator.getPage());
  }

}
