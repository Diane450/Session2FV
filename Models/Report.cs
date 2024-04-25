using iTextSharp.text.pdf;
using iTextSharp.text;
using Session2v2.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;

namespace Session2v2.Models
{
    public class Report
    {
        public DateOnly[] DateRange { get; set; }

        public int AcceptedRequestsCount { get; set; }

        public int TotalRequestsCount { get; set; }

        public int DeniedRequestsCount { get; set; }

        public Dictionary<string, int> PrivateRequestsDepartment { get; set; } = null!;

        public Dictionary<string, int> GroupRequestsDepartment { get; set; } = null!;


        public Report(DateOnly[] range)
        {
            DateRange = range;
        }

        public async Task GetReportData()
        {
            try
            {
                var AcceptedRequestsCountTask = DBCall.GetAcceptedAmountRequestAsync(DateRange);
                var TotalRequestsCountTask = DBCall.GetTotalAmountRequestsAsync(DateRange);
                var DeniedRequestsCountTask = DBCall.GetDeniedRequestsAsync(DateRange);

                var PrivateRequestsDepartmentTask = DBCall.GetPrivateRequestsDepartmentAsync(DateRange);
                var GroupRequestsDepartmentTask = DBCall.GetGroupRequestsDepartmentAsync(DateRange);

                await Task.WhenAll(AcceptedRequestsCountTask, TotalRequestsCountTask, DeniedRequestsCountTask, PrivateRequestsDepartmentTask, GroupRequestsDepartmentTask);

                AcceptedRequestsCount = await AcceptedRequestsCountTask;
                TotalRequestsCount = await TotalRequestsCountTask;
                DeniedRequestsCount = await DeniedRequestsCountTask;
                PrivateRequestsDepartment = await PrivateRequestsDepartmentTask;
                GroupRequestsDepartment = await GroupRequestsDepartmentTask;
            }
            catch (Exception ex)
            {

            }
            
        }

        public void CreateReport()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var pdfDoc = new Document(PageSize.A4, 40f, 40f, 60f, 60f);
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            PdfWriter.GetInstance(pdfDoc, new FileStream(desktopPath + $"\\Отчет {DateTime.Now:yyyy-MM-dd_HH-mm-ss}.pdf", FileMode.Create));
            pdfDoc.Open();

            string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "times.TTF");

            BaseFont fgBaseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            Font fgFont = new Font(fgBaseFont, 14, Font.NORMAL, new BaseColor(0, 0, 0));

