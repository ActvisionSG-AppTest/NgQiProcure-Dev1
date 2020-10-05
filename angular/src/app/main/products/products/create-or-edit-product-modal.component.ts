import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ViewEncapsulation, Input, ChangeDetectionStrategy } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { ProductsCustomServiceProxy,CreateOrEditProductDto, ProductImageDto, CreateOrEditProductImageDto,ProductImagesCustomServiceProxy,UpdateProductImagesInput, ProductCategoriesCustomServiceProxy, NameValueProductOfString, CategoriesCustomServiceByProductProxy, UploadDocumentsInput, DocumentsCustomServiceProxy} from '@shared/service-proxies/service-custom-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TabDirective } from 'ngx-bootstrap/tabs';
import { FileUploader, FileUploaderOptions, FileItem } from 'ng2-file-upload';
import { AppConsts } from '@shared/AppConsts';
import { TokenService } from 'abp-ng2-module';

import {
    NgxGalleryOptions,
    NgxGalleryImage,
    NgxGalleryAnimation
  } from "ngx-gallery-9";

import * as moment from 'moment';
import { IAjaxResponse } from 'abp-ng2-module';
import { finalize } from 'rxjs/operators';
import { ProductCategoryLookupTableModalComponent } from './product-category-lookup-table-modal.component';
import { ConstantPool } from '@angular/compiler';
import { DOCUMENT } from '@angular/common';
import { FileSaverService } from 'ngx-filesaver';

@Component({
    selector: 'createOrEditProductModal',
    templateUrl: './create-or-edit-product-modal.component.html',
    styleUrls: ["./create-or-edit-product-modal.component.css"],
})
export class CreateOrEditProductModalComponent extends AppComponentBase implements OnInit{

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('productCategoryLookupTableModal', { static: true }) productCategoryLookupTableModal: ProductCategoryLookupTableModalComponent;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    galleryOptions: NgxGalleryOptions[];
    galleryImages: NgxGalleryImage[] ;
    productImageView: ProductImageDto[];

    public activeTab: string; 
    hasBaseDropZoneOver: boolean = false; hasBaseDropZoneOverDocument: boolean = false
    
    uploader: FileUploader; private _uploaderOptions: FileUploaderOptions = {};
    fileName : string = "ProductImage"; active = false;categoryName = ''; imageUploadCompleted: boolean = false

    uploaderDocument: FileUploader; private _uploaderOptionsDocument: FileUploaderOptions = {};
    fileNameDocument : string = "Document"; fileUploadCompleted: boolean = false

    public saving = false;

    product: CreateOrEditProductDto = new CreateOrEditProductDto();    
    productImageList: any[] = []; filteredCategories: NameValueProductOfString[];

    category: any;categories: NameValueProductOfString[] = new Array<NameValueProductOfString>();
    imageStartIndex: any = 0;    
    
    filterText = '';
    urlFilter = '';
    nameFilter = '';
    descriptionFilter = '';
    sysRefTenantIdFilter = '';
    productNameFilter = '';
    serviceNameFilter = '';

    documentList: any[] = []
    documents: any[] = []

    page:number = 1; showPdf: boolean =false;viewPdf: any = []
    public mobileFriendlyZoomSetting = '150%';
    
    constructor(
        injector: Injector,
        private _productsServiceProxy: ProductsCustomServiceProxy,
        private _productImageCustomService: ProductImagesCustomServiceProxy,
        private _documentCustomService: DocumentsCustomServiceProxy,
        private _productCategoriesCustomService: ProductCategoriesCustomServiceProxy,
        private _categoryCustomService: CategoriesCustomServiceByProductProxy,
        private _fileSaverService: FileSaverService,
        private _tokenService: TokenService)  {
        super(injector);
    }

    ngOnInit() {
        this.fileUpload();
        this.fileUploadDocument();

        this.displayProductImages();
        this.displayDocuments();

        if (this.product.id != null) {
            this.bindProductCategories();
        } else{
            this.categories = null;            
        }
    }
    
