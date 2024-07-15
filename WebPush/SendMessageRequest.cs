using Lib.Net.Http.WebPush;

namespace WebPush;

public record SendMessageRequest(string Sender, string Message, PushSubscription Subscription);
