using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.Extensions.Logging;
using WebPushChat.Models;

namespace WebPushChat.Services;

public class PeerService(
    ILogger<PeerService> logger,
    ISyncLocalStorageService localStorage,
    PushService pushService,
    VapidKeyService vapidKeyService,
    SubscriptionService subscriptionService
)
{
    private const string PeersStorageKey = "peers";
    public static Guid PeerMessageGuid { get; } = new("2c56608a-9720-4a97-af48-21cc32ae0013");

    public Dictionary<string, PeerInfo> Peers =>
        localStorage.GetItem<Dictionary<string, PeerInfo>>(PeersStorageKey) ?? [];

    public async Task AddPeer(PeerInfo peerInfo)
    {
        var peers = Peers;
        peers[peerInfo.Name] = peerInfo;
        localStorage.SetItem(PeersStorageKey, peers);

        logger.LogInformation("Added peer {Name} to peers", peerInfo.Name);
        if (subscriptionService.Subscription is null)
        {
            logger.LogWarning("Subscription is null, cannot send peer info to {Name}", peerInfo.Name);
            return;
        }

        await pushService.SendMessageAsync(
            peerInfo,
            new PeerMessageModel(
                PeerMessageGuid,
                new(
                    vapidKeyService.VapidKeys,
                    subscriptionService.Subscription!,
                    localStorage.GetItem<string>("Name")!,
                    localStorage.GetItem<Guid>("Id")
                )
            )
        );
    }

    public void AddPeerFromMessage(string message)
    {
        try
        {
            var messageData = JsonSerializer.Deserialize<PeerMessageModel>(message);
            if (messageData?.SenderId != PeerMessageGuid)
            {
                return;
            }
            logger.LogInformation("Add peer from message: {Message}", message);

            var peers = Peers;
            peers[messageData.PeerInfo.Name] = messageData.PeerInfo;
            localStorage.SetItem(PeersStorageKey, peers);
        }
        catch { }
    }

    private record PeerMessageModel(Guid SenderId, PeerInfo PeerInfo);
}
