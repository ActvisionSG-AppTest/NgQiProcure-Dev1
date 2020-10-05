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
using QiProcureDemo.ServiceImages.Dto;
using QiProcureDemo.Storage;
using QiProcureDemo.Web.Helpers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QiProcureDemo.Web.Controllers
{
    public abstract class ServiceImagesControllerBase : QiProcureDemoControllerBase
    {

        private readonly ITempFileCacheManager _tempFileCacheManager;

        private const int MaxProfilePictureSize = 5242880; //5MB

        public ServiceImagesControllerBase(ITempFileCacheManager tempFileCacheManager)
        {
            _tempFileCacheManager = tempFileCacheManager;
        }
        public UploadServiceImagesOutput UploadServiceImages(FileDto input)
        {
            try
            {
                var profilePictureFile = Request.Form.Files.First();
                
                byte[] fileBytes;
                using (var stream = profilePictureFile.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                if (!ImageFormatHelper.GetRawImageFormat(fileBytes).IsIn(ImageFormat.Jpeg, ImageFormat.Png, ImageFormat.Gif))
                {
                    throw new Exception(L("IncorrectImageFormat"));
                }
                _tempFileCacheManager.SetFile(input.FileToken, fileBytes);

                using (var bmpImage = new Bitmap(new MemoryStream(fileBytes)))
                {
                    return new UploadServiceImagesOutput
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
                return new UploadServiceImagesOutput(new ErrorInfo(ex.Message));
            }
        }
    }
}
