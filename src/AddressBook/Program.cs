using System.Text.Json.Serialization;
using AddressBook.BusinessLayer.MapperProfiles;
using AddressBook.BusinessLayer.Services;
using AddressBook.BusinessLayer.Services.Interfaces;
using AddressBook.BusinessLayer.Settings;
using AddressBook.BusinessLayer.Validations;
using AddressBook.DataAccessLayer;
using AddressBook.StorageProviders.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;
using OperationResults.AspNetCore;
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

    services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    services.AddAutoMapper(typeof(PersonMapperProfile).Assembly);
    services.AddValidatorsFromAssemblyContaining<SavePersonRequestValidator>();

    services.AddFluentValidationAutoValidation(options =>
    {
        options.DisableDataAnnotationsValidation = true;
    });

    services.AddOperationResult(options =>
    {
        options.ErrorResponseFormat = ErrorResponseFormat.List;
    });

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

    services.AddScoped<IPeopleService, PeopleService>();
}

void Configure(IApplicationBuilder app, IWebHostEnvironment environment, IServiceProvider services)
{
    app.UseHttpsRedirection();
    app.UseRequestLocalization();

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