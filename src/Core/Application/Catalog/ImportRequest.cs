using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog;
public class ImportRequest<T>: IRequest<T>
{
    public FileUploadRequest ExcelFile { get; set; } = default!;
    public string? SheetName { get; set; }
}
