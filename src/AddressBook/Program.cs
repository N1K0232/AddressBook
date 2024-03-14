using AddressBook.BusinessLayer.Settings;
using AddressBook.DataAccessLayer;
using AddressBook.StorageProviders.Extensions;
using TinyHelpers.AspNetCore.Extensions;

var builder = WebApplication.CreateBuilder(args);
ConfigureServices(builder.Services, builder.Configuration, builder.Environment);

var app = builder.Build();
Configure(app, app.Environment, app.Services);

await app.RunAsync();

void ConfigureServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
{
    var appSettings = services.ConfigureAndGet<AppSettings>(configuration, nameof(AppSettings));

    services.AddHttpContextAccessor();
    services.AddMemoryCache();
    services.AddRequestLocalization(appSettings.SupportedCultures);

    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    services.AddSqlServer<DataContext>(configuration.GetConnectionString("SqlConnection"));
    services.AddScoped<IDataContext>(services => services.GetRequiredService<DataContext>());

    if (environment.IsDevelopment())
    {
        services.AddFileSystemStorage(options =>
        {
            options.SiteRootFolder = environment.ContentRootPath;
            options.StorageFolder = appSettings.StorageFolder;
        });
    }
    else
    {
        services.AddAzureStorage(options =>
        {
            options.ConnectionString = configuration.GetConnectionString("AzureStorage");
            options.ContainerName = appSettings.ContainerName;
        });
    }
}

void Configure(IApplicationBuilder app, IWebHostEnvironment environment, IServiceProvider services)
{
    app.UseHttpsRedirection();

    if (environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseRouting();
    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}