namespace App
{
    using DataLayer.DbContext;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using Sentry.Extensibility;

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().MigrateDatabase().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseSentry(o =>
                    {
                        o.Debug = true;
                        o.MaxRequestBodySize = RequestSize.Always;
                        o.Dsn = "https://85dcb29cfd0749989cd42e21921163df@sentry.io/5167676";
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
