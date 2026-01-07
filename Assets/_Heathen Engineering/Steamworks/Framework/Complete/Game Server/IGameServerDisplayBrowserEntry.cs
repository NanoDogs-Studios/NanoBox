#if !DISABLESTEAMWORKS && HE_STEAMPLAYERSERVICES && HE_STEAMCOMPLETE

namespace HeathenEngineering.SteamAPI
{
    public interface IGameServerDisplayBrowserEntry
    {
        void SetEntryRecord(GameServerBrowserEntery entry);
    }
}
#endif