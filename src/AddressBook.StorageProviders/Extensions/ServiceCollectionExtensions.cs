using AddressBook.StorageProviders.AzureStorage;
using AddressBook.StorageProviders.FileSystem;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AddressBook.StorageProviders.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAzureStorage(this IServiceCollection services, Action<AzureStorageSettings> setupAction)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentNullException.ThrowIfNull(setupAction, nameof(setupAction));

        var settings = new AzureStorageSettings();
        setupAction.Invoke(settings);

        services.TryAddSingleton(settings);
        services.TryAddScoped<IStorageProvider, AzureStorageProvider>();

        return services;
    }

    public static IServiceCollection AddFileSystemStorage(this IServiceCollection services, Action<FileSystemStorageSettings> setupAction)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentNullException.ThrowIfNull(setupAction, nameof(setupAction));

        var settings = new FileSystemStorageSettings();
        setupAction.Invoke(settings);

        services.TryAddSingleton(settings);
        services.TryAddScoped<IStorageProvider, FileSystemStorageProvider>();

        return services;
    }
}