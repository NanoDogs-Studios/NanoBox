#if !DISABLESTEAMWORKS && HE_STEAMPLAYERSERVICES && HE_STEAMCOMPLETE && !UNITY_SERVER
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using Steamworks;
using System.Collections.Generic;

namespace HeathenEngineering.SteamAPI.Demo
{
    /// <summary>
    /// Controls the state and function of the Find Match button aka the Quick Search button.
    /// </summary>
    public class FindMatchButton : MonoBehaviour
    {
        [FormerlySerializedAs("QuickMatchFilter")]
        [FormerlySerializedAs("quickMatchFilter")]
        public LobbyQueryParameters matchFilter;
        [FormerlySerializedAs("quickMatchButton")]
        public Button matchButton;
        [FormerlySerializedAs("quickMatchLabel")]
        public Text matchLabel;

        private void Start()
        {
            MatchmakingTools.evtLobbyExit.AddListener(OnExitLobby);
            matchLabel.text = "Find Match";                
        }

        public void SimpleFindMatch()
        {
            if (!MatchmakingTools.InLobby)
            {
                Debug.Log("[FindMatchButton.SimpleFindMatch] Startimg a quickmatch search for a lobby that matches the filter defined in [FindMatchButton.quickMatchFilter].");
                matchLabel.text = "Searching";
                matchButton.interactable = false;
                MatchmakingTools.FindLobbies(matchFilter, (r, e) =>
                {
                    matchButton.interactable = true;
                    if (!e)
                    {
                        Debug.Log("Lobby query copleted with " + r.Count + " records found.");
                        if (r.Count > 0)
                        {
                            Debug.Log("Joining the first lobby found: " + r[0].name);
                            MatchmakingTools.JoinLobby(r[0].lobbyId);
                        }
                        else
                        {
                            Debug.Log("Creating a new lobby!");
                            MatchmakingTools.CreateLobby(ELobbyType.k_ELobbyTypePublic, 4, (cl, ie) =>
                            {
                                if (ie)
                                    Debug.LogError("Failed to create lobby, bIOFailure");
                                else
                                {
                                    matchLabel.text = "Leave Lobby";
                                    cl["name"] = SteamSettings.Client.user.DisplayName;

                                    foreach (var setting in matchFilter.stringValues)
                                    {
                                        cl[setting.key] = setting.value;
                                        Debug.Log("Metadata set: " + setting.key + " = " + setting.value);
                                    }
                                    foreach (var setting in matchFilter.numberValues)
                                    {
                                        cl[setting.key] = setting.value.ToString();
                                        Debug.Log("Metadata set: " + setting.key + " = " + setting.value);
                                    }
                                    foreach (var setting in matchFilter.nearValues)
                                    {
                                        cl[setting.key] = setting.value.ToString();
                                        Debug.Log("Metadata set: " + setting.key + " = " + setting.value);
                                    }

                                    Debug.Log("Entering lobby: " + cl.Name);
                                }
                            });
                        }
                    }
                    else
                    {
                        Debug.LogError("Failed to search for lobby, bIOFailure");
                    }
                });
            }
            else
            {
                MatchmakingTools.LeaveAllLobbies();
            }
        }

        public void GetHelp()
        {
            Application.OpenURL("https://partner.steamgames.com/doc/features/multiplayer/matchmaking");
        }

        public void OnExitLobby(Lobby lobby)
        {
            matchLabel.text = "Find Match";
            Debug.Log("Exiting lobby: " + lobby.Name);
        }
    }
}
#endif