using System;
using System.IO;
using System.Reflection;

using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

using Saeed.Utilities.DynamicSettings.Caching;
using Saeed.Utilities.DynamicSettings.Database;
using Saeed.Utilities.DynamicSettings.Identity;
using Saeed.Utilities.DynamicSettings.RabbitMq;
using Saeed.Utilities.DynamicSettings.Site;
using Saeed.Utilities.DynamicSettings.Storage;
using Saeed.Utilities.Infrastructures.Ef.Repositories;
using Saeed.Utilities.Infrastructures.Mongo.Repositories;

namespace Saeed.Utilities.Extensions.DependencyInjection
{
    public static class ServiceConfigurationExtenstions
    {
        /// <summary>
        /// get appsettings.{Development|Staging|Production}.json file by default, from specified assembly or path. entry must be the web or api project assembly type, who contain configuration settings.
        /// example: GetConfiguration(typeof(Startup).Assembly) which startup is the web project startup file.
        /// </summary>
        /// <param name="assembly">startup or configured project whose contain appsettings</param>
        /// <param name="path">path of appsettings file, instead of assembly</param>
        /// <param name="environment">specific json file by environment name like appsettings.Staging.json</param>
        /// <returns>the specific environment configuration</returns>
        public static IConfiguration GetConfiguration(Assembly assembly = null, string path = null, string environment = "Staging")
        {
            if (assembly == null && path == null)
                throw new ArgumentNullException(nameof(assembly), new ArgumentNullException(nameof(path)));
            if (string.IsNullOrWhiteSpace(environment))
            {
                throw new ArgumentException($"'{nameof(environment)}' cannot be null or whitespace.", nameof(environment));
            }

            var settingFile = path ?? Path.GetDirectoryName(assembly.Location);
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
                    .SetBasePath(settingFile) // set project path to read files
                    .AddJsonFile($"appsettings.{environment}.json", optional: false, reloadOnChange: true) // settings file in api prj
                    .AddEnvironmentVariables();
            return configurationBuilder.Build();
        }

        /// <summary>
        /// Bind Specific appsettings.json sections, to <see cref="DatabaseSettings"/> class as dynamic setting in 2 step:
        /// <list type="number">
        /// <item>
        /// Get <paramref name="databaseSettingsSection"/> and <paramref name="connectionStringsSection"/> that stored in <paramref name="configuration"/> (appsettings.json accessor), and bind them to the <see cref="DatabaseSettings"/> Class.
        /// </item>
        /// <item>
        /// In the next, <see cref="DatabaseSettings"/> will be registered in di container as <see cref="ServiceLifetime.Singleton"/> by default, 
        /// or as <see cref="IOptions{TOptions}"/> with
        /// <see cref="ServiceLifetime.Scoped"/> / <see cref="ServiceLifetime.Transient"/> lifetime, if defined in <paramref name="serviceLifetime"/> arg.
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="serviceProviderBuilder">services builder</param>
        /// <param name="configuration">IConfiguration accessor</param>
        /// <param name="serviceLifetime">option lifetime, Singleton (static) by default.</param>
        /// <param name="databaseSettingsSection">nameof DatabaseSettings section in appsettings.json</param>
        /// <param name="connectionStringsSection">nameof Connectionstrings section in appsettings.json</param>
        /// <returns></returns>
        public static DatabaseSettings RegisterDatabaseSettingOptions(this IServiceCollection serviceProviderBuilder, IConfiguration configuration, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton, string databaseSettingsSection = "DatabaseSettings", string connectionStringsSection = "ConnectionStrings")
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (string.IsNullOrWhiteSpace(databaseSettingsSection))
            {
                throw new ArgumentException($"'{nameof(databaseSettingsSection)}' cannot be null or whitespace.", nameof(databaseSettingsSection));
            }

            if (string.IsNullOrWhiteSpace(connectionStringsSection))
            {
                throw new ArgumentException($"'{nameof(connectionStringsSection)}' cannot be null or whitespace.", nameof(connectionStringsSection));
            }

            var dbConfig = new DatabaseSettings();
            dbConfig.ConnectionStrings = new DatabaseConnectionStringSettings();

            // bind DatabaseSettings Section To DatabaseSettings C# class.
            configuration.Bind(nameof(DatabaseSettings), dbConfig);
            // bind ConnectionStrings to DatabaseSettings.DatabaseConnectionStringSettings Property.
            configuration.Bind(connectionStringsSection, dbConfig.ConnectionStrings);
            // bind ConnectionStrings Section Seperatly To DatabaseConnectionStringSettings C# class. ( to resolve each one you needed. you need DatabaseSettings mostly)

            DatabaseConnectionStringSettings dbConnectionStrings = new();
            configuration.Bind(connectionStringsSection, dbConnectionStrings);

            // register to IOC by selected lifetime. normally, database settings not change at runtime. but if it does, register as scoped to reload per request.
            switch (serviceLifetime)
            {
                case ServiceLifetime.Scoped:
                case ServiceLifetime.Transient:
                    serviceProviderBuilder.AddOptions<DatabaseSettings>()
                        .Bind(configuration.GetSection(databaseSettingsSection))
                        .ValidateDataAnnotations();

                    serviceProviderBuilder.AddOptions<DatabaseConnectionStringSettings>()
                        .Bind(configuration.GetSection(databaseSettingsSection))
                        .ValidateDataAnnotations();
                    break;
                case ServiceLifetime.Singleton:
                default:
                    serviceProviderBuilder.AddSingleton(dbConfig);
                    serviceProviderBuilder.AddSingleton(dbConfig.ConnectionStrings);
                    serviceProviderBuilder.AddSingleton(dbConnectionStrings);
                    break;
            }

