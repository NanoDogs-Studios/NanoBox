using Nanodogs.Nanobox.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Nanodogs.Nanobox.Map
{
    [System.Serializable]
    public class NbMap
    {
        // MUST be set.
        public NbMapItem mapItem;

        // the actual scene of the map.
        public NbScene mapScene;
    }
}