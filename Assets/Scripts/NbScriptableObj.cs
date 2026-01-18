using UnityEngine;
using UnityEngine.UI;

namespace Nanodogs.Nanobox.Core
{
    [CreateAssetMenu(fileName = "New NanoBox Scriptable Obj", menuName = "Nanodogs/Nanobox/NB Scriptable Obj")]
    public class NbScriptableObj : ScriptableObject
    {
        // Name of the object, should match prefab name for easy loading.
        public string ObjName;

        // no need to set this, will be generated.
        public Sprite icon;

        public NbMaterialType materialType;
        public string Tags;

        public NBCategory category;
        public enum NBCategory
        {
            None,
            Weapon,
            NPC,
            Vehicle
        }
    }
}