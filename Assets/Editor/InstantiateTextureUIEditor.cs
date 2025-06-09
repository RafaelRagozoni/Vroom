using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

[CustomEditor(typeof(InstantiateTexturesUI))]
public class InstantiateTextureUIEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        InstantiateTexturesUI script = (InstantiateTexturesUI)target;

        if (GUILayout.Button("Preencher Wall List com Textures"))
        {
            script.WallTextureList = GetTexturesPaths("Prefabs/TexturePrefabs/WallTextures");
            EditorUtility.SetDirty(script);
        }
        if (GUILayout.Button("Preencher  Floor List com Textures"))
        {
            script.FloorTextureList = GetTexturesPaths("Prefabs/TexturePrefabs/FloorTextures");
            EditorUtility.SetDirty(script);
        }
        if (GUILayout.Button("Preencher Ceiling List com Textures"))
        {
            script.CeilTextureList = GetTexturesPaths("Prefabs/TexturePrefabs/CeilingTextures");
            EditorUtility.SetDirty(script);
        }
    }

    private List<string> GetTexturesPaths(string resourcesRelativeFolder)
    {
        List<string> paths = new List<string>();
        string resourcesPath = Path.Combine(Application.dataPath, "Resources", resourcesRelativeFolder);

        if (!Directory.Exists(resourcesPath))
        {
            Debug.LogWarning("Pasta não encontrada: " + resourcesPath);
            return paths;
        }

        string[] files = Directory.GetFiles(resourcesPath, "*.prefab", SearchOption.AllDirectories);
        foreach (var file in files)
        {
            string relPath = file.Substring(file.IndexOf("Resources") + "Resources".Length + 1);
            relPath = relPath.Replace("\\", "/");
            relPath = relPath.Substring(0, relPath.Length - ".prefab".Length);
            paths.Add(relPath);
        }
        return paths;
    }
}
