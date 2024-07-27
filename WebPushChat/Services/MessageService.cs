using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace WebPushChat.Services;

public class MessageService
{
    private static MessageService? instance;

    [JSInvokable("SendMessage")]
    public static void SendMessage(string jsonObject)
    {
        instance?.SendMessageInternal(jsonObject);
    }

    private readonly IServiceProvider serviceProvider;

    public event Action<string>? MessageReceived;

    public MessageService(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;

        instance = this;
    }

    private void SendMessageInternal(string jsonObject)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var peerService = scope.ServiceProvider.GetRequiredService<PeerService>();
            peerService.AddPeerFromMessage(jsonObject);
        }

        MessageReceived?.Invoke(jsonObject);
    }
}
