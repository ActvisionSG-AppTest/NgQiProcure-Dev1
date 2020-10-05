import { Component, Injector, ViewEncapsulation, ViewChild, Input } from '@angular/core';
import { ProductDto } from '@shared/service-proxies/service-custom-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { EntityTypeHistoryModalComponent } from '@app/shared/common/entityHistory/entity-type-history-modal.component';
import * as _ from 'lodash';
import * as moment from 'moment';
import { ProductPricesCustomServiceProxy } from '@shared/service-proxies/service-custom-proxies';

@Component({
  selector: 'product-prices-listing',
  templateUrl: './product-prices-listing.component.html',
  styleUrls: ['./product-prices-listing.component.css'],
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class ProductPricesListingComponent extends AppComponentBase {
  @Input() product: ProductDto;

  @ViewChild('entityTypeHistoryModal', { static: true }) entityTypeHistoryModal: EntityTypeHistoryModalComponent;
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;

  advancedFiltersAreShown = false;
  filterText = '';
  maxPriceFilter : number;
  maxPriceFilterEmpty : number;
  minPriceFilter : number;
  minPriceFilterEmpty : number;
  maxvalidityFilter : moment.Moment;
  minvalidityFilter : moment.Moment;
      productNameFilter = '';


  _entityTypeFullName = 'QiProcureDemo.ProductPrices.ProductPrice';
  entityHistoryEnabled = false;

  constructor(
      injector: Injector,
      private _productPricesCustomServiceProxy: ProductPricesCustomServiceProxy,
  ) {
      super(injector);    
  }

  private setIsEntityHistoryEnabled(): boolean {
      let customSettings = (abp as any).custom;
      return customSettings.EntityHistory && customSettings.EntityHistory.isEnabled && _.filter(customSettings.EntityHistory.enabledEntities, entityType => entityType === this._entityTypeFullName).length === 1;
  }

  getProductPrices(event?: LazyLoadEvent) {
    if (this.product != null) {
        if (this.product.id != null)
        {
            if (this.primengTableHelper.shouldResetPaging(event)) {
                this.paginator.changePage(0);
                return;
            }
    
            this.primengTableHelper.showLoadingIndicator();
    
            this._productPricesCustomServiceProxy.getByProductId(
                this.product.id,
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


}
