using System;

using Microsoft.JSInterop;

namespace WebPushChat.Services;

public class MessageService
{
    private static readonly Lazy<MessageService> instance = new(() => new MessageService());
    public static MessageService Instance => instance.Value;

    [JSInvokable("SendMessage")]
    public static void SendMessage(string jsonObject)
    {
        Instance.MessageReceived?.Invoke(jsonObject);
    }

    public event Action<string>? MessageReceived;
}
