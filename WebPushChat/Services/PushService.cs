using System.Threading.Tasks;
using Lib.Net.Http.WebPush;
using Microsoft.Extensions.Logging;
using WebPushChat.Models;

namespace WebPushChat.Services;

public class PushService(
    ILogger<PushService> logger,
    PushServiceClient pushServiceClient
)
{
    public async Task SendMessageAsync(PeerInfo peerInfo, string sender, string message)
    {
        logger.LogInformation("Sending message to {Endpoint} from {Sender}", peerInfo.PushSubscription.Endpoint, sender);
        await pushServiceClient.RequestPushMessageDeliveryAsync(
            peerInfo.PushSubscription,
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
            peerInfo.Vapid
        );
    }
}
