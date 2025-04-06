namespace EmailService
{
    public static class Constants
    {
        public const string FromEmail = "v007.learnmail@gmail.com";
        public const string FromName = "Venkatesh Kandregula";
        public const string Password = "swwy rqlt nqbc voeb";
        public const string OrderPlacedSucessMailBody = "OrderPlacedSucessMailBody";
        public const string OrderPlacedSucessMailMapping = "OrderPlacedSucessMailMapping";
        public const string OrderPlacedSucessMailSubject = "OrderPlacedSucessMailSubject";
        public static readonly List<string> OrderStatusRoutingKeys = new List<string> { "order.updated", "order.failed" };
    }
}
