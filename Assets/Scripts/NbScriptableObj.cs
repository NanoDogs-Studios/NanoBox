using UnityEngine;
using UnityEngine.UI;

namespace Nanodogs.Nanobox.Core
{
    public class NbScriptableObj : ScriptableObject
    {
        // Name of the object, should match prefab name for easy loading.
        public string ObjName;
        // no need make a sprite, there will be a rendered image.
        public NbMaterialType materialType;
        public string Tags;
    }
}