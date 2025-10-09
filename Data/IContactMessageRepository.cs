using HACT.Models;

public interface IContactMessageRepository
{
    void SaveMessage(ContactMessage message);
    List<ContactMessage> GetAllMessages();
    ContactMessage? GetById(int id);
    void MarkAllAsRead(int id);
    int GetUnreadCount();

}
