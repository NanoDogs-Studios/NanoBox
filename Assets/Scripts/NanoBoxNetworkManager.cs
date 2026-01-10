using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

namespace Nanodogs.Nanobox.Core
{
    /// <summary>
    /// Provides network management functionality for NanoBox components within a Unity scene.
    /// </summary>
    /// <remarks>This class should be attached to a GameObject in the Unity Editor to enable network-related
    /// features for NanoBox. It is intended to be used as part of the NanoBox system and may interact with other
    /// NanoBox components.</remarks>
    public class NanoBoxNetworkManager : MonoBehaviour
    {
        /// <summary>
        /// the current room
        /// </summary>
        public NbRoom currentnbRoom;

        public void LeaveCurrentRoom()
        {
            if (!PhotonNetwork.IsConnected)
            {  return; }

            currentnbRoom = null;
            PhotonNetwork.LeaveRoom();
            Debug.Log("Left the current room.");
        }

        public void SetHost(NbPlayer player)
        {
            if(!PhotonNetwork.IsMasterClient && PhotonNetwork.IsConnected)
            { return; }

            currentnbRoom.host = player;
            currentnbRoom.photonRoom.SetMasterClient(player.photonPlayer);
            Debug.Log($"Set {player.username} as the new host.");
        }
    }

    /// <summary>
    /// Defines a NanoBox Room.
    /// 
    /// </summary>
    [System.Serializable]
    public class NbRoom
    {
        public string roomName;
        public NbPlayer host;
        public List<NbPlayer> currentPlayers = new();
        public NbRoomOptions roomOptions;
        public Room photonRoom;
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
        public enum RoomJoinType
        {
            Private, // practically a single-player room.
            Friends, // only friends can join
            Public // ANYONE with the name can join.
        }
    }
}
