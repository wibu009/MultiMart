using System.Text.RegularExpressions;
using MultiMart.Domain.Common;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using MultiMart.Application.Common.FileStorage;
using MultiMart.Application.Common.FileStorage.Cloudinary;
using MultiMart.Domain.Common.Enums;
using MultiMart.Infrastructure.Common.Extensions;

namespace MultiMart.Infrastructure.FileStorage.Cloudinary;

public class CloudinaryFileStorageService : ICloudinaryFileStorageService
{
    private readonly CloudinaryDotNet.Cloudinary _cloudinary;

    public CloudinaryFileStorageService(IOptions<CloudinarySettings> cloudinarySettings)
    {
        var account = new Account(cloudinarySettings?.Value.CloudName, cloudinarySettings.Value.ApiKey, cloudinarySettings?.Value.ApiSecret);
        _cloudinary = new CloudinaryDotNet.Cloudinary(account);
    }

    public async Task<CloudinaryUploadResult> UploadAsync(FileUploadRequest? request, FileType supportedFileType, string? folderName = null, CancellationToken cancellationToken = default)
    {
        if (request?.Data == null)
        {
            throw new InvalidOperationException("Data is required.");
        }

        if (!supportedFileType.GetDescriptionList().Contains(request.Extension.ToLower(System.Globalization.CultureInfo.CurrentCulture)))
            throw new InvalidOperationException("File Format Not Supported.");
        if (request.Name is null)
            throw new InvalidOperationException("Name is required.");

        string base64Data = Regex.Match(request.Data, "data:(?<type>.+?);base64,(?<data>.+)").Groups["data"].Value;
        byte[] fileData = Convert.FromBase64String(base64Data);

        var uploadParams = supportedFileType switch
        {
            FileType.Image => new ImageUploadParams
            {
                File = new FileDescription(request.Name, new MemoryStream(fileData)),
                Folder = folderName,
                Transformation = new Transformation().Crop("scale").Gravity("face"),
            },
            FileType.Video => new VideoUploadParams
            {
                File = new FileDescription(request.Name, new MemoryStream(fileData)),
                Folder = folderName,
            },
            FileType.Audio => new RawUploadParams
            {
                File = new FileDescription(request.Name, new MemoryStream(fileData)),
                Folder = folderName,
            },
            FileType.Document => new RawUploadParams
            {
                File = new FileDescription(request.Name, new MemoryStream(fileData)),
                Folder = folderName,
            },
            _ => throw new InvalidOperationException("File Format Not Supported.")
        };

        var result = await _cloudinary.UploadAsync(uploadParams);
        return new CloudinaryUploadResult
        {
            PublicId = result.PublicId,
            Url = result.SecureUrl.ToString()
        };
    }

    public async Task<string> RemoveAsync(string idOrUrl, string? folder = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(idOrUrl))
        {
            return string.Empty;
        }

        string publicId = idOrUrl.Contains("http") ? GetIdFromUrl(idOrUrl, folder ?? throw new ArgumentNullException(nameof(folder))) : idOrUrl;
        var deletionParams = new DeletionParams(publicId);
        var result = await _cloudinary.DestroyAsync(deletionParams);
        return result.Result == "ok" ? result.Result : string.Empty;
    }

    private string GetIdFromUrl(string url, string folder)
    {
        var uri = new Uri(url);
        string[] segments = uri.Segments;
        string lastSegment = segments.Last();
        string publicId = folder + '/' + Path.GetFileNameWithoutExtension(lastSegment);
        return publicId;
    }
}