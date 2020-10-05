using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Abp.Extensions;
using Abp.IO.Extensions;
using Abp.UI;
using Abp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using QiProcureDemo.Dto;
using QiProcureDemo.ProductImages.Dto;
using QiProcureDemo.Storage;
using QiProcureDemo.Web.Helpers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QiProcureDemo.Web.Controllers
{
    public abstract class ProductImagesControllerBase : QiProcureDemoControllerBase
    {

        private readonly ITempFileCacheManager _tempFileCacheManager;

        private const int MaxProfilePictureSize = 5242880; //5MB

        public ProductImagesControllerBase(ITempFileCacheManager tempFileCacheManager)
        {
            _tempFileCacheManager = tempFileCacheManager;
        }
        public UploadProductImagesOutput UploadProductImages(FileDto input)
        {
            try
            {
                var profilePictureFile = Request.Form.Files.First();
                
                byte[] fileBytes;
                using (var stream = profilePictureFile.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                if (!ImageFormatHelper.GetRawImageFormat(fileBytes).IsIn(ImageFormat.Jpeg, ImageFormat.Png, ImageFormat.Gif, ImageFormat.Tiff))
                {
                    throw new Exception(L("IncorrectImageFormat"));
                }
                _tempFileCacheManager.SetFile(input.FileToken, fileBytes);

                using (var bmpImage = new Bitmap(new MemoryStream(fileBytes)))
                {
                    return new UploadProductImagesOutput
                    {
                        FileToken = input.FileToken,
                        FileName = input.FileName,
                        FileType = input.FileType,
                        Width = bmpImage.Width,
                        Height = bmpImage.Height
                    };
                }
            }
            catch (UserFriendlyException ex)
            {
                return new UploadProductImagesOutput(new ErrorInfo(ex.Message));
            }
        }
    }
}
