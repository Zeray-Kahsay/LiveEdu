namespace API.Services;

public class StripeSettings
{
    public string PublishableKey { get; set; } = default!;
    public string SecretKey { get; set; } = default!;
    public string WebhookSecret { get; set; } = default!;
}
