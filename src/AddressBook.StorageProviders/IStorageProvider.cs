namespace AddressBook.StorageProviders;

public interface IStorageProvider
{
    Task DeleteAsync(string path);

    Task<bool> ExistsAsync(string path);

    Task<Stream?> ReadAsStreamAsync(string path);

    async Task<byte[]?> ReadAsByteArrayAsync(string path)
    {
        using var stream = await ReadAsStreamAsync(path);
        if (stream is null)
        {
            return null;
        }

        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);

        stream.Close();
        return memoryStream.ToArray();
    }

    async Task<string?> ReadAsStringAsync(string path)
    {
        using var stream = await ReadAsStreamAsync(path);
        if (stream is null)
        {
            return null;
        }

        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }

    Task SaveAsync(string path, Stream stream, bool overwrite = false);

    async Task SaveAsync(string path, byte[] content, bool overwrite = false)
    {
        using var stream = new MemoryStream(content);
        await SaveAsync(path, stream, overwrite);
    }
}