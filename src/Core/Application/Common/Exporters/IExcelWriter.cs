using System.Data;

namespace FSH.WebApi.Application.Common.Exporters;

public interface IExcelWriter : ITransientService
{
    Stream WriteToStream<T>(IList<T> data);
    Task<byte[]> ExportAsync<TData>(IEnumerable<TData> data
            , Dictionary<string, Func<TData, object>> mappers
, string sheetName = "Sheet1");

    Task<IResult<IEnumerable<TEntity>>> ImportAsync<TEntity>(byte[] data
        , Dictionary<string, Func<DataRow, TEntity, object>> mappers
        , string sheetName = "Sheet1");
}