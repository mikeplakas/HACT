//using HACT.Data;
//using HACT.Models;
//using System.Collections.Generic;

//namespace HACT.Services
//{
//    public interface IContactService
//    {
//        void Submit(ContactMessage message);
//        List<ContactMessage> GetAllMessages();
//        ContactMessage? GetById(int id);
//        void MarkAllAsRead(int id);
//        int GetUnreadCount();
//    }

//    public class ContactService : IContactService
//    {
//        private readonly IContactMessageRepository _repository;

//        public ContactService(IContactMessageRepository repository)
//        {
//            _repository = repository;
//        }

//        public void Submit(ContactMessage message)
//        {
//            _repository.SaveMessage(message);
//        }

//        public List<ContactMessage> GetAllMessages()
//        {
//            return _repository.GetAllMessages();
//        }

//        public ContactMessage? GetById(int id)
//        {
//            return _repository.GetById(id);
//        }

//        public void MarkAllAsRead(int id)
//        {
//            _repository.MarkAllAsRead(id);
//        }

//        public int GetUnreadCount()
//        {
//            return _repository.GetUnreadCount();
//        }
//    }
//}
