namespace Saeed.Utilities.DynamicSettings.RabbitMq
{
    /// <summary>
    /// RabbitMQ dynamic configurations which is bound to appsettings config.
    /// properties name must be same as appsettings.json -> RabbitMQSettings Section -> XXXServiceQueue
    /// </summary>
    public class RabbitMQSettings
    {
        // rabbit conf
        public string RabbitMqRootUri { get; set; } = "rabbitmq://localhost/";
        public string RabbitMqUri { get; set; } = "rabbitmq://localhost/queue/";
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";

        // message queues
        public string SmsServiceQueue { get; set; } = "sms.service.queue";
        public string OrderServiceQueue { get; set; } = "order.service.queue";
        public string ShoppingCartServiceQueue { get; set; } = "shoppingCart.service.queue";
        public string EmailServiceQueue { get; set; } = "email.service.queue";
        public string AuthServiceQueue { get; set; } = "auth.service.queue";
        public string StorageServiceQueue { get; set; } = "storage.service.queue";
        public string NotificationServiceQueue { get; set; } = "notification.service.queue";
        public string AlbumServiceQueue { get; set; } = "album.service.queue";
        public string ChatServiceQueue { get; set; } = "chat.service.queue";
        public string TicketServiceQueue { get; set; } = "ticket.service.queue";
        public string ServiceProviderServiceQueue { get; set; } = "serviceprovider.service.queue";
        public string DiscountQueue { get; set; } = "discount.service.queue";
        public string PaymentQueue { get; set; } = "payment.service.queue";
        public string CountryQueue { get; set; } = "country.service.queue";
        public string CategoryQueue { get; set; } = "category.service.queue";
        public string HomeQueue { get; set; } = "home.service.queue";
        public string AdvertisementQueue { get; set; } = "advertisement.service.queue";
        public string ProjectServiceQueue { get; set; } = "project.service.queue";
    }
}
