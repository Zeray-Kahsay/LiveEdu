namespace API.Helpers;

public static class StripeEventTypes
{
    public const string PaymentIntentSucceeded = "payment_intent.succeeded";
    public const string PaymentIntentFailed = "payment_intent.payment_failed";
    public const string CheckoutSessionCompleted = "checkout.session.completed";

}
