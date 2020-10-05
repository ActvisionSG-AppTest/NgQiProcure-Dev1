import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ViewEncapsulation} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ServicesCustomServiceProxy, CreateOrEditServiceDto,ServiceImagesCustomServiceProxy, ServiceImageDto,ServiceCategoriesCustomServiceProxy, CreateOrEditServiceImageDto, ServicePricesCustomServiceProxy, DocumentsCustomServiceProxy } from '@shared/service-proxies/service-custom-proxies';
import { UpdateServiceImagesInput, NameValueServiceOfString, CategoriesCustomServiceByServiceProxy, UploadDocumentsInput } from '@shared/service-proxies/service-custom-proxies';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

import { ServiceCategoryLookupTableModalComponent } from './service-category-lookup-table-modal.component';
import { ServiceSysRefLookupTableModalComponent } from './service-sysRef-lookup-table-modal.component';

import { TabDirective } from 'ngx-bootstrap/tabs';

import {
    NgxGalleryOptions,
    NgxGalleryImage,
    NgxGalleryAnimation
  } from "ngx-gallery-9";

import { AppConsts } from '@shared/AppConsts';
import { FileUploader, FileUploaderOptions, FileItem } from 'ng2-file-upload';
import { TokenService } from 'abp-ng2-module';
import { IAjaxResponse } from 'abp-ng2-module';
import * as moment from 'moment';
import { FileSaverService } from 'ngx-filesaver';

@Component({
    selector: 'createOrEditServiceModal',
    templateUrl: './create-or-edit-service-modal.component.html',
    styleUrls: ["./create-or-edit-service-modal.component.css"]
})
export class CreateOrEditServiceModalComponent extends AppComponentBase implements OnInit {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('serviceCategoryLookupTableModal', { static: true }) serviceCategoryLookupTableModal: ServiceCategoryLookupTableModalComponent;
    @ViewChild('serviceSysRefLookupTableModal', { static: true }) serviceSysRefLookupTableModal: ServiceSysRefLookupTableModalComponent;
    
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    galleryOptions: NgxGalleryOptions[];
    galleryImages: NgxGalleryImage[] ;
    serviceImageView: ServiceImageDto[];

    active = false;
    saving = false;

    public activeTab:string;

    service: CreateOrEditServiceDto = new CreateOrEditServiceDto();

    hasBaseDropZoneOver: boolean = false; ; hasBaseDropZoneOverDocument: boolean = false
    hasAnotherDropZoneOver: boolean = false;uploader: FileUploader; imageUploadCompleted: boolean = false
    private _uploaderOptions: FileUploaderOptions = {};
    
    fileName : string = "ServiceImage";categoryName = ''; fileUploadCount : number = 0

    uploaderDocument: FileUploader; private _uploaderOptionsDocument: FileUploaderOptions = {};
    fileNameDocument : string = "Document"; fileUploadCompleted: boolean = false

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
    category: any;categories: NameValueServiceOfString[] = new Array<NameValueServiceOfString>();
    imageStartIndex: any = 0;
    sysRefRefCode: string;

    page:number = 1; showPdf: boolean =false;viewPdf: any = []
    public mobileFriendlyZoomSetting = '150%';

    constructor(
        injector: Injector,
        private _servicesServiceProxy: ServicesCustomServiceProxy,
        private _serviceImageCustomService: ServiceImagesCustomServiceProxy,        
        private _documentService: DocumentsCustomServiceProxy,
        private _documentCustomService: DocumentsCustomServiceProxy,
        private _categoryCustomService: CategoriesCustomServiceByServiceProxy,
        private _serviceCategoriesCustomService: ServiceCategoriesCustomServiceProxy,
        private _fileSaverService: FileSaverService,
        private _tokenService: TokenService)  {
        super(injector);
    }

    ngOnInit() {
        
       this.fileUpload();
       this.fileUploadDocument();

       this.displayServiceImages();
       this.displayDocuments();

        if (this.service.id != null) {
            this.bindServiceCategories();
        } else{
            this.categories = null;            
        }
    }

