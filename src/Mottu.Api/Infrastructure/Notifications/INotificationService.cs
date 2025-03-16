namespace Mottu.Api.Infrastructure.Notifications;

public interface INotificationService
{
    List<Notification> Get();
    Dictionary<string, string> GetDictionary();
    string GetMessages(string separator = ", ");
    void Add(Notification notification);
    void AddRange(List<Notification> notifications);
    bool HaveNotifications();
}