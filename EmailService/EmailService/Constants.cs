namespace EmailService
{
    public static class Constants
    {
        public const string FromEmail = "v007.learnmail@gmail.com";
        public const string FromName = "ZMart";
        public const string Password = "swwy rqlt nqbc voeb";
        public const string OrderPlacedSucessMailBody = "OrderPlacedSucessMailBody";
        public const string OrderPlacedSucessMailMapping = "OrderPlacedSucessMailMapping";
        public const string OrderPlacedSucessMailSubject = "OrderPlacedSucessMailSubject";

        public const string PaymentSucessMailBody = "PaymentSucessMailBody";
        public const string PaymentSucessMailMapping = "PaymentSucessMailMapping";
        public const string PaymentSucessMailSubject = "PaymentSucessMailSubject";

        public const string PaymentFailedMailBody = "PaymentFailedMailBody";
        public const string PaymentFailedMailMapping = "PaymentFailedMailMapping";
        public const string PaymentFailedMailSubject = "PaymentFailedMailSubject";

        public static readonly List<string> OrderStatusRoutingKeys = new List<string> { "order.updated" };
        public static readonly List<string> PaymentStatusRoutingKeys = new List<string> { "payment.sucess", "payment.failed" };
    }
}
