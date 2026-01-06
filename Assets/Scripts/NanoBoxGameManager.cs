using Photon.Pun;
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
        }

        #region Public API

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
