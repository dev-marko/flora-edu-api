namespace FloraEdu.Application;

public static class Constants
{
    public const string AzureBlobStorageConnectionStringKey = "AzureBlobStorage";

    public static class StorageContainers
    {
        public const string PlantThumbnails = "plant-thumbnails";
        public const string PlantHeaderImages = "plant-header-images";
        public const string ArticleHeaderImages = "article-header-images";
    }

    public static class BlobSasBuilderResource
    {
        public const string Blob = "b";
    }
}