            return dbConfig;
        }

        /// <summary>
        /// add mongo db generic repository ( base repositories impl) to di.
        /// </summary>
        /// <param name="serviceProviderBuilder"></param>
        /// <param name="serviceLifetime">mongodb standard is one connection in the whole app life. so default is singleton.</param>
        /// <returns></returns>
        public static IServiceCollection AddMongoDbGenericRepository(this IServiceCollection serviceProviderBuilder, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
        {
            switch (serviceLifetime)
            {
                case ServiceLifetime.Scoped:
                    serviceProviderBuilder.TryAddScoped(typeof(IGenericMongoRepository<>), typeof(GenericMongoRepository<>));
                    break;
                case ServiceLifetime.Transient:
                    serviceProviderBuilder.TryAddTransient(typeof(IGenericMongoRepository<>), typeof(GenericMongoRepository<>));
                    break;
                case ServiceLifetime.Singleton:
                default:
                    serviceProviderBuilder.TryAddSingleton(typeof(IGenericMongoRepository<>), typeof(GenericMongoRepository<>));
                    break;
            }
            return serviceProviderBuilder;
        }

        /// <summary>
        /// add efcore generic repository to di.
        /// </summary>
        /// <param name="serviceProviderBuilder"></param>
        /// <param name="serviceLifetime">scoped by default, cause efcore is scoped by default.</param>
        /// <returns></returns>
        public static IServiceCollection AddEfGenericRepository(this IServiceCollection serviceProviderBuilder, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            switch (serviceLifetime)
            {
                case ServiceLifetime.Scoped:
                    serviceProviderBuilder.TryAddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
                    break;
                case ServiceLifetime.Transient:
                    serviceProviderBuilder.TryAddTransient(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
                    break;
                case ServiceLifetime.Singleton:
                default:
                    serviceProviderBuilder.TryAddSingleton(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
                    break;
            }
            return serviceProviderBuilder;
        }

        /// <summary>
        /// register RabbitMQ Settings into di container to resolve between services.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="serviceLifetime"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static RabbitMQSettings AddRabbitMQConfigurations(this IServiceCollection services, IConfiguration configuration, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            RabbitMQSettings rabbitConfig = new RabbitMQSettings();
            configuration.Bind(nameof(RabbitMQSettings), rabbitConfig);
            services.AddSingleton(rabbitConfig);

            return rabbitConfig;
        }

        /// <summary>
        /// register CachingSettings into di container as singleton by default, to resolve in services or useing IOptions pattern.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="serviceLifetime"></param>
        /// <param name="cachingSettingsSection"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static CachingSettings RegisterCachingSettingConfigurations(this IServiceCollection services, IConfiguration configuration, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton, string cachingSettingsSection = "CachingSettings")
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            CachingSettings cachingSettings = new CachingSettings();
            configuration.Bind(nameof(CachingSettings), cachingSettings);

            switch (serviceLifetime)
            {
                case ServiceLifetime.Scoped:
                case ServiceLifetime.Transient:
                    services.AddOptions<CachingSettings>()
                        .Bind(configuration.GetSection(cachingSettingsSection))
                        .ValidateDataAnnotations();
                    break;
                case ServiceLifetime.Singleton:
                default:
                    services.AddSingleton(cachingSettings);
                    break;
            }

            return cachingSettings;
        }

        public static StorageSettings RegisterStorageSettingConfigurations(this IServiceCollection services, IConfiguration configuration, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton, string storageSettingsSection = "StorageSettings")
        {
            // Bind IDS settings to container for using with DI.
            StorageSettings storageSettings = new StorageSettings();
            configuration.Bind(storageSettingsSection, storageSettings);

            switch (serviceLifetime)
            {
                case ServiceLifetime.Scoped:
                case ServiceLifetime.Transient:
                    services.AddOptions<StorageSettings>()
                        .Bind(configuration.GetSection(storageSettingsSection))
                        .ValidateDataAnnotations();
                    break;
                case ServiceLifetime.Singleton:
                default:
                    services.AddSingleton(storageSettings);
                    break;
            }


            return storageSettings;
        }
        public static IdentityServerSettings RegisterIdentityServerSettingsConfigurations(this IServiceCollection services, IConfiguration configuration, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton, string identitySettingsSection = "IdentityServerSettings")
        {
            // Bind IDS settings to container for using with DI.
            IdentityServerSettings identityServerSettings = new IdentityServerSettings();
            configuration.Bind(identitySettingsSection, identityServerSettings);
            services.AddSingleton(identityServerSettings);

            return identityServerSettings;
        }

        /// <summary>
        /// register site / server environments and options like domain name, https and etc.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="serviceLifetime"></param>
        /// <param name="serverSettingsSection"></param>
        /// <returns></returns>
        public static ServerSettings RegisterServerSettingsConfigurations(this IServiceCollection services, IConfiguration configuration, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton, string serverSettingsSection = "ServerSettings")
        {
            ServerSettings serverSettings = new ServerSettings();
            configuration.Bind(serverSettingsSection, serverSettings);
            services.AddSingleton(serverSettings);

            return serverSettings;
        }
    }

}
