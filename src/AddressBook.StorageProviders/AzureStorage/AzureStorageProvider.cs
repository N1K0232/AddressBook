using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MimeMapping;

namespace AddressBook.StorageProviders.AzureStorage;

public class AzureStorageProvider : IStorageProvider
{
    private readonly BlobServiceClient blobServiceClient;
    private readonly AzureStorageSettings settings;

    public AzureStorageProvider(AzureStorageSettings settings)
    {
        blobServiceClient = new BlobServiceClient(settings.ConnectionString);
        this.settings = settings;
    }

    public async Task DeleteAsync(string path)
    {
        var blobContainerClient = blobServiceClient.GetBlobContainerClient(settings.ContainerName);
        await blobContainerClient.DeleteBlobIfExistsAsync(path);
    }

    public async Task<bool> ExistsAsync(string path)
    {
        var blobClient = await GetBlobClientAsync(path);
        return await blobClient.ExistsAsync();
    }

    public async Task<Stream?> ReadAsStreamAsync(string path)
    {
        var blobClient = await GetBlobClientAsync(path);
        var exists = await blobClient.ExistsAsync();

        if (!exists)
        {
            return null;
        }

        var stream = await blobClient.OpenReadAsync();
        return stream;
    }

    public async Task SaveAsync(string path, Stream stream, bool overwrite = false)
    {
        var blobClient = await GetBlobClientAsync(path, true);

        if (!overwrite)
        {
            var exists = await blobClient.ExistsAsync();
            if (exists)
            {
                throw new IOException($"The file {path} already exists");
            }
        }

        var headers = new BlobHttpHeaders
        {
            ContentType = MimeUtility.GetMimeMapping(path)
        };

        stream.Position = 0;
        await blobClient.UploadAsync(stream, headers);
    }

    private async Task<BlobClient> GetBlobClientAsync(string path, bool createIfNotExists = false)
    {
        var blobContainerClient = blobServiceClient.GetBlobContainerClient(settings.ContainerName);
        if (createIfNotExists)
        {
            await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.None);
        }

        return blobContainerClient.GetBlobClient(path);
    }
}