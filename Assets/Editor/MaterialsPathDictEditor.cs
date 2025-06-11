#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

[CustomEditor(typeof(MaterialsPathDict))]
public class MaterialsPathDictEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MaterialsPathDict script = (MaterialsPathDict)target;

        if (GUILayout.Button("Preencher MaterialsPath com materiais da Resources/Materials"))
        {
            List<string> names;
            List<string> paths;
            GetMaterialsNamesAndPaths("Materials", out names, out paths);
            script.MaterialsName = names;
            script.MaterialsPath = paths;
            EditorUtility.SetDirty(script);
            Debug.Log("MaterialsName e MaterialsPath preenchidos!");
        }
    }

    private void GetMaterialsNamesAndPaths(string resourcesRelativeFolder, out List<string> names, out List<string> paths)
    {
        names = new List<string>();
        paths = new List<string>();
        string resourcesPath = Path.Combine(Application.dataPath, "Resources", resourcesRelativeFolder);

        if (!Directory.Exists(resourcesPath))
        {
            Debug.LogWarning("Pasta não encontrada: " + resourcesPath);
            return;
        }

        string[] files = Directory.GetFiles(resourcesPath, "*.mat", SearchOption.AllDirectories);
        foreach (var file in files)
        {
            string relPath = file.Substring(file.IndexOf("Resources") + "Resources".Length + 1);
            relPath = relPath.Replace("\\", "/");
            relPath = relPath.Substring(0, relPath.Length - ".mat".Length);
            string matName = Path.GetFileNameWithoutExtension(file);
            names.Add(matName);
            paths.Add(relPath);
        }
    }
}
#endif