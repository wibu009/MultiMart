using MultiMart.Application.Common.Interfaces;

namespace MultiMart.Application.Common.FileStorage.LocalStorage;

public interface ILocalFileStorageService : ITransientService
{
    Task<string> UploadAsync<T>(FileUploadRequest? request, FileType supportedFileType, CancellationToken cancellationToken = default)
    where T : class;

    Task RemoveAsync(string path, CancellationToken cancellationToken = default);
}