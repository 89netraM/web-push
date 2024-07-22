using System.Threading.Tasks;
using Lib.Net.Http.WebPush;
using Microsoft.Extensions.Logging;

namespace WebPushChat.Services;

public class PushService(
    ILogger<PushService> logger,
    VapidKeyService vapidKeyService,
    PushServiceClient pushServiceClient
)
{
    public async Task SendMessageAsync(PushSubscription subscription, string sender, string message)
    {
        var vapidKeys = vapidKeyService.VapidKeys;
        logger.LogInformation("Sending message to {Endpoint} from {Sender}", subscription.Endpoint, sender);
        await pushServiceClient.RequestPushMessageDeliveryAsync(
            subscription,
            new(
                $$"""
                {
                    "sender": "{{sender}}",
                    "message": "{{message}}"
                }
                """
            )
            {
                Urgency = PushMessageUrgency.High,
            },
            vapidKeys
        );
    }
}
