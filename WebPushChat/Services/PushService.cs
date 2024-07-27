using System;
using System.Text.Json;
using System.Threading.Tasks;
using Lib.Net.Http.WebPush;
using Microsoft.Extensions.Logging;
using WebPushChat.Models;

namespace WebPushChat.Services;

public class PushService(ILogger<PushService> logger, PushServiceClient pushServiceClient)
{
    public async Task SendMessageAsync<T>(PeerInfo peerInfo, T message)
    {
        logger.LogInformation("Sending message to {Endpoint}", peerInfo.PushSubscription.Endpoint);
        await pushServiceClient.RequestPushMessageDeliveryAsync(
            peerInfo.PushSubscription,
            new(JsonSerializer.Serialize(message)) { Urgency = PushMessageUrgency.High, },
            peerInfo.Vapid
        );
    }
}
