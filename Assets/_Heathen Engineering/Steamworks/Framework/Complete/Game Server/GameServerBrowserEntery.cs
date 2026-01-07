#if !DISABLESTEAMWORKS && HE_STEAMPLAYERSERVICES && HE_STEAMCOMPLETE
#if MIRROR
using Mirror;
#endif
using Steamworks;
using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace HeathenEngineering.SteamAPI
{
    [Serializable]
    public class GameServerBrowserEntery : gameserveritem_t
    {
        public string IpAddress => SteamSettings.IPUintToString(m_NetAdr.GetIP());
        public ushort QueryPort => m_NetAdr.GetQueryPort();
        public ushort ConnectionPort => m_NetAdr.GetConnectionPort();
        public CSteamID SteamId => m_steamID;
        public AppId_t AppId => new AppId_t(m_nAppID);
        public bool UsesPassword => m_bPassword;
        public bool IsSecured => m_bSecure;
        public int PlayerCount => m_nPlayers;
        public int BotPlayerCount => m_nBotPlayers;
        public int MaxPlayerCount => m_nMaxPlayers;
        public int Ping => m_nPing;
        public int Version => m_nServerVersion;
        public DateTime LastPlayed => SteamSettings.ConvertUnixDate(m_ulTimeLastPlayed);
        public string Discription { get => GetGameDescription(); set => SetGameDescription(value); }
        public string Tags { get => GetGameTags(); set => SetGameTags(value); }
        public string Name { get => GetServerName(); set => SetServerName(value); }
        public string Map { get => GetMap(); set => SetMap(value); }
        public string Directory { get => GetGameDir(); set => SetGameDir(value); }

        public List<StringKeyValuePair> rules;
        public List<ServerPlayerEntry> players;

        public UnityEvent DataUpdated = new UnityEvent();

        public GameServerBrowserEntery(gameserveritem_t item)
        {
            DataUpdated = new UnityEvent();
            m_bDoNotRefresh = item.m_bDoNotRefresh;
            m_bHadSuccessfulResponse = item.m_bHadSuccessfulResponse;
            m_bPassword = item.m_bPassword;
            m_bSecure = item.m_bSecure;
            m_nAppID = item.m_nAppID;
            m_nBotPlayers = item.m_nBotPlayers;
            m_NetAdr = item.m_NetAdr;
            m_nMaxPlayers = item.m_nMaxPlayers;
            m_nPing = item.m_nPing;
            m_nPlayers = item.m_nPlayers;
            m_nServerVersion = item.m_nServerVersion;
            m_steamID = item.m_steamID;
            m_ulTimeLastPlayed = item.m_ulTimeLastPlayed;
            SetGameDescription(item.GetGameDescription());
            SetGameDir(item.GetGameDir());
            SetGameTags(item.GetGameTags());
            SetMap(item.GetMap());
            SetServerName(item.GetServerName());
            this.players = new List<ServerPlayerEntry>();
            this.rules = new List<StringKeyValuePair>();
        }

        public void Update(gameserveritem_t item)
        {
            m_bDoNotRefresh = item.m_bDoNotRefresh;
            m_bHadSuccessfulResponse = item.m_bHadSuccessfulResponse;
            m_bPassword = item.m_bPassword;
            m_bSecure = item.m_bSecure;
            m_nAppID = item.m_nAppID;
            m_nBotPlayers = item.m_nBotPlayers;
            m_NetAdr = item.m_NetAdr;
            m_nMaxPlayers = item.m_nMaxPlayers;
            m_nPing = item.m_nPing;
            m_nPlayers = item.m_nPlayers;
            m_nServerVersion = item.m_nServerVersion;
            m_steamID = item.m_steamID;
            m_ulTimeLastPlayed = item.m_ulTimeLastPlayed;
            SetGameDescription(item.GetGameDescription());
            SetGameDir(item.GetGameDir());
            SetGameTags(item.GetGameTags());
            SetMap(item.GetMap());
            SetServerName(item.GetServerName());

            DataUpdated.Invoke();
        }

#if MIRROR
        /// <summary>
        /// Join the indicated server if we are not already part of a server
        /// </summary>
        /// <returns></returns>
        public bool JoinServer()
        {
            if (!NetworkManager.singleton.isNetworkActive)
            {
                NetworkManager.singleton.networkAddress = SteamId.ToString();
                NetworkManager.singleton.StartClient();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Leave the current server e.g. Stop Client Networking
        /// </summary>
        public void LeaveServer()
        {
            NetworkManager.singleton.StopClient();
        }

        /// <summary>
        /// Switch to the indicated server i.e. leave the current server if any and join the indicated one
        /// </summary>
        public void SwitchServer()
        {
            if (NetworkManager.singleton.isNetworkActive)
                NetworkManager.singleton.StopClient();

            NetworkManager.singleton.networkAddress = SteamId.ToString();
            NetworkManager.singleton.StartClient();
        }
#endif
    }
}
#endif