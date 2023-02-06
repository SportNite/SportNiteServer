using SportNiteServer.Services;

namespace SportNiteServer.Entities;

public class Notification
{
    public Guid NotificationId { get; set; }

    public string Title { get; set; }
    public string Body { get; set; }
    public NotificationType Type { get; set; }
    public Guid UserId { get; set; }

    public enum NotificationType
    {
        NewResponse,
        MyResponseAccepted,
        MyResponseRejected,
        ResponseCancelled
    }

    public async Task Notify([Service ] UserService userService)
    {
        var user = await userService.GetUserById(UserId);
        
    }
}