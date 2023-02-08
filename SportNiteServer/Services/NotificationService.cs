using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.EntityFrameworkCore;
using SportNiteServer.Database;
using SportNiteServer.Entities;
using Notification = SportNiteServer.Entities.Notification;
using Path = System.IO.Path;

namespace SportNiteServer.Services;

public class NotificationService
{
    private readonly DatabaseContext _databaseContext;

    public NotificationService(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
        if (FirebaseApp.DefaultInstance == null)
            FirebaseApp.Create(new AppOptions
            {
                Credential =
                    GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                        "/secret/fcm-key.json")),
            });
    }

    public async Task Push(Notification notification)
    {
        var user = await _databaseContext.Users.FirstAsync(x => x.UserId == notification.UserId);
        notification.UserId = user.UserId;
        _databaseContext.Notifications.Add(notification);
        await _databaseContext.SaveChangesAsync();

        foreach (var device in await _databaseContext.Devices.Where(x => x.UserId == user.UserId).ToListAsync())
        {
            var message = new Message
            {
                Android = new AndroidConfig
                {
                    Notification = new AndroidNotification
                    {
                        Title = notification.Title,
                        Body = notification.Body,
                        ChannelId = notification.Type.ToString()
                    },
                },
                Data = new Dictionary<string, string>
                {
                    {
                        "TYPE", notification.Type.ToString()
                    }
                },
                Token = device.Token
            };
            var messaging = FirebaseMessaging.DefaultInstance;
            var result = await messaging.SendAsync(message);
            Console.Write(result);
        }
    }

    public IEnumerable<Notification> GetNotifications(User user)
        => _databaseContext.Notifications.Where(n => n.UserId == user.UserId).OrderByDescending(n => n.DateTime);
}