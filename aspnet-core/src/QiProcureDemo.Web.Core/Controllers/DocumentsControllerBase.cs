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
using QiProcureDemo.Documents.Dto;
using QiProcureDemo.Dto;
using QiProcureDemo.Storage;
using QiProcureDemo.Web.Helpers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QiProcureDemo.Web.Controllers
{
    public abstract class DocumentsControllerBase : QiProcureDemoControllerBase
    {

        private readonly ITempFileCacheManager _tempFileCacheManager;

        private const int MaxProfilePictureSize = 5242880; //5MB

        public DocumentsControllerBase(ITempFileCacheManager tempFileCacheManager)
        {
            _tempFileCacheManager = tempFileCacheManager;
        }
        public UploadDocumentsOutput UploadDocuments(FileDto input)
        {
            try
            {
                var profilePictureFile = Request.Form.Files.First();
                
                byte[] fileBytes;
                using (var stream = profilePictureFile.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                _tempFileCacheManager.SetFile(input.FileToken, fileBytes);

                    return new UploadDocumentsOutput
                    {
                        FileToken = input.FileToken,
                        Name = input.FileName,
                        FileExt = input.FileType,
                    };
            }
            catch (UserFriendlyException ex)
            {
                return new UploadDocumentsOutput(new ErrorInfo(ex.Message));
            }
        }
    }
}
