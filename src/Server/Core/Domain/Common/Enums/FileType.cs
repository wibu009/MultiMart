using System.ComponentModel;

namespace MultiMart.Domain.Common.Enums;

public enum FileType
{
    [Description(".jpg,.png,.jpeg")]
    Image,
    [Description(".mp4,.avi,.mkv,.flv,.wmv,.mov,.webm")]
    Video,
    [Description(".pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.txt")]
    Document,
    [Description(".mp3,.wav,.flac,.ogg,.wma,.aac,.m4a")]
    Audio
}