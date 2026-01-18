using Nanodogs.Nanobox.UI;
using Photon.Pun;
using UnityEngine;

namespace Nanodogs.Nanobox.Core
{
    /// <summary>
    /// Represents a generic object in the Nanobox system.
    /// </summary>
    [System.Serializable]
    public class NbObj
    {
        // The scriptable object associated with this NbObj
        public NbScriptableObj NanoBoxScriptableObject;

        // must be placed in Resources; also acts as path if in folders (/Resources/Objects/)
        public string prefabName;

        public void Spawn(Vector3 position, Quaternion rotation)
        {
            PhotonNetwork.Instantiate(prefabName, position, rotation);
        }

        /// <summary>
        /// Generates and returns a new Sprite instance for rendering purposes.
        /// </summary>
        /// <returns>A Sprite object representing the generated render. The returned Sprite may be used for display or further
        /// manipulation.</returns>
        public Sprite GenerateRender()
        {
            if (NanoBoxScriptableObject.icon != null)
                return NanoBoxScriptableObject.icon;

            NanoBoxScriptableObject.icon =
                NbIconRenderer.Instance.Render(
                    Resources.Load<GameObject>(prefabName)
                );

            return NanoBoxScriptableObject.icon;
        }
    }
}