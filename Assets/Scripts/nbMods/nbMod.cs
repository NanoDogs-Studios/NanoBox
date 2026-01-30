using System;
using UnityEngine;

namespace Nanodogs.Nanobox.Mod
{
    /// <summary>
    /// A mod in the Nanobox ecosystem.
    /// Mods can add maps, models, props, and more; theres no definition to what a mod can be.
    /// </summary>
    [Serializable]
    public class nbMod
    {
        // Mod metadata
        public string ModName;
        public ModVersion ModVersion;
        public string ModDescription;
        public ModAuthor ModAuthor;

        // Mod data
        public ModData data;

        public nbMod(string modName, ModVersion modVersion, string modDescription, ModAuthor modAuthor)
        {
            this.ModName = modName;
            this.ModVersion = modVersion;
            this.ModDescription = modDescription;
            this.ModAuthor = modAuthor;
            this.data = new ModData();
        }
    }
}
