namespace BookStack.Application.Common.FileStorage.Cloudinary;

public class CloudinaryUploadResult
{
    public string PublicId { get; set; } = default!;
    public string Url { get; set; } = default!;
}