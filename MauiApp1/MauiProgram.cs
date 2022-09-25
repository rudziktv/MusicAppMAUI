using Android.Content;
using CommunityToolkit.Maui;
using Plugin.LocalNotification;

namespace MauiApp1;

public static class MauiProgram
{
	public static Context context = Android.App.Application.Context;
	public static MauiApp CreateMauiApp()
	{
        var builder = MauiApp.CreateBuilder();

		LocalNotificationCenter.CreateNotificationChannel(
			new Plugin.LocalNotification.AndroidOption.NotificationChannelRequest
			{
				Id = "sample_notify"
			});



        // Initialise the toolkit
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("Rubik-ExtraBold.ttf", "RubikExtraBold");
			})
			.UseLocalNotification(config =>
			{
				config.AddCategory(new NotificationCategory(NotificationCategoryType.Status)
				{
					ActionList = new HashSet<NotificationAction>(new List<NotificationAction>())
					{
						new NotificationAction(100)
						{
							Title = "Play/Pause",
							Android =
							{
								LaunchAppWhenTapped = false,
                            }
						},
                        new NotificationAction(101)
                        {
                            Title = "Repeat",
                            Android =
                            {
                                LaunchAppWhenTapped = false,
                            }
                        }
                    }
				});
			});
        builder.UseMauiApp<App>().UseMauiCommunityToolkit();

        return builder.Build();
	}
}
