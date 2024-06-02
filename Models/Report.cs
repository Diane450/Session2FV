using iTextSharp.text.pdf;
using iTextSharp.text;
using Session2v2.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using Avalonia.Platform;

namespace Session2v2.Models
{
    public class Report
    {
        public Document PdfDoc {  get; set; }

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
                throw new Exception();
            }
        }

        private void CreateTitle(string text)
        {
            string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "times.TTF");
            BaseFont fgBaseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

            var title = new Paragraph(text, new Font(fgBaseFont, 14, Font.BOLD, new BaseColor(0, 0, 0)))
            {
                SpacingAfter = 25f,
                SpacingBefore = 25f,
                Alignment = Element.ALIGN_CENTER
            };
            PdfDoc.Add(title);
        }

        public void CreateReport()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            PdfDoc = new Document(PageSize.A4, 40f, 40f, 60f, 60f);
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            PdfWriter.GetInstance(PdfDoc, new FileStream(desktopPath + $"\\Отчет {DateTime.Now:yyyy-MM-dd_HH-mm-ss}.pdf", FileMode.Create));
            PdfDoc.Open();

            string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "times.TTF");
            BaseFont fgBaseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            Font fgFont = new(fgBaseFont, 14, Font.NORMAL, new BaseColor(0, 0, 0));

            AddReportLogo();

            var spacer = new Paragraph("")
            {
                SpacingAfter = 10f,
                SpacingBefore = 10f
            };
            PdfDoc.Add(spacer);

            var title = new Paragraph($"ОТЧЕТ ПОСЕЩАЕМОСТИ ПРЕДПРИЯТИЯ ЗА ПЕРОИД \r ОТ {DateRange[0]} ДО {DateRange[1]}", new Font(fgBaseFont, 14, Font.BOLD, new BaseColor(0, 0, 0)))
            {
                SpacingAfter = 25f,
                Alignment = Element.ALIGN_CENTER
            };
            PdfDoc.Add(title);

            AddHeaderTable(fgFont);

            AddStatisticsTable(fgFont);

            AddStatisticsTablePrivateMeeting(fgFont);

            AddStatisticsTableGroupMeeting(fgFont);

            PdfDoc.Close();
        }

        private void AddReportLogo()
        {
            var image = AssetLoader.Open(new Uri("avares://Session2v2/Assets/report-logo.png"));
            var png = Image.GetInstance(System.Drawing.Image.FromStream(image), ImageFormat.Png);
            png.ScalePercent(25f);
            png.SetAbsolutePosition(PdfDoc.Left, PdfDoc.Top);
            PdfDoc.Add(png);
        }

        private void AddHeaderTable(Font fgFont)
        {
            var headerTable = new PdfPTable(new[] { .75f })
            {
                HorizontalAlignment = 0,
                WidthPercentage = 75,
                DefaultCell = { MinimumHeight = 22f },
            };

            PdfPCell cell = new PdfPCell(new Phrase($"Дата: {DateTime.Now.ToString("dd.MM.yyyy")}", fgFont));
            cell.Border = Rectangle.NO_BORDER;
            headerTable.AddCell(cell);

            cell.Phrase = new Phrase("Отдел: Общий отдел", fgFont);
            headerTable.AddCell(cell);

            cell.Phrase = new Phrase($"Автор: {CurrentUser.Employee.FullName}", fgFont);
            headerTable.AddCell(cell);

            PdfDoc.Add(headerTable);
        }

        private void AddStatisticsTable(Font fgFont)
        {
            CreateTitle("Общая статистика");
            var statisticsTitleTable = new PdfPTable(new[] { .75f })
            {
                HorizontalAlignment = 0,
                WidthPercentage = 75,
                DefaultCell = { MinimumHeight = 22f },
            };

            PdfPCell acceptedRequestsCell = new PdfPCell(new Phrase($"Одобренные заявки: {AcceptedRequestsCount}", fgFont));
            statisticsTitleTable.AddCell(acceptedRequestsCell);

            PdfPCell allequestsCell = new PdfPCell(new Phrase($"Общее количество заявок: {TotalRequestsCount}", fgFont));
            statisticsTitleTable.AddCell(allequestsCell);

            PdfPCell deniedRequestscell = new PdfPCell(new Phrase($"Отклоненные заявки: {DeniedRequestsCount}", fgFont));
            statisticsTitleTable.AddCell(deniedRequestscell);

            PdfDoc.Add(statisticsTitleTable);
        }

        private void AddStatisticsTablePrivateMeeting(Font fgFont)
        {
            CreateTitle("Cтатистика по подразделениям. Личные посещения");
            
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
            PdfDoc.Add(statisticsDepartmentPrivateTable);
        }

        private void AddStatisticsTableGroupMeeting(Font fgFont)
        {
            CreateTitle("Cтатистика по подразделениям.Групповые посещения");

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
            PdfDoc.Add(statisticsDepartmentGroupTable);
        }
    }
}
