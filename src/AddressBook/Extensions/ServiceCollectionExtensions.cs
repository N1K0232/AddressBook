using AutoMapper;

namespace AddressBook.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAutomapper(this IServiceCollection services, Type type)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentNullException.ThrowIfNull(type, nameof(type));

        services.AddAutoMapper(options =>
        {
            var profiles = new List<Profile>();
            var profileTypes = type.Assembly.GetTypes().Where(t => typeof(Profile).IsAssignableFrom(t));

            foreach (var profileType in profileTypes)
            {
                profiles.Add((Profile)Activator.CreateInstance(profileType));
            }

            options.AddProfiles(profiles);
        });

        return services;
    }
}