    private fileUpload(){
        this.uploader = new FileUploader({
            url: AppConsts.remoteServiceBaseUrl + '/ServiceImages/UploadServiceImages',
            authToken: "Bearer " + this._tokenService.getToken(),      
            allowedFileType: ["image"],
            removeAfterUpload: true,
            autoUpload: false,
            maxFileSize: 10 * 1024 * 1024
          });
          
          this.uploader.onAfterAddingFile = file => {
            this.imageUploadCompleted = false;  
            file.withCredentials = false;
          };  
          this.uploader.onBuildItemForm = (fileItem: FileItem, form: any) => {
            var fileDescr = ""
            if (fileItem.formData != "")
                fileDescr = fileItem.formData;

            form.append('FileType', fileItem.file.type);
            form.append('FileName', this.fileName);
            form.append('FileToken', this.guid());
            form.append('FileDescr', fileDescr)
        };
        this.uploader.onSuccessItem = (item, response, status) => {
            var fileDescr = ""
            if (item.formData != "")
                fileDescr = item.formData;

            this.saving = false;
            const resp = <IAjaxResponse>JSON.parse(response);
            if (resp.success) {
               this.updateServiceImages(resp.result.fileToken,item.file.name,item.file.type,fileDescr);
            } else {
                this.message.error(resp.error.message);
            }
        }

        this.uploader.setOptions(this._uploaderOptions);
    }

    private fileUploadDocument(){
        this.uploaderDocument = new FileUploader({
            url: AppConsts.remoteServiceBaseUrl + '/Documents/UploadDocuments',
            authToken: "Bearer " + this._tokenService.getToken(),      
            allowedFileType: ["image","pdf","doc","xls","ppt","compress"],
            removeAfterUpload: true,
            autoUpload: false,
            maxFileSize: 10 * 1024 * 1024
          });
          
          this.uploaderDocument.onAfterAddingFile = file => {
            this.fileUploadCompleted = false;     
            file.withCredentials = false;
          };  
          this.uploaderDocument.onBuildItemForm = (fileItem: FileItem, form: any) => {
            var fileDescr = ""
            if (fileItem.formData != "")
                fileDescr = fileItem.formData;
            
            form.append('FileExt', fileItem.file.name.slice((fileItem.file.name.lastIndexOf(".") - 1 >>> 0) + 2));
            form.append('FileName', fileItem.file.name);
            form.append('FileToken', this.guid());
            form.append('FileDescr', fileDescr)
        };
        this.uploaderDocument.onSuccessItem = (item, response, status) => {
            var fileDescr = ""
            if (item.formData != "")
                fileDescr = item.formData;

            this.saving = false;
            const resp = <IAjaxResponse>JSON.parse(response);
            if (resp.success) {
               this.uploadDocuments(resp.result.fileToken,item.file.name,item.file.name.slice((item.file.name.lastIndexOf(".") - 1 >>> 0) + 2),fileDescr);
            } else {
                this.message.error(resp.error.message);
            }
        }

        this.uploaderDocument.setOptions(this._uploaderOptionsDocument);
    }

