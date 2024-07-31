using MultiMart.Application.Common.Interfaces;
using MultiMart.Domain.Common.Enums;

namespace MultiMart.Application.Common.FileStorage.Cloudinary;

public interface ICloudinaryFileStorageService : ITransientService
{
    Task<CloudinaryUploadResult> UploadAsync(FileUpload? file, FileType supportedFileType, string? folderName = "BookStack", CancellationToken cancellationToken = default);
    Task<string> RemoveAsync(string idOrUrl, string? folder = "BookStack", CancellationToken cancellationToken = default);
}