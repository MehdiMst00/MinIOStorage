using Microsoft.Extensions.Options;

namespace MinIOStorage.MinioConfiguration;

public static class ConfigureServices
{
    public static IServiceCollection AddMinIO(this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        // Configure minio options
        services.Configure<MinioOptions>(configuration.GetSection(MinioOptions.Section));

        // Get configure minio options
        var provider = services.BuildServiceProvider();
        MinioOptions option = provider.GetRequiredService<IOptions<MinioOptions>>().Value;

        // Add Minio using the custom endpoint and configure additional settings for default MinioClient initialization
        services.AddMinio(configureClient => configureClient
            .WithEndpoint(option.Endpoint)
            .WithCredentials(option.AccessKey, option.SecretKey)
            .WithSSL(environment.IsProduction())
            .Build());

        services.AddScoped<IStorageService, StorageService>();

        return services;
    }
}
