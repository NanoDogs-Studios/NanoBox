using UnityEngine;
using UnityEngine.SceneManagement;

namespace Nanodogs.Nanobox.Core
{
    [System.Serializable]
    public class NbMap
    {
        // MUST be set.
        public NbMapItem mapItem;

        // usually located at 0,0,0
        public Transform spawnPlatformLocation;

        // the actual scene of the map.
        public Scene map;
    }
}