            string projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string filePath = Path.Combine(projectPath, "Assets", "report-logo.png");

            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                var png = Image.GetInstance(System.Drawing.Image.FromStream(fs), ImageFormat.Png);
                png.ScalePercent(25f);
                png.SetAbsolutePosition(pdfDoc.Left, pdfDoc.Top);
                pdfDoc.Add(png);
            }
            var spacer = new Paragraph("")
            {
                SpacingAfter = 10f,
                SpacingBefore = 10f
            };
            pdfDoc.Add(spacer);

            var headerTable = new PdfPTable(new[] { .75f })
            {
                HorizontalAlignment = 0,
                WidthPercentage = 75,
                DefaultCell = { MinimumHeight = 22f },
            };

            var title = new Paragraph($"ОТЧЕТ ПОСЕЩАЕМОСТИ ПРЕДПРИЯТИЯ ЗА ПЕРОИД \r ОТ {DateRange[0]} ДО {DateRange[1]}", new Font(fgBaseFont, 14, Font.BOLD, new BaseColor(0, 0, 0)))
            {
                SpacingAfter = 25f,
                Alignment = Element.ALIGN_CENTER
            };
            pdfDoc.Add(title);

            PdfPCell cell = new PdfPCell(new Phrase($"Дата: {DateTime.Now.ToString("dd.MM.yyyy")}", fgFont));
            cell.Border = Rectangle.NO_BORDER;
            headerTable.AddCell(cell);
            
            cell.Phrase = new Phrase("Отдел: Общий отдел", fgFont);
            headerTable.AddCell(cell);

            cell.Phrase = new Phrase($"Автор: {CurrentUser.FullName}", fgFont);
            headerTable.AddCell(cell);

            pdfDoc.Add(headerTable);

            var statisticsTitle = new Paragraph($"Общая статистика", new Font(fgBaseFont, 14, Font.BOLD, new BaseColor(0, 0, 0)))
            {
                SpacingAfter = 25f,
                SpacingBefore = 25f,
                Alignment = Element.ALIGN_CENTER
            };
            pdfDoc.Add(statisticsTitle);

            var statisticsTitleTable = new PdfPTable(new[] { .75f })
            {
                HorizontalAlignment = 0,
                WidthPercentage = 75,
                DefaultCell = { MinimumHeight = 22f },
            };

            PdfPCell acceptedRequestsCell = new PdfPCell(new Phrase($"Одобренные заявки: {AcceptedRequestsCount}", fgFont));
            cell.Border = Rectangle.NO_BORDER;
            statisticsTitleTable.AddCell(acceptedRequestsCell);

            PdfPCell allequestsCell = new PdfPCell(new Phrase($"Общее количество заявок: {TotalRequestsCount}", fgFont));
            cell.Border = Rectangle.NO_BORDER;
            statisticsTitleTable.AddCell(allequestsCell);

            PdfPCell deniedRequestscell = new PdfPCell(new Phrase($"Отклоненные заявки: {DeniedRequestsCount}", fgFont));
            cell.Border = Rectangle.NO_BORDER;
            statisticsTitleTable.AddCell(deniedRequestscell);

            pdfDoc.Add(statisticsTitleTable);

            
            var statisticsTitleDepartmentPrivate = new Paragraph($"Cтатистика по подразделениям. Личные посещения", new Font(fgBaseFont, 14, Font.BOLD, new BaseColor(0, 0, 0)))
            {
                SpacingAfter = 25f,
                SpacingBefore = 25f,
                Alignment = Element.ALIGN_CENTER
            };
            pdfDoc.Add(statisticsTitleDepartmentPrivate);

            var statisticsDepartmentPrivateTable = new PdfPTable(new[] { .1f, .1f })
            {
                HorizontalAlignment = 0,
                WidthPercentage = 75,
                DefaultCell = { MinimumHeight = 22f },
            };


            foreach (var item in PrivateRequestsDepartment)
            {
                PdfPCell privateRequestsDepartmentCellKey = new PdfPCell(new Phrase(item.Key, fgFont));
                statisticsDepartmentPrivateTable.AddCell(privateRequestsDepartmentCellKey);

                string count = item.Value.ToString();
                PdfPCell privateRequestsDepartmentCellValue = new PdfPCell(new Phrase(count, fgFont));
                statisticsDepartmentPrivateTable.AddCell(privateRequestsDepartmentCellValue);
            }
            pdfDoc.Add(statisticsDepartmentPrivateTable);


            var statisticsTitleDepartmentGroup = new Paragraph($"Cтатистика по подразделениям. Групповые посещения", new Font(fgBaseFont, 14, Font.BOLD, new BaseColor(0, 0, 0)))
            {
                SpacingAfter = 25f,
                SpacingBefore = 25f,
                Alignment = Element.ALIGN_CENTER
            };
            pdfDoc.Add(statisticsTitleDepartmentGroup);

            var statisticsDepartmentGroupTable = new PdfPTable(new[] { .1f, .1f })
            {
                HorizontalAlignment = 0,
                WidthPercentage = 75,
                DefaultCell = { MinimumHeight = 22f },
            };


            foreach (var item in GroupRequestsDepartment)
            {
                PdfPCell groupRequestsDepartmentCellKey = new PdfPCell(new Phrase(item.Key, fgFont));
                statisticsDepartmentGroupTable.AddCell(groupRequestsDepartmentCellKey);

                string count = item.Value.ToString();
                PdfPCell privateRequestsDepartmentCellValue = new PdfPCell(new Phrase(count, fgFont));
                statisticsDepartmentGroupTable.AddCell(privateRequestsDepartmentCellValue);
            }
            pdfDoc.Add(statisticsDepartmentGroupTable);

            pdfDoc.Close();
        }
    }
}
