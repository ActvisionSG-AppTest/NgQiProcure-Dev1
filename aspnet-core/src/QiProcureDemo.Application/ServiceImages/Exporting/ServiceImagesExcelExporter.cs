using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using QiProcureDemo.DataExporting.Excel.NPOI;
using QiProcureDemo.ServiceImages.Dtos;
using QiProcureDemo.Dto;
using QiProcureDemo.Storage;

namespace QiProcureDemo.ServiceImages.Exporting
{
    public class ServiceImagesExcelExporter : NpoiExcelExporterBase, IServiceImagesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ServiceImagesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetServiceImageForViewDto> serviceImages)
        {
            return CreateExcelPackage(
                "ServiceImages.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("ServiceImages"));
                    

                    AddHeader(
                        sheet,
                        L("Description"),
                        L("Url"),
                        L("IsMain"),
                        L("IsApproved"),
                        L("Bytes"),
                        (L("Service")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, serviceImages,
                        _ => _.ServiceImage.Description,
                        _ => _.ServiceImage.Url,
                        _ => _.ServiceImage.IsMain,
                        _ => _.ServiceImage.IsApproved,
                        _ => _.ServiceImage.Bytes,
                        _ => _.ServiceName
                        );

                    for (var i = 0; i < 6; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }

                });
        }
    }
}
