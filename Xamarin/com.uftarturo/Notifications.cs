using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace com.uftarturo
{
    class Notifications
    {
        public static void Show(string title, string content, Activity t)
        {
            // Instantiate the builder and set notification elements:
            Notification.Builder builder = new Notification.Builder(t)
                .SetContentTitle(title)
                .SetContentText(content)
                .SetSmallIcon(Resource.Drawable.logo);
            builder.SetDefaults(NotificationDefaults.Sound | NotificationDefaults.Vibrate);
            // Build the notification:
            Notification notification = builder.Build();

            // Get the notification manager:
            NotificationManager notificationManager =
                t.GetSystemService(Context.NotificationService) as NotificationManager;

            // Publish the notification:
            const int notificationId = 0;
            notificationManager.Notify(notificationId, notification);
        }
    }
}