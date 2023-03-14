namespace FSH.WebApi.Application.Common.FileStorage;

public interface IFileStorageService : ITransientService
{
    public Task<string> GetStringFileAsync(string name);
    public Task<string> SaveStringFileAsync(string name,string text);
    public Task<string> UploadAsync<T>(FileUploadRequest? request,  FileType supportedFileType, CancellationToken cancellationToken = default, string subPath = "")
    where T : class;

    public void Remove(string? path);
    public string GetRootFilesPath();
}