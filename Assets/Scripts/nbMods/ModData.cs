using System.Collections.Generic;

namespace Nanodogs.Nanobox.Mod
{
    [System.Serializable]
    public class ModData
    {
        public List<Nanodogs.Nanobox.Map.NbMap> Maps = new List<Map.NbMap>();
        public List<Nanodogs.Nanobox.Core.NbPlayerModel> PlayerModels = new List<Core.NbPlayerModel>();
        public List<Nanodogs.Nanobox.Enemy.NbNextbot> Nextbots = new List<Enemy.NbNextbot>();
    }
}