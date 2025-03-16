namespace Mottu.Api.Infrastructure.Notifications;

public class NotificationService : INotificationService
{
    private readonly List<Notification> _notifications = [];

    public List<Notification> Get()
    {
        return _notifications;
    }

    public Dictionary<string, string> GetDictionary()
    {
        return _notifications.ToDictionary(x => x.Key, x => x.Message);
    }

    public void Add(Notification notification)
    {
        _notifications.Add(notification);
    }

    public void AddRange(List<Notification> notifications)
    {
        
        _notifications.AddRange(notifications);
    }

    public bool HaveNotifications()
    {
        return _notifications.Any();
    }

    public string GetMessages(string separator = ", ")
    {
        if(!HaveNotifications())
        {
            return string.Empty;
        }

        return string.Join(separator, _notifications.Select(x => x.Message));
    }
}