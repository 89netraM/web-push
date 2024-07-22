using System.Collections.Generic;
using Blazored.LocalStorage;
using WebPushChat.Models;

namespace WebPushChat.Services;

public class PeerService(ISyncLocalStorageService localStorage)
{
    private const string PeersStorageKey = "peers";

    public Dictionary<string, PeerInfo> Peers =>
        localStorage.GetItem<Dictionary<string, PeerInfo>>(PeersStorageKey) ?? [];

    public void AddPeer(string id, PeerInfo peerInfo)
    {
        var peers = Peers;
        peers[id] = peerInfo;
        localStorage.SetItem(PeersStorageKey, peers);
    }
}