    private displayServiceImages(){
        this._serviceImageCustomService.getServiceImagesByServiceId(this.service.id).subscribe(result => {
            this.serviceImageList = result.serviceImageList; 
        });
               
        this.galleryImages = this.getServiceImages();

        this.setGalleryOption(this.imageStartIndex);
      
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
              thumbnailActions: [{icon: 'fa fa-tint mr-1', onClick: this.setMainImage.bind(this), titleText: 'set as Main'},
              {icon: 'fa fa-times-circle', onClick: this.deleteImage.bind(this), titleText: 'delete'}],
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
    // get ALL countries
    filterCategories(event): void {
        this._categoryCustomService.GetCategoriesNameValue().subscribe(categories => {
            this.filteredCategories = categories;
        });
    }

    // get Selected Countries
    bindServiceCategories(){
        this.categories = [];
        this._serviceCategoriesCustomService.getServiceCategoriesNameValue(this.service.id).subscribe(result => {
            this.categories = result;
        });


    }
  
    setMainImage(event, index): void {
        var imageId = this.galleryImages[index].label || 0;
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {

                    this._serviceImageCustomService.getServiceImageForEdit(Number(imageId)).subscribe(result => {
                        var serviceImage: CreateOrEditServiceImageDto;
                        serviceImage = result.serviceImage;
                        serviceImage.isMain = true;
                        this._serviceImageCustomService.createOrEdit(serviceImage)
                        .pipe(finalize(() => { this.saving = false;}))
                        .subscribe(() => {
                           this.notify.info(this.l('SuccessfullySetAsMain'));
                           this.galleryImages = this.setMainServiceImages(index);
                           this.setGalleryOption(this.imageStartIndex);
                        });                                                
                    });
            
                }
            }
        );
    }

    deleteImage(event, index): void {
        var imageId = this.galleryImages[index].label || 0;
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {

                    this._serviceImageCustomService.delete(Number(imageId))
                    .subscribe(() => {
                        this.galleryImages.splice(index, 1); 
                        this.notify.success(this.l('SuccessfullyDeleted'));
                    });
            
                }
            }
        );
    }

    show(serviceId?: number): void {

        if (!serviceId) {
            this.service = new CreateOrEditServiceDto();
            this.service.id = serviceId;
            this.categoryName = '';
            this.categories = null;
            this.active = true;
            this.modal.show();
        } else {
            this._servicesServiceProxy.getServiceForEdit(serviceId).subscribe(result => {
                this.service = result.service;

                this.categoryName = result.categoryName;

                this.active = true;
                this.modal.show();
                this.bindServiceCategories();
                this.galleryImages = this.getServiceImages();

                this.setGalleryOption(this.imageStartIndex);

                this.displayDocuments()
            });
        }
    }
    
    save(): void {
        this.saving = true;
        var savingOk = false;
        if (this.service.id == null) {
            var minDurationFilterEmpty : number;
            var maxDurationFilter : number;
            this._servicesServiceProxy.getAll("",this.service.name,"",minDurationFilterEmpty,maxDurationFilter,-1,-1,"",
                this.categoryName,"","",1,1).pipe(finalize(() => { this.saving = false;}))
                .subscribe(result => {
                        if (result.totalCount != 0)
                            {
                                this.notify.error(this.l('DuplicateRecord'));                                             
                            } else {
                                this._servicesServiceProxy.createOrEdit(this.service).pipe(finalize(() => { this.saving = false;})).subscribe(() => {
                                    this.notify.info(this.l('SavedSuccessfully'));
                                    this.close();
                                    this.modalSave.emit(null);
                                });                                                }
                });
        } else {
            //remove previously selected categories which are not in the current selected categories
            this._serviceCategoriesCustomService.getServiceCategoriesNameValue(this.service.id).subscribe(
                (categories : NameValueServiceOfString[]) => {
                    var categories = categories;                    
                    for (const currentCategory of categories )
                    {                        
                        var found = false;
                        for (const newCategory of this.categories )
                        {
                            if (currentCategory.value == newCategory.value){
                                found = true;                               
                                break;
                            }
                        }                        

                        if (!found ){

                            this._serviceCategoriesCustomService.getServiceCategory(this.service.id,Number(currentCategory.value)).subscribe(
                                category  => {
                                    if (category != null){
                                    var selectedCategory = category.items[0]
                                    if (selectedCategory != null)
                                    {
                                        this._serviceCategoriesCustomService.delete(selectedCategory.serviceCategory.id)
                                            .subscribe(() => {
                                        });
                                    }
                                }
                            })    
                        }                        
                    }
                }
            )
            
            this._servicesServiceProxy.createOrEdit(this.service).pipe(finalize(() => { this.saving = false;})).subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
            
            this._serviceCategoriesCustomService.sendAndGetSelectedCategories(this.service.id, this.categories)
                .subscribe((categories: NameValueServiceOfString[]) => {                   
                });
        }

    }

    openSelectCategoryModal() {
        this.serviceCategoryLookupTableModal.id = this.service.categoryId;
        this.serviceCategoryLookupTableModal.displayName = this.categoryName;
        this.serviceCategoryLookupTableModal.show();
    }
    openSelectSysRefModal() {
        this.serviceSysRefLookupTableModal.id = this.service.sysRefId;
        this.serviceSysRefLookupTableModal.displayName = this.sysRefRefCode;                
        this.serviceSysRefLookupTableModal.showByRefType("Duration");
    }


    setCategoryIdNull() {
        this.service.categoryId = null;
        this.categoryName = '';
    }
    setSysRefIdNull() {
        this.service.sysRefId = null;
        this.sysRefRefCode = '';
    }


    getNewCategoryId() {
        this.service.categoryId = this.serviceCategoryLookupTableModal.id;
        this.categoryName = this.serviceCategoryLookupTableModal.displayName;
    }
    getNewSysRefId() {
        this.service.sysRefId = this.serviceSysRefLookupTableModal.id;
        this.sysRefRefCode = this.serviceSysRefLookupTableModal.displayName;
    }


    onSelectTab(data: TabDirective): void {
        this.activeTab = data.heading;
        if (this.activeTab == "Images"){
            this._serviceImageCustomService.getServiceImagesByServiceId(this.service.id).subscribe(result => {
                this.serviceImageList = result.serviceImageList; 
                this.galleryImages = this.getServiceImages();
                this.setGalleryOption(this.imageStartIndex);
            });
           
        }

        if (this.activeTab == "Documents"){
            this.displayDocuments()
        }
    }

    close(): void {
        this.active = false;
        this.modal.hide();
        this.uploader.clearQueue();
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

    getDocuments() {
        const documents = [];
        let index = 1;let documentDescription = ""        
        for (const document of this.documentList) { 
            documents.push({                
             documentId: document.document.id,
             fileName:document.document.name,
             name: document.document.description || document.document.name,
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

    setMainServiceImages(imageIndex) {
        const images = [];
        let index = 0;let imageDescription = ""
        for (const image of this.serviceImageList) {   
           
            imageDescription = image.fileDescr + ' ( Image: ' + (index + 1).toString() + '/' + this.serviceImageList.length 
            
            if (index == imageIndex){
                imageDescription +=  " - Main "
                this.imageStartIndex = index;
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


    updateServiceImages(fileToken: string, fileName: string, fileType: string, fileDescr: string): void {
    
        const input = new UpdateServiceImagesInput();
        input.fileToken = fileToken;
        input.x = 0;
        input.y = 0;
        input.width = 0;
        input.height = 0;
        input.serviceId = this.service.id;
        input.fileName = fileName;
        input.fileType = fileType;
        input.description = fileDescr;
        this.saving = true;
        
        this.fileUploadCount += 1;
        this._serviceImageCustomService.updateServiceImages(input)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {              
   
            });            

            if (this.uploader.queue.length == 1)
            {
                this.imageUploadCompleted = true;       
            }
            if (this.imageUploadCompleted) {
                this.refreshUploadedImages();     
                // this.notify.info(this.l('UploadCompleted'));   
            }       
    }

    refreshUploadedImages(){
        this._serviceImageCustomService.getServiceImagesByServiceId(this.service.id).subscribe(result => {
            this.serviceImageList = result.serviceImageList; 
            this.galleryImages = this.getServiceImages();
            this.setGalleryOption(this.imageStartIndex);                               
            this.notify.info(this.l('UploadedSuccessfully'));
            });  
    }
    uploadDocuments(fileToken: string, fileName: string, fileExt: string, fileDescr: string): void {
    
        const input = new UploadDocumentsInput();
        input.fileToken = fileToken;
        input.x = 0;
        input.y = 0;
        input.width = 0;
        input.height = 0;
        input.serviceId = this.service.id;
        input.Name = fileName;
        input.fileExt = fileExt;
        input.description = fileDescr;
        this.saving = true;           

        this._documentCustomService.uploadDocuments(input)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {         
               
            });
           
            if (this.uploaderDocument.queue.length == 1)
            {
                this.fileUploadCompleted = true;       
            }
            if (this.fileUploadCompleted) {
                this.displayDocuments();                            
                this.notify.info(this.l('UploadedSuccessfully'));
            }                     
    }

    private displayDocuments(){
        if (this.service.id != null) {
            this._documentCustomService.getDocuments(
                this.filterText,
                this.urlFilter,
                this.nameFilter,
                this.descriptionFilter,
                this.sysRefTenantIdFilter,
                this.productNameFilter,
                this.serviceNameFilter,
                0,
                this.service.id,
                "",
                0,
                1
            ).subscribe(result => {
                this.documentList = result.items; 
                this.documents = this.getDocuments();
            });
        }         
    }    

    guid(): string {
        function s4() {
            return Math.floor((1 + Math.random()) * 0x10000)
                .toString(16)
                .substring(1);
        }
        return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
    }
    
    public fileOverBase(e: any): void {
      this.hasBaseDropZoneOver = e;
    }
  
    public fileOverAnother(e: any): void {
      this.hasAnotherDropZoneOver = e;
    }

    public fileOverBaseDocument(e: any): void {
        this.hasBaseDropZoneOverDocument = e;
      }
    
    download(doc){
        this._fileSaverService.save(doc.blob, doc.fileName);
    }

    delete(doc){       
        var documentId = doc.documentId || 0;
        var index = this.documents.findIndex(d => d.documentId === documentId)
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {                                    
                    this._documentService.delete(Number(documentId))
                     .subscribe(() => {
                         this.documents.splice(index, 1); 
                         this.notify.success(this.l('SuccessfullyDeleted'));
                    });
            
                }
            }
        );
    }

    displayPdf(doc){
        this.showPdf = true;
        this.viewPdf = doc.base64;
    }
}
