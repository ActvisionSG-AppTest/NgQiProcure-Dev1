import { Component, ViewChild, Injector, Output, EventEmitter, ViewEncapsulation} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {ApprovalsCustomServiceProxy, ApprovalSysRefLookupTableDto } from '@shared/service-proxies/service-custom-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { SysRefsCustomServiceProxy } from '@shared/service-proxies/service-custom-proxies';

@Component({
    selector: 'approvalSysRefLookupTableModal',
    styleUrls: ['./approval-sysRef-lookup-table-modal.component.less'],
    encapsulation: ViewEncapsulation.None,
    templateUrl: './approval-sysRef-lookup-table-modal.component.html'
})
export class ApprovalSysRefLookupTableModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    filterText = '';
    id: number;
    displayName: string;
    
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    active = false;
    saving = false;

    constructor(
        injector: Injector,
        private _approvalsServiceProxy: ApprovalsCustomServiceProxy,
        private _sysRefsCustomServiceProxy: SysRefsCustomServiceProxy

    ) {
        super(injector);
    }

    show(): void {
        this.active = true;
        this.paginator.rows = 5;
        this.getAll();
        this.modal.show();
    }
    showByRefType(refType: string): void {
        this.active = true;
        this.paginator.rows = 5;
        this.getByRefType(refType);
        this.modal.show();
    }
    getAll(event?: LazyLoadEvent) {
        if (!this.active) {
            return;
        }

        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._approvalsServiceProxy.getAllSysRefForLookupTable(
            this.filterText,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }
    getByRefType(refType: string,event?: LazyLoadEvent) {
        if (!this.active) {
            return;
        }

        this.primengTableHelper.showLoadingIndicator();
        this._sysRefsCustomServiceProxy.GetSysRefByRefType(refType,undefined
        ).subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    setAndSave(sysRef: ApprovalSysRefLookupTableDto) {
        this.id = sysRef.id;
        this.displayName = sysRef.displayName;
        this.active = false;
        this.modal.hide();
        this.modalSave.emit(null);
    }

    close(): void {
        this.active = false;
        this.modal.hide();
        this.modalSave.emit(null);
    }
}
