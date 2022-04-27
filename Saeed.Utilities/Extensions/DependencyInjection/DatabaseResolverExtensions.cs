using System;
using System.Reflection;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Saeed.Utilities.DynamicSettings.Database;

namespace Saeed.Utilities.Extensions.DependencyInjection
{
    public class DatabaseResolverExtensions<TContext> where TContext : DbContext
    {

        /// <summary>
        /// configure db context and db providers based on DbConfig.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="dbConfig"></param>
        /// <param name="directory">path of local db file, if selected.</param>
        /// <returns></returns>
        public static IServiceCollection ConfigureApplicationDatabaseContext(IServiceCollection services, DatabaseSettings dbConfig, string directory = null)
        {
            #region Database selection

            services.AddDbContext<TContext>(options =>
            {
                if (dbConfig.EnableLogging)
                {
                    options.EnableDetailedErrors();
                    options.EnableSensitiveDataLogging();
                }

                if (dbConfig.UseLocalDb)
                {
                    options.UseSqlServer(dbConfig.ConnectionStrings.LocalDb.Replace("|DataDirectory|",
                        directory ?? Assembly.GetExecutingAssembly().Location),
                     opt =>
                     {
                         opt.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), null)
                         .MigrationsAssembly(typeof(TContext).Assembly.FullName)
                         .CommandTimeout(120);
                     });
                }
                else if (dbConfig.UseSqlite)
                {
                    options.UseSqlite(dbConfig.ConnectionStrings.SqLite,
                        opt =>
                        {
                            opt.MigrationsAssembly(typeof(TContext).Assembly.FullName)
                            .CommandTimeout(120);
                        });
                    options.ConfigureWarnings(x => x.Ignore(Microsoft.EntityFrameworkCore.Diagnostics
                                                            .RelationalEventId
                                                            .AmbientTransactionWarning));
                }
                else if (dbConfig.UseSqlServer)
                {
                    //if (dbConfig.EnableRetryOnFailure)
                    options.UseSqlServer(dbConfig.ConnectionStrings.SqlServer,
                        opt =>
                        {
                            opt.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), null);
                            opt.MigrationsAssembly(typeof(TContext).Assembly.FullName).CommandTimeout(120);
                        });
                }
                else if (dbConfig.UseInMemory)
                {
                    options.UseInMemoryDatabase(dbConfig.ConnectionStrings.InMemory);
                }
                else if (dbConfig.UseDocker) // use default
                {
                    //if (dbConfig.EnableRetryOnFailure)
                    options.UseSqlServer(dbConfig.ConnectionStrings.Docker,
                        opt =>
                        {
                            opt.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), null);
                            opt.MigrationsAssembly(typeof(TContext).Assembly.FullName)
                            .CommandTimeout(120);
                        });
                }
                else // use default
                {
                    //if (dbConfig.EnableRetryOnFailure)
                    options.UseSqlServer(dbConfig.ConnectionStrings.Default,
                    opt =>
                    {

                        opt.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), null);
                        opt.MigrationsAssembly(typeof(TContext).Assembly.FullName)
                        .CommandTimeout(120);
                    });
                }
            });

            #endregion

            return services;
        }
    }
}
