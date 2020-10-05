import { Component, OnInit, Input, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { FileUploader, FileItem, FileUploaderOptions } from 'ng2-file-upload';
import { ProductImagesCustomServiceProxy, UpdateProductImagesInput, ProductDto } from '@shared/service-proxies/service-custom-proxies';
import { TokenService } from 'abp-ng2-module';
import { AppConsts } from '@shared/AppConsts';
import { IAjaxResponse } from 'abp-ng2-module';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent extends AppComponentBase  {
  @Input() product: ProductDto;

  //@Output() getMemberPhotoChange = new EventEmitter<string>();
  hasBaseDropZoneOver: boolean = false;
  hasAnotherDropZoneOver: boolean = false;
  uploader: FileUploader;
  private _uploaderOptions: FileUploaderOptions = {};
  public saving = false;
  fileName : string = "ProductImage";
  
  constructor(injector: Injector,
    private _productImageService: ProductImagesCustomServiceProxy,
    private _tokenService: TokenService) {super(injector) }

  ngOnInit() {
    this.uploader = new FileUploader({
      url: AppConsts.remoteServiceBaseUrl + '/ProductImages/UploadProductImages',
      authToken: "Bearer " + this._tokenService.getToken(),      
      allowedFileType: ["image"],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });
    
    this.uploader.onAfterAddingFile = file => {
      file.withCredentials = false;
    };  
    this.uploader.onBuildItemForm = (fileItem: FileItem, form: any) => {
      form.append('FileType', fileItem.file.type);
      form.append('FileName', this.fileName);
      form.append('FileToken', this.guid());
    };
    
    this.uploader.onSuccessItem = (item, response, status) => {

      this.saving = false;
      const resp = <IAjaxResponse>JSON.parse(response);
      if (resp.success) {
          this.updateProductImages(resp.result.fileToken,item.file.name,item.file.type);
      } else {
          this.message.error(resp.error.message);
      }
    };

  this.uploader.setOptions(this._uploaderOptions);
  }

  updateProductImages(fileToken: string, fileName: string, fileType: string): void {
    
    const input = new UpdateProductImagesInput();
    input.fileToken = fileToken;
    input.x = 0;
    input.y = 0;
    input.width = 0;
    input.height = 0;
    input.productId = this.product.id;
    input.fileName = fileName;
    input.fileType = fileType;

    this.saving = true;
    this._productImageService.updateProductImages(input)
        .pipe(finalize(() => { this.saving = false; }))
        .subscribe(() => {
          this.notify.info(this.l('SavedSuccessfully'));
          this.close();
        });
}
  guid(): string {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
            .toString(16)
            .substring(1);
    }
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
  }

  setMainPhoto(photo){

  }

  deletePhoto(photoId: any){

  }
  public fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  public fileOverAnother(e: any): void {
    this.hasAnotherDropZoneOver = e;
  }
  
  close(): void {
    this.uploader.clearQueue();}


}
