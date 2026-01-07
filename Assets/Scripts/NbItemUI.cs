using UnityEngine;

namespace Nanodogs.Nanobox.Core
{
    [CreateAssetMenu(fileName = "Map Item", menuName = "Nanodogs/Nanobox/Map Item")]
    public class NbMapItem : ScriptableObject
    {
        public string mapName;
        public Sprite mapImage;
    }
}