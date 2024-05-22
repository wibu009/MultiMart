namespace BookStack.Application.Common.FileStorage.Cloudinary;

public interface ICloudinaryFileStorageService : ITransientService
{
    Task<CloudinaryUploadResult> UploadAsync(FileUploadRequest? request, FileType supportedFileType, string? folderName = null, CancellationToken cancellationToken = default);
    Task<string> RemoveAsync(string idOrUrl, string? folder = null, CancellationToken cancellationToken = default);
}