using System.Threading.Tasks;
using Blazored.LocalStorage;
using Lib.Net.Http.WebPush;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace WebPushChat.Services;

public class SubscriptionService(
    ILogger<SubscriptionService> logger,
    ISyncLocalStorageService localStorage,
    VapidKeyService vapidKeyService,
    IJSRuntime jsRuntime
)
{
    private const string SubscriptionStorageKey = "subscription";

    public PushSubscription? Subscription
    {
        get => localStorage.GetItem<PushSubscription>(SubscriptionStorageKey);
        set => localStorage.SetItem(SubscriptionStorageKey, value);
    }

    public async Task<PushSubscription?> RegisterSubscription()
    {
        var publicKey = vapidKeyService.VapidKeys.PublicKey;
        logger.LogInformation("Registering subscription");
        var subscription = await jsRuntime.InvokeAsync<PushSubscription>("registerSubscription", publicKey);
        logger.LogInformation("Subscription registered");
        Subscription = subscription;
        return subscription;
    }
}
