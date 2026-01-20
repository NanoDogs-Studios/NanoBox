using UnityEngine;

namespace Nanodogs.Nanobox.Mod
{
    /// <summary>
    /// A mod in the Nanobox ecosystem.
    /// </summary>
    public class nbMod
    {
        // Mod metadata
        public string ModName { get; private set; }
        public string ModVersion { get; private set; } = "1.0";
        public string ModDescription { get; private set; }
        public ModAuthor ModAuthor { get; private set; }

        // Mod data
    }
}
