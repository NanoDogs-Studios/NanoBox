using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Nanodogs.Nanobox.Core
{
    /// <summary>
    /// Represents a networkecomponentsd player in a Photon multiplayer game session, providing access to player-specific data and
    /// Photon networking components.
    /// </summary>
    /// <remarks>This class encapsulates references to the player's Photon networking objects and associated
    /// GameObject. It is typically used to manage player state and interactions within a multiplayer environment.
    /// Instances of this class are created and managed by the game logic to synchronize player data across the
    /// network.</remarks>
    public class NbPlayer : MonoBehaviourPunCallbacks
    {
        public string username;

        /// <summary>
        /// Gets a NbPlayer's username.
        /// </summary>
        /// <returns></returns>
        public string GetUsername()
        { return username; }
        
        // TODO: store player model data

        public PhotonView pv;
        public GameObject playerObject;
        public Player photonPlayer;
        public int actorNumber;
        public bool isLocal;

        public static NbPlayer CreateLocal(Player photonPlayer)
        {
            return new NbPlayer
            {
                username = photonPlayer.NickName,
                photonPlayer = photonPlayer,
                actorNumber = photonPlayer.ActorNumber,
                isLocal = true
            };
        }


        public static NbPlayer FromPhotonPlayer(Player p)
        {
            return new NbPlayer
            {
                photonPlayer = p,
                username = p.NickName
            };
        }
    }
}