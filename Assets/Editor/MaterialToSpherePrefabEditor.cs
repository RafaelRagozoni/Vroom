using UnityEngine;
using UnityEditor;
using System.IO;

public class MaterialToSpherePrefabEditor : EditorWindow
{
    [MenuItem("Tools/Gerar Esferas de Materiais")]
    public static void ShowWindow()
    {
        GetWindow<MaterialToSpherePrefabEditor>("Gerar Esferas de Materiais");
    }

    void OnGUI()
    {
        GUILayout.Label("Gerar Prefabs de Esferas para Materiais", EditorStyles.boldLabel);

        if (GUILayout.Button("Gerar Esferas de Floor"))
        {
            GeneratePrefabs(
                "Assets/Resources/Materials/Floor",
                "Assets/Resources/Prefabs/TexturePrefabs/FloorTextures"
            );
        }
        if (GUILayout.Button("Gerar Esferas de Wall"))
        {
            GeneratePrefabs(
                "Assets/Resources/Materials/Wall",
                "Assets/Resources/Prefabs/TexturePrefabs/WallTextures"
            );
        }
        if (GUILayout.Button("Gerar Esferas de Ceiling"))
        {
            GeneratePrefabs(
                "Assets/Resources/Materials/Ceiling",
                "Assets/Resources/Prefabs/TexturePrefabs/CeilingTextures"
            );
        }
    }

    void GeneratePrefabs(string materialsFolder, string saveFolder)
    {
        if (!Directory.Exists(materialsFolder))
        {
            Debug.LogError("Pasta de materiais não encontrada: " + materialsFolder);
            return;
        }
        if (!Directory.Exists(saveFolder))
        {
            Directory.CreateDirectory(saveFolder);
        }

        string[] materialGuids = AssetDatabase.FindAssets("t:Material", new[] { materialsFolder });
        foreach (string guid in materialGuids)
        {
            string materialPath = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(materialPath);

            // Cria esfera
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.GetComponent<Renderer>().sharedMaterial = mat;
            sphere.name = mat.name + "_Sphere";

            // Salva como prefab
            string prefabPath = Path.Combine(saveFolder, sphere.name + ".prefab").Replace("\\", "/");
            PrefabUtility.SaveAsPrefabAsset(sphere, prefabPath);

            GameObject.DestroyImmediate(sphere);
        }
        AssetDatabase.Refresh();
        Debug.Log("Prefabs gerados em: " + saveFolder);
    }
}