using System;

namespace Nanodogs.Nanobox.Mod
{
    [Serializable]
    public class ModVersion
    {
        public int Major;
        public int Minor;
        public int Patch;
        public ModVersion(int major, int minor, int patch)
        {
            Major = major;
            Minor = minor;
            Patch = patch;
        }
        public override string ToString()
        {
            return $"{Major}.{Minor}.{Patch}";
        }
    }
}