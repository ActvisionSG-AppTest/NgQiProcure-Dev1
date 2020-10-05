import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ViewEncapsulation } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { CreateOrEditProductDto, ProductImageDto, GetProductForViewDto, ProductDto } from '@shared/service-proxies/service-custom-proxies';
import { ProductImagesCustomServiceProxy,  ProductCategoriesCustomServiceProxy, NameValueProductOfString, ProductPricesCustomServiceProxy, DocumentsCustomServiceProxy } from '@shared/service-proxies/service-custom-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TabDirective } from 'ngx-bootstrap/tabs';

import {
    NgxGalleryOptions,
    NgxGalleryImage,
    NgxGalleryAnimation
  } from "ngx-gallery-9";
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import * as moment from 'moment';
import { FileSaverService } from 'ngx-filesaver';

@Component({
    selector: 'viewProductModal',
    templateUrl: './view-product-modal.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ViewProductModalComponent  extends AppComponentBase implements OnInit{

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    galleryOptions: NgxGalleryOptions[];
    galleryImages: NgxGalleryImage[] ;
    productImageView: ProductImageDto[];

    public activeTab: string; item: GetProductForViewDto;
    
    active = false; public saving = false;fileName : string = "ProductImage";

    product: CreateOrEditProductDto = new CreateOrEditProductDto();
    
    productImageList: any[] = [];

    categoryName = '';productId: number = 0;

    filteredCategories: NameValueProductOfString[];
    category: any;categories: NameValueProductOfString[] = new Array<NameValueProductOfString>();otherCategories: any = {}; imageStartIndex: any
    
    advancedFiltersAreShown = false;
    filterText = '';maxPriceFilter : number;maxPriceFilterEmpty : number;minPriceFilter : number;minPriceFilterEmpty : number;maxvalidityFilter : moment.Moment;
    minvalidityFilter : moment.Moment;productNameFilter = '';urlFilter = '';nameFilter = '';descriptionFilter = '';sysRefTenantIdFilter = '';serviceNameFilter = '';

    documentList: any[] = []; documents: any[] = []

    page:number = 1; showPdf: boolean =false;viewPdf: any = []
    public mobileFriendlyZoomSetting = '150%';

    constructor(
        injector: Injector,
        private _productImageCustomService: ProductImagesCustomServiceProxy,
        private _productCategoryCustomService: ProductCategoriesCustomServiceProxy,
        private _documentCustomService: DocumentsCustomServiceProxy,
        private _productPricesCustomServiceProxy: ProductPricesCustomServiceProxy,
        private _fileSaverService: FileSaverService)  {
        super(injector);
        this.item = new GetProductForViewDto();
        this.item.product = new ProductDto();
        this.otherCategories = this.bindProductCategories(this.product.id);   
    }

    ngOnInit() {
        this.displayProductImages();
        this.displayDocuments();

    }
    
    private displayProductImages(){
        this._productImageCustomService.getProductImagesByProductId(this.product.id).subscribe(result => {
            this.productImageList = result.productImageList; 
        });
        this.galleryImages = this.getProductImages();

        this.setGalleryOption(this.imageStartIndex);
      
    }

    getProductPrices(event?: LazyLoadEvent) {

        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }
    
        this.primengTableHelper.showLoadingIndicator();
    
        this._productPricesCustomServiceProxy.getByProductId(
            this.productId,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }
    
    setGalleryOption(imageStartIndex){
        this.galleryOptions = [
            {
              width: "500px",
              height: "500px",
              thumbnailsColumns: 4,
              imageAnimation: NgxGalleryAnimation.Slide,
              preview: true,
              imagePercent: 100,
              startIndex: imageStartIndex,
              thumbnailsMoveSize: 4,
              imageArrowsAutoHide:true,
              thumbnailsArrowsAutoHide:true,
              previewZoom:true,
              previewRotate:true,
              previewDownload:true,
              previewCloseOnEsc:true,
              imageDescription:true,
              closeIcon:"fa fa-times-circle",
              fullscreenIcon:"fa fa-arrows-alt" ,
              imageBullets:false
  
  
            },
             // max-width 800
             {
              imageAnimation: "zoom",
              breakpoint: 800,
              width: '100%',
              height: '600px',
              imagePercent: 80,
              thumbnailsPercent: 20,
              thumbnailsMargin: 20,
              thumbnailMargin: 20
              },
              // max-width 400
              {                
                  breakpoint: 400,
                  preview: false
              }
          ];
          
    }

    // get Selected Countries
    bindProductCategories(productId){
        const otherCategories = [];

        this.categories = [];

        this._productCategoryCustomService.getProductCategoriesNameValue(productId).subscribe(result => {
            this.categories = result;
            for (const category of this.categories) {
                otherCategories.push({
                    name: category.name,
                    value: category.value
                })
            }
           
        });
        return otherCategories;
    }
  
    show(item: GetProductForViewDto): void {
        if (item.product.id != null) {
            
            this.productId = item.product.id;

            this.otherCategories = this.bindProductCategories(item.product.id);   
            this.item = item;
            this.categories = null;
            this.active = true;
            this.modal.show();
            
            this._productImageCustomService.getProductImagesByProductId(item.product.id).subscribe(result => {
                this.productImageList = result.productImageList; 
                this.galleryImages = this.getProductImages();
                this.setGalleryOption(this.imageStartIndex);
                });

            this.getProductPrices();

            this.displayDocuments()
        }
    }
  
    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
    onSelectTab(data: TabDirective): void {
        this.activeTab = data.heading;
        if (this.activeTab == "Images"){
            this._productImageCustomService.getProductImagesByProductId(this.productId).subscribe(result => {
                this.productImageList = result.productImageList; 
                this.galleryImages = this.getProductImages();
                this.setGalleryOption(this.imageStartIndex);
            });
           
        }

        if (this.activeTab == "Prices"){
           this.getProductPrices()           
        }

        if (this.activeTab == "Documents"){
            this.displayDocuments()
        }
    }

    getProductImages() {
        const images = [];
        let index = 1;let imageDescription = ""
        this.imageStartIndex = 0;
        for (const image of this.productImageList) {   
           
            imageDescription = image.fileDescr + ' ( Image: ' + index.toString() + '/' + this.productImageList.length 
            if (image.isMain){
                imageDescription +=  " - Main "
                this.imageStartIndex = index-1;
            }
            imageDescription += " )"
            
            images.push({
             small: image.imageBase64String,
             medium: image.imageBase64String,
             big: image.imageBase64String,
             description: imageDescription,
             url: image.url ,
             label: image.imageId 
             }
             );
            index += 1;
        }        
        return images;
    }    

    getDocuments() {
        const documents = [];
        let index = 1;let documentDescription = ""
        for (const document of this.documentList) {   
            documents.push({
             fileName: document.document.name, 
             name: document.document.description ||  document.document.name,
             base64: document.document.imageBase64String,
             //Pdf: this.base64ToArrayBuffer(document.document.imageBase64String),
             blob: this.base64ToBlob(document.document.imageBase64String),
             page: 1}
             );
        }        
        return documents;
    }    

    base64ToArrayBuffer(base64) {
        let binary_string =  window.atob(base64);
        let len = binary_string.length;
        let bytes = new Uint8Array(len);
        for (let i = 0; i < len; i++)        {
            bytes[i] = binary_string.charCodeAt(i);
        }
        return bytes.buffer;
    }

    base64ToBlob(base64) {
        let binary_string =  window.atob(base64);
        let len = binary_string.length;
        let bytes = new Uint8Array(len);
        for (let i = 0; i < len; i++)        {
            bytes[i] = binary_string.charCodeAt(i);
        }
        return bytes;
    }

    private displayDocuments(){
        if (this.productId != null) {
            this._documentCustomService.getDocuments(
                this.filterText,
                this.urlFilter,
                this.nameFilter,
                this.descriptionFilter,
                this.sysRefTenantIdFilter,
                this.productNameFilter,
                this.serviceNameFilter,
                this.productId,
                0,
                "",
                0,
                1
            ).subscribe(result => {
                this.documentList = result.items; 
                this.documents = this.getDocuments();
            });
        }         
    }    

    download(doc){
        this._fileSaverService.save(doc.blob, doc.name);
    }

    displayPdf(doc){
        this.showPdf = true;
        this.viewPdf = doc.base64;

    }
}
