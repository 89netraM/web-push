using Blazored.LocalStorage;
using Lib.Net.Http.WebPush.Authentication;
using Microsoft.Extensions.Logging;
using WebPush;

namespace WebPushChat.Services;

public class VapidKeyService(ILogger<PushService> logger, ISyncLocalStorageService localStorage)
{
    private const string VapidKeysStorageKey = "vapidKeys";

    public VapidAuthentication VapidKeys =>
        localStorage.GetItem<VapidAuthentication>(VapidKeysStorageKey) ?? GenerateVapidKeys();

    private VapidAuthentication GenerateVapidKeys()
    {
        logger.LogInformation("Generating new VAPID keys");
        var vapidDetails = VapidHelper.GenerateVapidKeys();
        var keys = new VapidAuthentication(vapidDetails.PublicKey, vapidDetails.PrivateKey)
        {
            Subject = "mailto:marten.asberg@outlook.com",
        };
        localStorage.SetItem(VapidKeysStorageKey, keys);
        return keys;
    }
}
