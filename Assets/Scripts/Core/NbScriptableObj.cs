using UnityEngine;
using UnityEngine.UI;

namespace Nanodogs.Nanobox.Core
{
    /// <summary>
    /// Represents a base ScriptableObject for NanoBox assets, providing common metadata such as name, icon, material
    /// type, tags, and category.
    /// </summary>
    /// <remarks>Use this type to define and organize NanoBox-related assets in the Unity editor. The object
    /// name should match the corresponding prefab name to facilitate asset loading. This class is intended to be
    /// extended or instantiated as assets via the Unity Create Asset Menu.</remarks>
    [CreateAssetMenu(fileName = "New NanoBox Scriptable Obj", menuName = "Nanodogs/Nanobox/NB Scriptable Obj")]
    public class NbScriptableObj : ScriptableObject
    {
        // Name of the object, should match prefab name for easy loading.
        public string ObjName;

        // the icon of the object, appears in the spawn menu.
        // no need to set this, will be generated.
        public Sprite icon;

        // the material type.
        public NbMaterialType materialType;

        // the tags of this object, used for organization.
        public string[] Tags;

        /// The category for this scriptable obj
        public NBCategory category;

        /// <summary>
        /// Specifies the category type for objects such as weapons, non-player characters (NPCs), and vehicles.
        /// </summary>
        /// <remarks>Use this enumeration to classify objects according to their functional role within
        /// the application. The values can be used for filtering, grouping, or applying category-specific
        /// logic.</remarks>
        public enum NBCategory
        {
            None,
            Weapon,
            NPC,
            Vehicle
        }
    }
}