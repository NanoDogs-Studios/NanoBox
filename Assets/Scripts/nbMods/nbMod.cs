using System;
using UnityEngine;

namespace Nanodogs.Nanobox.Mod
{
    /// <summary>
    /// A mod in the Nanobox ecosystem.
    /// Mods can add maps, models, props, and more; theres no definition to what a mod can be.
    /// </summary>
    public class nbMod
    {
        // Mod metadata
        public string ModName { get; private set; }
        public ModVersion ModVersion { get; private set; }
        public string ModDescription { get; private set; }
        public ModAuthor ModAuthor { get; private set; }

        // Mod data
        public ModData data;

        public nbMod(string modName, ModVersion modVersion, string modDescription, ModAuthor modAuthor)
        {
            ModName = modName;
            ModVersion = modVersion;
            ModDescription = modDescription;
            ModAuthor = modAuthor;
            data = new ModData();
        }
    }
}
