using CaloriePlannerApp.Data;
using CaloriePlannerApp.Data.Helper;
using CaloriePlannerApp.Data.Model;
using CommunityToolkit.Maui;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System.Diagnostics;

namespace CaloriePlannerApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        var builder = MauiApp.CreateBuilder();
		builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

        builder.Services.AddDbContext<DataContext>();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetService<DataContext>();

            if (context != null)
            {
                context.Database.Migrate();
                var shouldSeed = !context.Foods.Any(x => x.IsCustomFood == false);
                
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/Seed/food.xlsx");

                if (File.Exists(filePath) && shouldSeed)
                {
                    var foodSeed = ExcelHelper.ReadFoodData(filePath);
                    context.Foods.AddRange(foodSeed);
                    context.SaveChanges();
                }
            }
        }

        return app;
    }
}
