using UnityEditor;
using UnityEngine;

namespace Nanodogs.Nanobox.Mod.Editor
{
    [CreateAssetMenu(fileName = "NanoBoxModCreationWindow", menuName = "Nanodogs/Games/NanoBox/Mod/Create New")]
    public class NanoBoxModCreationWindow : EditorWindow
    {
        string ModName = "New NanoBox Mod";
        string ModVersion = "1.0.0";
        string ModDescription = "A description of the mod.";
        ModVersion modVersion;

        // author
        string AuthorName;
        string[] AuthorSocialLinks;
        string AuthorMainLink;

        ModAuthor author;

        private void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 300, 20), "NanoBox Mod Creation Window");

            ModName = NbExtEditorUtils.TextFieldWithLabel("The name of your mod", ModName);

            ModVersion = NbExtEditorUtils.TextFieldWithLabel("Mod Version (e.g. 1.0.0)", ModVersion);

            if (modVersion == null)
            {
                modVersion = new ModVersion();
            }
            modVersion.Major = EditorGUILayout.IntField("Version Major", modVersion.Major);
            modVersion.Minor = EditorGUILayout.IntField("Version Minor", modVersion.Minor);
            modVersion.Patch = EditorGUILayout.IntField("Version Patch", modVersion.Patch);

            ModDescription = NbExtEditorUtils.TextFieldWithLabel("A short description of your mod", ModDescription);

            // Author fields
            AuthorName = NbExtEditorUtils.TextFieldWithLabel("Author Name", AuthorName);
            AuthorMainLink = NbExtEditorUtils.TextFieldWithLabel("Author Main Link (e.g your linktree or sitee)", AuthorMainLink);

            // Social links input (comma separated)
            string socialLinksInput = NbExtEditorUtils.TextFieldWithLabel("Author Social Links (comma separated)", string.Join(", ", AuthorSocialLinks ?? new string[0]));
            AuthorSocialLinks = socialLinksInput.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

            author = new ModAuthor
            {
                AuthorName = AuthorName,
                AuthorMainLink = AuthorMainLink,
                AuthorSocialLinks = AuthorSocialLinks
            };

            if (GUI.Button(new Rect(10, 200, 150, 30), "Create Mod"))
            {
                Debug.Log($"Creating mod: {ModName} v{ModVersion} by {author.AuthorName}");

                nbMod mod = new nbMod(ModName, ModVersion, ModDescription, author);
            }
        }
    }
}
