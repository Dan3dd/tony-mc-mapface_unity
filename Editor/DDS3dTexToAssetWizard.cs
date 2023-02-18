using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class DDS3dTexToAssetWizard : ScriptableWizard
{
    public Object textureFile;

    
    [MenuItem("Tools/DDS 3d tex to asset")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<DDS3dTexToAssetWizard>("Convert", "Create");
    }

    void OnWizardCreate()
    {
        var path = AssetDatabase.GetAssetPath(textureFile);
        string assetPath = Path.ChangeExtension(path, ".asset");
        Debug.Log(path);
        Debug.Log(assetPath);
        
        // Hardcoded size and header offset
        var tex = new Texture3D(48, 48, 48, TextureFormat.RGB9e5Float, false);
        var bits = new BinaryReader(File.Open(path, FileMode.Open, FileAccess.Read));
        bits.BaseStream.Seek(148, SeekOrigin.Begin);
        var bytes = bits.ReadBytes(48 * 48 * 48 * 4);
        bits.Close();
        tex.SetPixelData(bytes, 0, 0);
        tex.Apply();
        
        AssetDatabase.CreateAsset(tex, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
