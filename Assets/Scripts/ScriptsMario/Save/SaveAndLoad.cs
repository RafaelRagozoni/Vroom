using System.Collections.Generic;
using Meta.WitAi.Json;
using UnityEngine;

public class SaveAndLoad : MonoBehaviour
{
    [System.Serializable]
    public class InstancedFurnitureData
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Scale;
        public string FurnitureModelPath;
    }
    [System.Serializable]
    public class SceneData
    {
        public List<InstancedFurnitureData> InstancedFurnitures = new List<InstancedFurnitureData>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Save()
    {
        var instantiatedFurniture = GetComponent<FurnitureSpawner>().InstantiatedFurnitureIds;
        var furniturePaths = GetComponent<FurnitureSpawner>().InstantiatedFurniturePaths;
        var sceneData = new SceneData();
        foreach (var furniture in instantiatedFurniture)
        {
            Debug.Log($"Saving Furniture ID: {furniture.FurnitureBaseId}, Model ID: {furniture.ModelId}");
            // Aqui você pode adicionar a lógica para salvar os dados do mobiliário, por exemplo, em um arquivo ou banco de dados.
            var modelGameObject = FindGameObjectByInstanceID(furniture.FurnitureBaseId);
            if (modelGameObject != null)
            {
                // Salvar a posição, rotação, etc. do modelo
                InstancedFurnitureData data = new InstancedFurnitureData
                {
                    Position = modelGameObject.transform.position,
                    Rotation = modelGameObject.transform.rotation,
                    Scale = modelGameObject.transform.localScale,
                    FurnitureModelPath = furniturePaths[furniture.FurnitureBaseId]
                };
                sceneData.InstancedFurnitures.Add(data);
            }
        }
        string json = JsonUtility.ToJson(sceneData, true); // 'true' para formatar bonito
        System.IO.File.WriteAllText(Application.persistentDataPath + "/sceneData.json", json);
        Debug.Log(Application.persistentDataPath);
        Debug.Log("Scene data saved successfully.");
    }

    public void Load()
    {   
        string path = Application.persistentDataPath + "/sceneData.json";
        if (System.IO.File.Exists(path))
        {
            var instantiatedFurniture = GetComponent<FurnitureSpawner>().InstantiatedFurnitureIds;
            foreach (var furniture in instantiatedFurniture)
            {
                Debug.Log($"Removing Furniture ID: {furniture.FurnitureBaseId}, Model ID: {furniture.ModelId}");
                GameObject furnitureObject = FindGameObjectByInstanceID(furniture.FurnitureBaseId);
                if (furnitureObject != null)
                {
                    Destroy(furnitureObject);
                }
            }
            string json = System.IO.File.ReadAllText(path);
            SceneData sceneData = JsonUtility.FromJson<SceneData>(json);
            foreach (var furnitureData in sceneData.InstancedFurnitures)
            {
                Debug.Log($"Loading Furniture Model Path: {furnitureData.FurnitureModelPath}");
                GameObject spawnedFurniture = GetComponent<FurnitureSpawner>().SpawnPrefab(furnitureData.FurnitureModelPath, furnitureData.Position, furnitureData.Rotation);
                if (spawnedFurniture != null)
                {
                    spawnedFurniture.transform.localScale = furnitureData.Scale;
                }
            }
            Debug.Log("Scene data loaded successfully.");
        }
        else
        {
            Debug.LogError("No saved scene data found.");
        }
    }

    public GameObject FindGameObjectByInstanceID(int instanceID)
    {
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        foreach (var obj in allObjects)
        {
            if (obj.GetInstanceID() == instanceID)
                return obj;
        }
        return null;
    }
}
