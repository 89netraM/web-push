using System;
using Lib.Net.Http.WebPush;
using Lib.Net.Http.WebPush.Authentication;

namespace WebPushChat.Models;

public record PeerInfo(VapidAuthentication Vapid, PushSubscription PushSubscription, string Name, Guid Id);
