using System.Collections.Generic;
using Blazored.LocalStorage;
using WebPushChat.Models;

namespace WebPushChat.Services;

public class PeerService(ISyncLocalStorageService localStorage)
{
    private const string PeersStorageKey = "peers";

    public Dictionary<string, PeerInfo> Peers =>
        localStorage.GetItem<Dictionary<string, PeerInfo>>(PeersStorageKey) ?? [];

    public void AddPeer(PeerInfo peerInfo)
    {
        var peers = Peers;
        peers[peerInfo.Name] = peerInfo;
        localStorage.SetItem(PeersStorageKey, peers);
    }
}
