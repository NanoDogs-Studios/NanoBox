using UnityEngine;

namespace Nanodogs.Nanobox.Mod
{
    /// <summary>
    /// Stores data for the author of a mod.
    /// </summary>
    /// <remarks>This is used in <see cref="nbMod"/> to reference an author.</remarks>
    [System.Serializable]
    public class ModAuthor
    {
        [Header("Mod Author's Information goes below")]

        [Header("The Author's name")]
        // the author's name
        public string AuthorName;

        [Header("All of the authors social links go here for credit purposes.")]
        // All of the authors social links go here for credit purposes.
        public string[] authorSocialLinks;

        [Header("This is the author's main link. for example, a Linktree or Sitee link would go here.")]
        // This is the author's main link. for example, a Linktree or Sitee link would go here.
        public string authorMainLink;
    }
}