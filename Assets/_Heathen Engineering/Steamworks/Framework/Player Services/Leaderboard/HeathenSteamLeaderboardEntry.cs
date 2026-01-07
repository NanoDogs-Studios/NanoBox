#if !DISABLESTEAMWORKS && HE_STEAMPLAYERSERVICES
using HeathenEngineering.Tools;
using Steamworks;

namespace HeathenEngineering.SteamAPI
{
    /// <summary>
    /// Base class used by HeathenSteamLeaderboard to represent leaderboard entries
    /// Derive from this class and override the ApplyEntry method to create a custom entry record
    /// </summary>
    public class HeathenSteamLeaderboardEntry : HeathenUIBehaviour
    {
        public virtual void ApplyEntry(ExtendedLeaderboardEntry entry)
        { }
    }
}
#endif
