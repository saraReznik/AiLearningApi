using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

// אם שם הפרויקט שלך הוא Server, ה-namespace צריך להיות Server
namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            // הקוד הזה מוודא שמסד הנתונים נוצר וכל המיגרציות מופעלות בהרצה
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    // שימוש בשם ה-DbContext הנכון שמצאנו
                    var context = services.GetRequiredService<Dal.Models.DatabaseManager>();

                    // [התיקון המרכזי]: שימוש ב-Migrate במקום EnsureCreated
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the DB.");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}