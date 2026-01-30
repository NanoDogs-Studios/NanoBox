using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nanodogs.Nanobox.Mod
{
    public class NanoBoxModCreationWindow : EditorWindow
    {
        private TextField modNameField;
        private IntegerField majorField;
        private IntegerField minorField;
        private IntegerField patchField;
        private TextField descriptionField;

        private TextField authorNameField;
        private TextField authorMainLinkField;
        private TextField authorSocialLinksField;

        [MenuItem("Nanodogs/Games/NanoBox/Mod/Create New")]
        public static void ShowWindow()
        {
            var wnd = GetWindow<NanoBoxModCreationWindow>();
            wnd.titleContent = new GUIContent("Create NanoBox Mod");
        }

        public void CreateGUI()
        {
            var root = rootVisualElement;
            root.style.paddingLeft = 10;
            root.style.paddingRight = 10;
            root.style.paddingTop = 10;
            root.style.paddingBottom = 10;

            var title = new Label("NanoBox Mod Creation Window");
            title.style.unityFontStyleAndWeight = FontStyle.Bold;
            title.style.marginBottom = 8;
            root.Add(title);

            // --- Mod Info ---
            root.Add(new Label("Mod Info") { style = { unityFontStyleAndWeight = FontStyle.Bold, marginTop = 6 } });

            modNameField = new TextField("Mod Name") { value = "New NanoBox Mod" };
            root.Add(modNameField);

            var versionRow = new VisualElement { style = { flexDirection = FlexDirection.Row } };
            majorField = new IntegerField("Major") { value = 0 };
            minorField = new IntegerField("Minor") { value = 1 };
            patchField = new IntegerField("Patch") { value = 0 };

            majorField.style.flexGrow = 1;
            minorField.style.flexGrow = 1;
            patchField.style.flexGrow = 1;

            versionRow.Add(majorField);
            versionRow.Add(minorField);
            versionRow.Add(patchField);
            root.Add(versionRow);

            descriptionField = new TextField("Description") { value = "A description of the mod." };
            descriptionField.multiline = true;
            descriptionField.style.minHeight = 60;
            root.Add(descriptionField);

            // --- Author Info ---
            root.Add(new Label("Author Info") { style = { unityFontStyleAndWeight = FontStyle.Bold, marginTop = 10 } });

            authorNameField = new TextField("Author Name");
            root.Add(authorNameField);

            authorMainLinkField = new TextField("Author Main Link (Linktree/Site)");
            root.Add(authorMainLinkField);

            authorSocialLinksField = new TextField("Author Social Links (comma separated)");
            root.Add(authorSocialLinksField);

            // --- Create Button ---
            var createButton = new Button(CreateMod)
            {
                text = "Create Mod"
            };
            createButton.style.marginTop = 12;
            createButton.style.height = 30;
            root.Add(createButton);
        }

        private void CreateMod()
        {
            string modName = modNameField.value;
            var version = new ModVersion(majorField.value, minorField.value, patchField.value);
            string description = descriptionField.value;

            string authorName = authorNameField.value;
            string authorMainLink = authorMainLinkField.value;
            string[] socialLinks = authorSocialLinksField.value
                .Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries)
                .Select(link => link.Trim())
                .ToArray();

            var author = new ModAuthor
            {
                AuthorName = authorName,
                AuthorMainLink = authorMainLink,
                AuthorSocialLinks = socialLinks
            };

            Debug.Log($"Creating mod: {modName} v{version} by {author.AuthorName}");

            string assetPath = $"Assets/User/Mods/{modName.Replace(" ", "_")}.nbm";

            // Inside CreateMod()
            nbMod mod = new nbMod(modName, version, description, author);

            // Ensure the directory exists physically
            string folderPath = Application.dataPath + "/User/Mods";
            if (!System.IO.Directory.Exists(folderPath))
                System.IO.Directory.CreateDirectory(folderPath);

            // Use the object, not the SO, for JsonUtility
            string json = JsonUtility.ToJson(mod, true);
            System.IO.File.WriteAllText(assetPath, json);

            AssetDatabase.ImportAsset(assetPath);

            var asset = AssetDatabase.LoadMainAssetAtPath(assetPath);
            EditorGUIUtility.PingObject(asset); // Highlights the file
            Selection.activeObject = asset;      // Selects the file
        }
    }
}