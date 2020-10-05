import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetServiceForViewDto, ServiceDto, ServiceImageDto, ServiceImagesCustomServiceProxy, ServiceCategoriesCustomServiceProxy } from '@shared/service-proxies/service-custom-proxies';
import { NameValueServiceOfString, DocumentsCustomServiceProxy } from '@shared/service-proxies/service-custom-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

import { TabDirective } from 'ngx-bootstrap/tabs';

import {
    NgxGalleryOptions,
    NgxGalleryImage,
    NgxGalleryAnimation
  } from "ngx-gallery-9";
import { FileSaverService } from 'ngx-filesaver';

@Component({
    selector: 'viewServiceModal',
    templateUrl: './view-service-modal.component.html',
    styleUrls: ["./view-service-modal.component.css"]
})
export class ViewServiceModalComponent extends AppComponentBase implements OnInit  {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    galleryOptions: NgxGalleryOptions[];
    galleryImages: NgxGalleryImage[] ;
    serviceImageView: ServiceImageDto[];

    active = false;
    saving = false;

    public activeTab: string; 
    item: GetServiceForViewDto;serviceId: number = 0;
    fileName : string = "ServiceImage";categoryName = '';

    filterText = '';
    urlFilter = '';
    nameFilter = '';
    descriptionFilter = '';
    sysRefTenantIdFilter = '';
    productNameFilter = '';
    serviceNameFilter = '';

    documentList: any[] = []
    documents: any[] = []

    serviceImageList: any[] = [];filteredCategories: NameValueServiceOfString[];
    otherCategories: any = {}
    category: any;categories: NameValueServiceOfString[] = new Array<NameValueServiceOfString>();
    imageStartIndex: any = 0;
    
    page:number = 1; showPdf: boolean =false;viewPdf: any = []
    public mobileFriendlyZoomSetting = '150%';

    constructor(
        injector: Injector,
        private _serviceImageCustomService: ServiceImagesCustomServiceProxy,
        private _serviceCategoryCustomService: ServiceCategoriesCustomServiceProxy,
        private _fileSaverService: FileSaverService,
        private _documentCustomService: DocumentsCustomServiceProxy
        ) {
        super(injector);
        this.item = new GetServiceForViewDto();
        this.item.service = new ServiceDto();
    }

    ngOnInit() {
        
        this.displayServiceImages();
        this.displayDocuments();

    }

    private displayServiceImages(){
        this._serviceImageCustomService.getServiceImagesByServiceId(this.item.service.id).subscribe(result => {
            this.serviceImageList = result.serviceImageList; 
        });
               
        this.galleryImages = this.getServiceImages();

        this.setGalleryOption(this.imageStartIndex);
      
    }

    show(item: GetServiceForViewDto): void {
        if (item.service.id != null) {
            
            this.serviceId = item.service.id;
            this.otherCategories = this.bindServiceCategories(item.service.id);   
            this.item = item;
            this.categories = null;
            this.active = true;
            this.modal.show();
            
            this._serviceImageCustomService.getServiceImagesByServiceId(item.service.id).subscribe(result => {
                this.serviceImageList = result.serviceImageList; 
                this.galleryImages = this.getServiceImages();
                this.setGalleryOption(this.imageStartIndex);
                });

            this.displayDocuments()
        }
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
     bindServiceCategories(serviceId){
        const otherCategories = [];

        this.categories = [];

        this._serviceCategoryCustomService.getServiceCategoriesNameValue(serviceId).subscribe(result => {
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

    onSelectTab(data: TabDirective): void {
        this.activeTab = data.heading;
        if (this.activeTab == "Images"){
            this._serviceImageCustomService.getServiceImagesByServiceId(this.item.service.id).subscribe(result => {
                this.serviceImageList = result.serviceImageList; 
                this.galleryImages = this.getServiceImages();
                this.setGalleryOption(this.imageStartIndex);
            });
           
        }

        if (this.activeTab == "Documents"){
            this.displayDocuments()
        }
    }

    getDocuments() {
        const documents = [];
        let index = 1;let documentDescription = ""
        for (const document of this.documentList) {   
            documents.push({
             fileName: document.document.name,
             name:  document.document.description || document.document.name,
             base64: document.document.imageBase64String,
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
        if (this.serviceId != null) {
            this._documentCustomService.getDocuments(
                this.filterText,
                this.urlFilter,
                this.nameFilter,
                this.descriptionFilter,
                this.sysRefTenantIdFilter,
                this.productNameFilter,
                this.serviceNameFilter,
                0,
                this.serviceId,
                "",
                0,
                1
            ).subscribe(result => {
                this.documentList = result.items; 
                this.documents = this.getDocuments();
            });
        }         
    }    
    
    getServiceImages() {
        const images = [];
        let index = 1;let imageDescription = ""
        this.imageStartIndex = 0;
        for (const image of this.serviceImageList) {   
           
            imageDescription = image.fileDescr + ' ( Image: ' + index.toString() + '/' + this.serviceImageList.length 
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

    close(): void {
        this.active = false;
        this.modal.hide();
    }

        
    download(doc){
        this._fileSaverService.save(doc.blob, doc.fileName);
    }

    displayPdf(doc){
        this.showPdf = true;
        this.viewPdf = doc.base64;

    }
}
