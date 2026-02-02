using Photon.Pun;
using System;
using UnityEngine;

namespace Nanodogs.Nanobox.Core
{
    public class NanoBoxPlayerSpawner : MonoBehaviourPunCallbacks
    {
        public NanoBoxNetworkManager NetworkManager;
        public NanoBoxGameManager GameManager;
        public PhotonView PlayerPrefab; // MUST BE PLACED in /Resources! No subfolder!!

        private void Awake()
        {
            NetworkManager = this.GetComponent<NanoBoxNetworkManager>();
            if (NetworkManager == null )
            { Debug.LogError("No Network Manager found!!! "); return; }

            GameManager = this.GetComponent<NanoBoxGameManager>();
            if (GameManager == null )
            { Debug.LogError("No Game Manager found!!!"); return; }

            if (PlayerPrefab.gameObject == null)
            {
                Resources.Load<GameObject>(PlayerPrefab.name);
            }
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.Instantiate(
                PlayerPrefab.name,
                GameManager.PickRandomNonOccupiedSpawn().transform.position,
                Quaternion.identity
            );
        }

        public override void OnLeftRoom()
        {

        }
    }
}
