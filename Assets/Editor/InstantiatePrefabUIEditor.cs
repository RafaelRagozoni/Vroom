using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

[CustomEditor(typeof(InstantiatePrefabUI))]
public class InstantiatePrefabUIEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        InstantiatePrefabUI script = (InstantiatePrefabUI)target;

        if (GUILayout.Button("Preencher BedList com raw_model"))
        {
            script.BedList = GetRawModelPaths("Prefabs/FurnitureModels/Bed");
            EditorUtility.SetDirty(script);
        }
        if (GUILayout.Button("Preencher ChairList com raw_model"))
        {
            script.ChairList = GetRawModelPaths("Prefabs/FurnitureModels/Chair");
            EditorUtility.SetDirty(script);
        }
        if (GUILayout.Button("Preencher SofaList com raw_model"))
        {
            script.SofaList = GetRawModelPaths("Prefabs/FurnitureModels/Sofa");
            EditorUtility.SetDirty(script);
        }
        if (GUILayout.Button("Preencher TableList com raw_model"))
        {
            script.TableList = GetRawModelPaths("Prefabs/FurnitureModels/Table");
            EditorUtility.SetDirty(script);
        }
        if (GUILayout.Button("Preencher WallDecorationList com raw_model"))
        {
            script.WallDecorationList = GetRawModelPaths("Prefabs/FurnitureModels/WallDecoration");
            EditorUtility.SetDirty(script);
        }
    }

    private List<string> GetRawModelPaths(string resourcesRelativeFolder)
    {
        List<string> paths = new List<string>();
        string resourcesPath = Path.Combine(Application.dataPath, "Resources", resourcesRelativeFolder);

        if (!Directory.Exists(resourcesPath))
        {
            Debug.LogWarning("Pasta não encontrada: " + resourcesPath);
            return paths;
        }

        string[] files = Directory.GetFiles(resourcesPath, "raw_model.obj", SearchOption.AllDirectories);
        foreach (var file in files)
        {
            string relPath = file.Substring(file.IndexOf("Resources") + "Resources".Length + 1);
            relPath = relPath.Replace("\\", "/");
            relPath = relPath.Substring(0, relPath.Length - ".obj".Length);
            paths.Add(relPath);
        }
        return paths;
    }
}