namespace OrderManagement.Application.Services
{
    public class SmsNotificationService : INotificationService
    {
        public string Notify(string message)
        {
            return $"SMS: {message}";
        }
    }

    public class EmailNotificationService : INotificationService
    {
        public string Notify(string message)
        {
            return $"Email: {message}";
        }
    }

    public class PushNotificationService : INotificationService
    {
        public string Notify(string message)
        {
            return $"Push: {message}";
        }
    }

}
