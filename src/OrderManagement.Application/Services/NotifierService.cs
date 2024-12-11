using Microsoft.Extensions.DependencyInjection;

namespace OrderManagement.Application.Services
{
    //ESEMPIO SERVIZIO CHE UTILIZZA I KEYES SERVICE
    public class NotifierService
    {
        private readonly INotificationService _notificationServiceSms;
        private readonly INotificationService _notificationServiceEmail;
        private readonly INotificationService _notificationServicePush;
       // private readonly IKeyedServiceProvider _keyedServiceProvider;
        public NotifierService([FromKeyedServices("SMS")] INotificationService notificationServiceSms, [FromKeyedServices("EMAIL")] INotificationService notificationServiceEmail, [FromKeyedServices("PUSH")] INotificationService notificationServicePush)
        {
            _notificationServiceEmail = notificationServiceEmail;
            _notificationServiceSms = notificationServiceSms;
            _notificationServicePush = notificationServicePush;
            //_keyedServiceProvider = keyedServiceProvider;

        }

        public void Notify(string message)
        {
            _notificationServiceSms.Notify($"SMS Notification. Text {message}");
            _notificationServiceEmail.Notify($"Email Notification. Text {message}");
            _notificationServicePush.Notify($"Push Notification. Text {message}");


            /* AL MOMENTO NON FUNZIONA L'INIEZIONE DI IKeyedServiceProvider IN UN SERVIZIO CON LIFETIME SCOPED
            var notificationServiceSmsProvider = _keyedServiceProvider.GetRequiredKeyedService<INotificationService>("SMS");
            var notificationServiceEmailProvider = _keyedServiceProvider.GetRequiredKeyedService<INotificationService>("EMAIL");
            var notificationServicePushProvider = _keyedServiceProvider.GetRequiredKeyedService<INotificationService>("PUSH");

            notificationServiceSmsProvider.Notify($"SMS Notification. Text {message}");
            notificationServiceEmailProvider.Notify($"Email Notification. Text {message}");
            notificationServicePushProvider.Notify($"Push Notification. Text {message}");

            */

        }

    }
}
