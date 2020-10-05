using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using QiProcureDemo.DataExporting.Excel.NPOI;
using QiProcureDemo.ServicePrices.Dtos;
using QiProcureDemo.Dto;
using QiProcureDemo.Storage;

namespace QiProcureDemo.ServicePrices.Exporting
{
    public class ServicePricesExcelExporter : NpoiExcelExporterBase, IServicePricesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ServicePricesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetServicePriceForViewDto> servicePrices)
        {
            return CreateExcelPackage(
                "ServicePrices.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("ServicePrices"));
                    

                    AddHeader(
                        sheet,
                        L("Price"),
                        L("Validity"),
                        (L("Service")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, servicePrices,
                        _ => _.ServicePrice.Price,
                        _ => _timeZoneConverter.Convert(_.ServicePrice.Validity, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.ServiceName
                        );

                    for (var i = 1; i <= servicePrices.Count; i++)
                    {
                        //Formatting cells
                        SetCellDataFormat(sheet.GetRow(i).Cells[1], "yyyy-mm-dd");
                    }

                    for (var i = 0; i < 3; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }

                });
        }
    }
}
