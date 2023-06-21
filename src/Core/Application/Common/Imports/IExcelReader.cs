namespace FSH.WebApi.Application.Common.DataIO;

public interface IExcelReader : ITransientService
{
    Task<IList<T>> ToListAsync<T>(
        FileUploadRequest request,
        FileType supportedFileType,
        string sheetName = "Sheet1",
        Dictionary<string, string> colNames = null);
}