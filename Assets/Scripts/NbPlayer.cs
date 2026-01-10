using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Nanodogs.Nanobox.Core
{
    public class NbPlayer : MonoBehaviourPunCallbacks
    {
        public string username;
        
        // TODO: store player model data

        public PhotonView pv;
        public GameObject playerObject;
        public Player photonPlayer;
    }
}