namespace App.DataLayer.DbContext
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public static class MigrationManager
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using IServiceScope scope = host.Services.CreateScope();
            using DefaultDbContext defaultDbContext = scope.ServiceProvider.GetRequiredService<DefaultDbContext>();
            try
            {
                defaultDbContext.Database.Migrate();
            }
            catch (Exception ex)
            {
                throw;
            }

            return host;
        }
    }
}
