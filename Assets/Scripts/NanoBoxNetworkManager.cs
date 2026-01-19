using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Nanodogs.Nanobox.Core
{
    /// <summary>
    /// Provides network management functionality for NanoBox components within a Unity scene.
    /// </summary>
    /// <remarks>This class should be attached to a GameObject in the Unity Editor to enable network-related
    /// features for NanoBox. It is intended to be used as part of the NanoBox system and may interact with other
    /// NanoBox components.</remarks>
    public class NanoBoxNetworkManager : MonoBehaviourPunCallbacks
    {
        private GameObject LocalPlayerObj => NanoBoxGameManager.Instance.GetLocalPlayerGameObject();
        private NbPlayer LocalPlayer => NanoBoxGameManager.Instance.GetLocalPlayer();

        [SerializeField] bool testing = false;

        public UnityEvent onConnectedToPhoton;
        public UnityEvent onDisconnectedToPhoton;

        public UnityEvent onLobbyJoined;
        public UnityEvent onLobbyLeft;

        public UnityEvent onRoomJoined;
        public UnityEvent onRoomLeft;
        public UnityEvent onRoomCreated;

        public UnityEvent<NbPlayer> onPlayerJoined;
        public UnityEvent<NbPlayer> onPlayerLeft;

        private void Start()
        {
            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.NickName = "Connecting...";
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to Photon Master");

            onConnectedToPhoton.Invoke();

            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("Joined Lobby");

            onLobbyJoined.Invoke();

            if (!testing) return;

            NbRoom testRoom = new()
            {
                roomName = "test",
                roomOptions = new NbRoomOptions
                {
                    maxPlayers = 4,
                    joinType = NbRoomOptions.RoomJoinType.Friends
                }
            };

            CreateRoom(testRoom);
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.LogError($"CreateRoom failed: {message} ({returnCode})");
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.LogError($"JoinRoom failed: {message} ({returnCode})");
        }


        /// <summary>
        /// the current room
        /// </summary>
        public NbRoom currentnbRoom;

        /// <summary>
        /// Gets the current room information for the room the player is currently in.
        /// </summary>
        /// <returns>The <see cref="NbRoom"/> instance representing the current room. Returns <c>null</c> if no room is active.</returns>
        public NbRoom GetCurrentNbRoom()
        { return currentnbRoom; }

        public void JoinRoom(NbRoom room)
        {
            if (!PhotonNetwork.IsConnected)
            {
                Debug.LogWarning("Not connected to Photon.");
                return;
            }

            PhotonNetwork.JoinRoom(room.roomName);
        }

        public void CreateRoom(NbRoom room)
        {
            var options = new RoomOptions
            {
                MaxPlayers = (byte)room.roomOptions.maxPlayers,
                IsVisible = room.roomOptions.joinType == NbRoomOptions.RoomJoinType.Public,
                IsOpen = true,
                CustomRoomProperties = new ExitGames.Client.Photon.Hashtable
            {
            { "joinType", (int)room.roomOptions.joinType }
            },
                CustomRoomPropertiesForLobby = new[] { "joinType" }
            };

            PhotonNetwork.CreateRoom(room.roomName, options);
        }

        public void JoinOrCreateRoom(NbRoom room)
        {
            var options = new RoomOptions
            {
                MaxPlayers = (byte)room.roomOptions.maxPlayers,
                IsVisible = room.roomOptions.joinType == NbRoomOptions.RoomJoinType.Public,
                IsOpen = true
            };

            PhotonNetwork.JoinOrCreateRoom(room.roomName, options, TypedLobby.Default);
        }

        public void RequestHost(NbPlayer player)
        {
            if (!PhotonNetwork.IsMasterClient) return;
            PhotonNetwork.SetMasterClient(player.photonPlayer);
        }

        public void LeaveCurrentRoom()
        {
            if (!PhotonNetwork.IsConnected)
            {  return; }

            currentnbRoom = null;
            PhotonNetwork.LeaveRoom();
            Debug.Log("Left the current room.");
        }

        private void RebuildPlayerList()
        {
            currentnbRoom.currentPlayers.Clear();

            foreach (var photonPlayer in PhotonNetwork.PlayerList)
            {
                var nbPlayer = NbPlayer.FromPhotonPlayer(photonPlayer);
                currentnbRoom.currentPlayers.Add(nbPlayer);
            }
        }

        #region Callbacks

        public override void OnCreatedRoom()
        {
            Debug.Log("Room successfully created.");

            onRoomCreated.Invoke();
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Joined Photon room.");
            onRoomJoined.Invoke();

            // Create local NbPlayer FROM Photon
            var localPhotonPlayer = PhotonNetwork.LocalPlayer;

            var nbPlayer = NbPlayer.CreateLocal(localPhotonPlayer);

            NanoBoxGameManager.Instance.RegisterLocalPlayer(nbPlayer);

            currentnbRoom = new NbRoom
            {
                roomName = PhotonNetwork.CurrentRoom.Name,
                photonRoom = PhotonNetwork.CurrentRoom,
                currentPlayers = new List<NbPlayer>()
            };

            RebuildPlayerList();

            if (PhotonNetwork.IsMasterClient)
            {
                currentnbRoom.host = nbPlayer;
            }
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            RebuildPlayerList();
            currentnbRoom.OnPlayerJoined(
                NbPlayer.FromPhotonPlayer(newPlayer)
            );

            onPlayerJoined.Invoke(NbPlayer.FromPhotonPlayer(newPlayer));
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            RebuildPlayerList();

            onPlayerLeft.Invoke(NbPlayer.FromPhotonPlayer(otherPlayer));
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            currentnbRoom.host = NbPlayer.FromPhotonPlayer(newMasterClient);
            Debug.Log($"{currentnbRoom.host.username} is now host.");
        }
        #endregion


    }

    /// <summary>
    /// Defines a NanoBox Room.
    /// </summary>
    [System.Serializable]
    public class NbRoom
    {
        public string roomName;

        // The host of the room.
        public NbPlayer host;

        // all players in the room.
        public List<NbPlayer> currentPlayers = new();

        public NbRoomOptions roomOptions;

        public Room photonRoom;

        public virtual void OnPlayerJoined(NbPlayer player)
        {
            Debug.Log($"{player.username} joined {roomName}");
        }
    }

    /// <summary>
    /// Represents configuration options for creating or joining a networked room, including player limits and access
    /// type.
    /// </summary>
    /// <remarks>Use this class to specify settings such as the maximum number of players allowed and the join
    /// policy when initializing a room. The options defined here determine how the room is discovered and who can
    /// participate.</remarks>
    [System.Serializable]
    public class NbRoomOptions
    {
        public int maxPlayers;

        public RoomJoinType joinType;

        public enum RoomJoinType
        {
            Private, // practically a single-player room.
            Friends, // only friends can join
            Public // ANYONE with the name can join.
        }
    }
}
