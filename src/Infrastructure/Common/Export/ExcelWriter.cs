using System.ComponentModel;
using System.Data;

using System.Runtime.InteropServices;
using ClosedXML.Excel;
using ClosedXML.Graphics;
using DocumentFormat.OpenXml.Spreadsheet;
using FSH.WebApi.Application.Common.Exporters;
using FSH.WebApi.Application.Common.Interfaces;
using FSH.WebApi.Application.Common.Models;
using Microsoft.Extensions.Localization;
using Org.BouncyCastle.Asn1.BC;
using SixLabors.Fonts;

namespace FSH.WebApi.Infrastructure.Common.Export;

public class ExcelWriter : IExcelWriter
{
    private readonly IStringLocalizer<ExcelWriter> _localizer;
    public ExcelWriter(IStringLocalizer<ExcelWriter> localizer)
    {
        _localizer = localizer;  
    }
    public Stream WriteToStream<T>(IList<T> data)
    {
        PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
        DataTable table = new DataTable("table", "table");
        foreach (PropertyDescriptor prop in properties)
            table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
        foreach (T item in data)
        {
            DataRow row = table.NewRow();
            foreach (PropertyDescriptor prop in properties)
                row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
            table.Rows.Add(row);
        }

        using XLWorkbook wb = new XLWorkbook();
        wb.Worksheets.Add(table);
        Stream stream = new MemoryStream();
        wb.SaveAs(stream);
        stream.Seek(0, SeekOrigin.Begin);
        return stream;
    }
    private void borderAround(IXLCell cell)
    {
        var border = cell.Style.Border;
        border.BottomBorder =
            border.TopBorder =
                border.LeftBorder =
                    border.RightBorder =
                    XLBorderStyleValues.Thin;
       
    }
    private   void UpdateGraphicsEngineFonts()
    {
        bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        if (isWindows) return;

        // https://github.com/ClosedXML/ClosedXML/releases/tag/0.97.0
        const string preferredFont = "DejaVu Sans";
        List<string> fonts = SystemFonts.Collection.Families.Select(f => f.Name).ToList();

        // If the preferred font is not available, use the first available font
        var fontName = fonts.Contains(preferredFont) ? preferredFont : fonts.First();

        // All workbooks created later will use the engine with a fallback font DejaVu Sans
        // (or the first available font)
        LoadOptions.DefaultGraphicEngine = new DefaultGraphicEngine(fontName);
    }
    public async Task<byte[]> ExportAsync<TData>(IEnumerable<TData> data
           , Dictionary<string, Func<TData, object>> mappers
           , string sheetName = "Sheet1")
    {
        UpdateGraphicsEngineFonts();
        using (var workbook = new XLWorkbook())
        {
            workbook.Properties.Author = "";

            var ws = workbook.Worksheets.Add(sheetName);
          
            var colIndex = 1;
            var rowIndex = 1;
            var headers = mappers.Keys.Select(x => x).ToList();
            foreach (var header in headers)
            {
                var cell = ws.Cell(rowIndex, colIndex);
                var fill = cell.Style.Fill;
                cell.Style.Font.SetBold(true);
                
                fill.PatternType = XLFillPatternValues.Solid;
                
                fill.SetBackgroundColor(XLColor.LightBlue);
                borderAround(cell);
                

                cell.Value = header;
                
                colIndex++;
            }
            
            var dataList = data.ToList();
            foreach (var item in dataList)
            {
                colIndex = 1;
                rowIndex++;

                var result = headers.Select(header => mappers[header](item));

                foreach (var value in result)
                {
                    borderAround(ws.Cell(rowIndex, colIndex));
                    ws.Cell(rowIndex, colIndex++).Value = XLCellValue.FromObject(value);
                    
                    
                }
            }
            foreach (var item in ws.ColumnsUsed())
            {
                item.AdjustToContents();
            }
            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                //var base64 = Convert.ToBase64String(stream.ToArray());
                stream.Seek(0, SeekOrigin.Begin);
                return await Task.FromResult(stream.ToArray());
            }
        }
    }

    public async Task<IResult<IEnumerable<TEntity>>> ImportAsync<TEntity>(byte[] data, Dictionary<string, Func<DataRow, TEntity, object>> mappers, string sheetName = "Sheet1")
    {

        using (var workbook = new XLWorkbook(new MemoryStream(data)))
        {
            if (!workbook.Worksheets.Contains(sheetName))
            {
                return await Result<IEnumerable<TEntity>>.FailureAsync(new string[] { string.Format(_localizer["Sheet with name {0} does not exist!"], sheetName) });
            }
            var ws = workbook.Worksheet(sheetName);
            var dt = new DataTable();
            var titlesInFirstRow = true;

            foreach (var firstRowCell in ws.Range(1, 1, 1, ws.LastCellUsed().Address.ColumnNumber).Cells())
            {
                dt.Columns.Add(titlesInFirstRow ? firstRowCell.GetString() : $"Column {firstRowCell.Address.ColumnNumber}");
            }
            var startRow = titlesInFirstRow ? 2 : 1;
            var headers = mappers.Keys.Select(x => x).ToList();
            var errors = new List<string>();
            foreach (var header in headers)
            {
                if (!dt.Columns.Contains(header))
                {
                    errors.Add(string.Format(_localizer["Header '{0}' does not exist in table!"], header));
                }
            }
            if (errors.Any())
            {
                return await Result<IEnumerable<TEntity>>.FailureAsync(errors);
            }
            var lastrow = ws.LastRowUsed();
            var list = new List<TEntity>();
            foreach (IXLRow row in ws.Rows(startRow, lastrow.RowNumber()))
            {
                try
                {
                    DataRow datarow = dt.Rows.Add();
                    var item = (TEntity)Activator.CreateInstance(typeof(TEntity));
                    foreach (IXLCell cell in row.Cells())
                    {
                        if (cell.DataType == XLDataType.DateTime)
                        {
                            datarow[cell.Address.ColumnNumber - 1] = cell.GetDateTime().ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        else
                        {
                            datarow[cell.Address.ColumnNumber - 1] = cell.Value.ToString();
                        }
                    }
                    headers.ForEach(x => mappers[x](datarow, item));
                    list.Add(item);
                }
                catch (Exception e)
                {
                    return await Result<IEnumerable<TEntity>>.FailureAsync(new string[] { string.Format(_localizer["Sheet name {0}:{1}"], sheetName, e.Message) });
                }
            }


            return await Result<IEnumerable<TEntity>>.SuccessAsync(list);
        }
    }
}
