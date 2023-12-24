using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using FloraEdu.Application.Services.Interfaces;
using FloraEdu.Domain.Exceptions;
using Microsoft.Extensions.Configuration;

namespace FloraEdu.Application.Services.Implementations;

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobContainerClient _plantThumbnailsClient;
    private readonly BlobContainerClient _plantHeaderImagesClient;
    private readonly BlobContainerClient _articleHeaderImagesClient;

    public BlobStorageService(IConfiguration configuration)
    {
        var client = new BlobServiceClient(
            configuration.GetConnectionString(Constants.AzureBlobStorageConnectionStringKey));
        _plantThumbnailsClient
            = client.GetBlobContainerClient(Constants.StorageContainers.PlantThumbnails);
        _plantHeaderImagesClient = client.GetBlobContainerClient(Constants.StorageContainers.PlantHeaderImages);
        _articleHeaderImagesClient = client.GetBlobContainerClient(Constants.StorageContainers.ArticleHeaderImages);
    }

    private static Uri GetUploadUri(string blobName, string containerName, BlobContainerClient containerClient,
        AccessTier? accessTier)
    {
        var blobClient = containerClient.GetBlobClient(blobName);
        if (accessTier is not null) blobClient.SetAccessTierAsync((AccessTier)accessTier);

        var blobSasBuilder = new BlobSasBuilder
        {
            BlobContainerName = containerName,
            StartsOn = DateTime.UtcNow,
            ExpiresOn = DateTime.UtcNow.AddMinutes(2),
            Resource = Constants.BlobSasBuilderResource.Blob
        };

        blobSasBuilder.SetPermissions(BlobSasPermissions.Write);

        if (!blobClient.CanGenerateSasUri)
        {
            throw new ApiException("Failed to generate upload SAS Uri", ErrorCodes.OperationFailed);
        }

        var sasUri = blobClient.Uri.AbsoluteUri + blobClient.GenerateSasUri(blobSasBuilder).Query;

        return new Uri(sasUri);
    }

    public Uri GetPlantThumbnailUploadUri(string blobName, AccessTier? accessTier)
    {
        return GetUploadUri(blobName, Constants.StorageContainers.PlantThumbnails, _plantThumbnailsClient,
            accessTier);
    }

    public Uri GetPlantHeaderImageUploadUri(string blobName, AccessTier? accessTier)
    {
        return GetUploadUri(blobName, Constants.StorageContainers.PlantHeaderImages,
            _plantHeaderImagesClient,
            accessTier);
    }

    public Uri GetArticleHeaderImageUploadUri(string blobName, AccessTier? accessTier)
    {
        return GetUploadUri(blobName, Constants.StorageContainers.ArticleHeaderImages,
            _articleHeaderImagesClient,
            accessTier);
    }
}