    private fileUpload(){
        this.uploader = new FileUploader({
            url: AppConsts.remoteServiceBaseUrl + '/ProductImages/UploadProductImages',
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
               this.updateProductImages(resp.result.fileToken,item.file.name,item.file.type,fileDescr);
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
    
    private displayProductImages(){
        this._productImageCustomService.getProductImagesByProductId(this.product.id).subscribe(result => {
            this.productImageList = result.productImageList; 
        });
               
        this.galleryImages = this.getProductImages();

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

    filterCategories(event): void {
        this._categoryCustomService.GetCategoriesNameValue().subscribe(categories => {
            this.filteredCategories = categories;
        });
    }

    // get Selected Catgories
    bindProductCategories(){
        this.categories = [];
        this._productCategoriesCustomService.getProductCategoriesNameValue(this.product.id).subscribe(result => {
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

                    this._productImageCustomService.getProductImageForEdit(Number(imageId)).subscribe(result => {
                        var productImage: CreateOrEditProductImageDto;
                        productImage = result.productImage;
                        productImage.isMain = true;
                        this._productImageCustomService.createOrEdit(productImage)
                        .pipe(finalize(() => { this.saving = false;}))
                        .subscribe(() => {
                           this.notify.info(this.l('SuccessfullySetAsMain'));
                           this.galleryImages = this.setMainProductImages(index);
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

                    this._productImageCustomService.delete(Number(imageId))
                    .subscribe(() => {
                        this.galleryImages.splice(index, 1); 
                        this.notify.success(this.l('SuccessfullyDeleted'));
                    });
            
                }
            }
        );
    }

    show(productId?: number): void {

        if (!productId) {
            this.product = new CreateOrEditProductDto();
            this.product.id = productId;
            this.categoryName = '';
            this.categories = null;
            this.active = true;
            this.modal.show();
        } else {
            this._productsServiceProxy.getProductForEdit(productId).subscribe(result => {
                this.product = result.product;

                this.categoryName = result.categoryName;

                this.active = true;
                this.modal.show();
                this.bindProductCategories();
                this.galleryImages = this.getProductImages();                
                this.setGalleryOption(this.imageStartIndex);

                this.displayDocuments()
            });

        }
    }

    save(): void {
        this.saving = true;
        let savingOk = false;
        
        //check for duplicate Product Name
        if (this.product.id == null) {
            var maxStockFilterEmpty : number;
            var minStockFilter : number;
            this._productsServiceProxy.getAll("",this.product.name,"",maxStockFilterEmpty,minStockFilter,"",-1,-1,
                this.categoryName,"",1,1).pipe(finalize(() => { this.saving = false;}))
                .subscribe(result => {
                        if (result.totalCount != 0)
                        {
                               this.notify.error(this.l('DuplicateRecord'));                                             
                        } else {
                            this._productsServiceProxy.createOrEdit(this.product).pipe(finalize(() => { this.saving = false;})).subscribe(() => {
                                this.notify.info(this.l('SavedSuccessfully'));
                                this.close();
                                this.modalSave.emit(null);
                            });
                                        }
                });
        } else {
            //remove previously selected categories which are not in the current selected categories                
            this._productCategoriesCustomService.getProductCategoriesNameValue(this.product.id).subscribe(
                (categories : NameValueProductOfString[]) => {
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
                            this._productCategoriesCustomService.getProductCategory(this.product.id,Number(currentCategory.value)).subscribe(
                                category  => {
                                if (category != null){
                                    var selectedCategory = category.items[0]
                                    if (selectedCategory != null)
                                    {
                                        this._productCategoriesCustomService.delete(selectedCategory.productCategory.id)
                                            .subscribe(() => {
                                        });
                                    }
                                }
                            })    
                        }                        
                    }
                }
            )
            this._productsServiceProxy.createOrEdit(this.product).pipe(finalize(() => { this.saving = false;})).subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
            this._productCategoriesCustomService.sendAndGetSelectedCategories(this.product.id, this.categories)
                .subscribe((categories: NameValueProductOfString[]) => {                   
                });

        }


    }
    openSelectCategoryModal() {
        this.productCategoryLookupTableModal.id = this.product.categoryId;
        this.productCategoryLookupTableModal.displayName = this.categoryName;
        this.productCategoryLookupTableModal.show();
    }


    setCategoryIdNull() {
        this.product.categoryId = null;
        this.categoryName = '';
    }


    getNewCategoryId() {
        this.product.categoryId = this.productCategoryLookupTableModal.id;
        this.categoryName = this.productCategoryLookupTableModal.displayName;
    }
    
    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
    onSelectTab(data: TabDirective): void {
        this.activeTab = data.heading;
        if (this.activeTab == "Images"){
            this._productImageCustomService.getProductImagesByProductId(this.product.id).subscribe(result => {
                this.productImageList = result.productImageList; 
                this.galleryImages = this.getProductImages();
                this.setGalleryOption(this.imageStartIndex);
            });
           
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
        for (const document of this.documentList) {   
            documents.push({
             fileName: document.document.name,
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

    setMainProductImages(imageIndex) {
        const images = [];
        let index = 0;let imageDescription = ""
        for (const image of this.productImageList) {   
           
            imageDescription = image.fileDescr + ' ( Image: ' + (index + 1).toString() + '/' + this.productImageList.length 
            
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

    updateProductImages(fileToken: string, fileName: string, fileType: string, fileDescr: string): void {
    
        const input = new UpdateProductImagesInput();
        input.fileToken = fileToken;
        input.x = 0;
        input.y = 0;
        input.width = 0;
        input.height = 0;
        input.productId = this.product.id;
        input.fileName = fileName;
        input.fileType = fileType;
        input.description = fileDescr;
        this.saving = true;
        
        this._productImageCustomService.updateProductImages(input)
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
        this._productImageCustomService.getProductImagesByProductId(this.product.id).subscribe(result => {
            this.productImageList = result.productImageList; 
            this.galleryImages = this.getProductImages();
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
        input.productId = this.product.id;
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
        if (this.product.id != null) {
            this._documentCustomService.getDocuments(
                this.filterText,
                this.urlFilter,
                this.nameFilter,
                this.descriptionFilter,
                this.sysRefTenantIdFilter,
                this.productNameFilter,
                this.serviceNameFilter,
                this.product.id,
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
                    this._documentCustomService.delete(Number(documentId))
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
