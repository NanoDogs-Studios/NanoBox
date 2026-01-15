using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

namespace Nanodogs.Nanobox.Core
{
    /// <summary>
    /// Manages the core gameplay logic and state for NanoBox.
    /// </summary>
    /// <remarks>This class should be attached to a GameObject in the Unity scene to coordinate game flow,
    /// manage game state, and interact with other game systems. It serves as the central point for NanoBox game
    /// management within the Unity engine.</remarks>
    public class NanoBoxGameManager : MonoBehaviourPunCallbacks
    {
        private GameObject localPlayer;
        private List<GameObject> players = new List<GameObject>();

        GameObject[] spawns;

        // Singleton instance
        public static NanoBoxGameManager Instance { get; private set; }

        private void Awake()
        {
            // Ensure only one instance of the game manager exists
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            // provides this to nanoboxnetworkmanager
            spawns = GameObject.FindGameObjectsWithTag("Spawn");
        }

        #region Public API

        /// <summary>
        /// Gets and returns the local player's gameobject.
        /// </summary>
        /// <returns>A Player <see cref="GameObject"></see></returns>
        public GameObject GetLocalPlayerGameObject()
        { return localPlayer; }

        /// <summary>
        /// Gets the local player instance associated with the current context.
        /// </summary>
        /// <returns>The <see cref="NbPlayer"/> component representing the local player, or <see langword="null"/> if no such
        /// component is found.</returns>
        public NbPlayer GetLocalPlayer()
        { return localPlayer.GetComponent<NbPlayer>(); }

        /// <summary>
        /// returns the player that is on that view id.
        /// </summary>
        /// <param name="viewId">A per-room id that means their index. host is usually 0.</param>
        /// <returns>A Player <see cref="GameObject"></see></returns>
        public GameObject GetPlayerGameObjectByViewId(int viewId)
        { return players[viewId]; }

        /// <summary>
        /// Returns an array of spawn point game objects used for player or entity placement in the game.
        /// </summary>
        /// <returns>An array of <see cref="GameObject"/> instances representing all available spawn points. The array will be
        /// empty if no spawn points are defined.</returns>
        public GameObject[] GetSpawnpoints()
        { return spawns; }


        public static void SpawnObject(NbObj obj, Vector3 position, Quaternion rotation)
        {
            if (obj == null || obj.NanoBoxScriptableObject == null)
            {
                Debug.LogError("Invalid NbObj or NbScriptableObj provided for spawning.");
                return;
            }

            if (PhotonNetwork.IsConnected == false)
            {
                Debug.LogError("Photon Network is not connected. Cannot spawn object.");
                return;
            }

            if (string.IsNullOrEmpty(obj.NanoBoxScriptableObject.ObjName))
            {
                Debug.LogError("NbScriptableObj has an invalid ObjName. Cannot spawn object.");
                return;
            }

            if(Resources.Load<GameObject>(obj.NanoBoxScriptableObject.ObjName) == null)
            {
                Debug.LogError($"Prefab '{obj.NanoBoxScriptableObject.ObjName}' not found in Resources. Cannot spawn object.");
                return;
            }

            // Implementation for spawning an object in the game world
            Debug.Log($"Spawning object: {obj.NanoBoxScriptableObject.ObjName} at position {position}");

            PhotonNetwork.Instantiate(
                obj.NanoBoxScriptableObject.ObjName,
                position,
                rotation,
                0,
                new object[] { obj.NanoBoxScriptableObject.ObjName }
            );
        }

        /// <summary>
        /// Destroys the specified game object across the network using Photon. This ensures that the object is removed
        /// for all connected players.
        /// </summary>
        /// <remarks>This method requires an active Photon Network connection. If the network is not
        /// connected or if the object is null, the method will not perform any action and will log an error. Only
        /// objects instantiated via Photon Network should be destroyed using this method.</remarks>
        /// <param name="obj">The game object to be destroyed. Must not be null and must be managed by Photon Network.</param>
        public static void DestroyObject(GameObject obj)
        {
            if (obj == null)
            {
                Debug.LogError("Cannot destroy a null object.");
                return;
            }
            if (PhotonNetwork.IsConnected == false)
            {
                Debug.LogError("Photon Network is not connected. Cannot destroy object.");
                return;
            }

            // Implementation for destroying an object in the game world
            Debug.Log($"Destroying object: {obj.name}");
            PhotonNetwork.Destroy(obj);
        }

        /// <summary>
        /// Sets whether the specified player is allowed to fly.
        /// </summary>
        /// <param name="player">The player GameObject whose flying ability will be updated. Cannot be null.</param>
        /// <param name="canFly">A value indicating whether the player should be able to fly. Set to <see langword="true"></see> to enable flying; otherwise, <see langword="false"></see>.</param>
        public static void SetPlayerFlyState(GameObject player, bool canFly)
        {
            if (player == null)
            {
                Debug.LogError("Player GameObject is null. Cannot set fly state.");
                return;
            }
            // Implementation for setting player's fly state
            Debug.Log($"Setting player '{player.name}' fly state to: {canFly}");
            // disable for now
            //var flyComponent = player.GetComponent<PlayerFlyComponent>();
            //if (flyComponent != null)
            //{
            //flyComponent.SetCanFly(canFly);
            //}
            //else
            //{
            //Debug.LogError("PlayerFlyComponent not found on the player GameObject.");
            //}
        }

        #endregion
    }
}
