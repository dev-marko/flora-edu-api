using Azure.Storage.Blobs.Models;

namespace FloraEdu.Application.Services.Interfaces;

public interface IBlobStorageService
{
    Uri GetPlantThumbnailUploadUri(string blobName, AccessTier? accessTier);
    Uri GetPlantHeaderImageUploadUri(string blobName, AccessTier? accessTier);
    Uri GetArticleHeaderImageUploadUri(string blobName, AccessTier? accessTier);
}
