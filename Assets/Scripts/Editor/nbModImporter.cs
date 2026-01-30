using UnityEditor.AssetImporters;
using UnityEngine;
using System.IO;

[ScriptedImporter(1, "nbm")]
public class NbModImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        // This tells Unity to treat the .nbm as a TextAsset
        var subAsset = new TextAsset(File.ReadAllText(ctx.assetPath));
        ctx.AddObjectToAsset("main", subAsset);
        ctx.SetMainObject(subAsset);
    }
}
