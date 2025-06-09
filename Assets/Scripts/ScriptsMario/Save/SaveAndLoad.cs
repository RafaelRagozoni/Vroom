using System;
using System.Collections.Generic;
using Meta.WitAi.Json;
using UnityEngine;

public class SaveAndLoad : MonoBehaviour
{
    // Room Objects
    public GameObject wallR;
    public GameObject wallL;
    public GameObject wallB;
    public GameObject wallF;
    public GameObject ceiling;
    public GameObject floor;

    

    [System.Serializable]
    public class InstancedFurnitureData
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Scale;
        public string FurnitureModelPath;
    }
    [System.Serializable]
    public class RoomData
    {
        public Vector3 GizmosPosition; // Position of the Gizmo in the room

        public string[] Materials = new string[6]; // Paths of Materials for each wall and floor
        // 0: Floor, 1: Ceiling, 2: Wall Left, 3: Wall Right, 4: Wall Front, 5: Wall Back

    }
    [System.Serializable]
    public class SceneData
    {
        public List<InstancedFurnitureData> InstancedFurnitures = new List<InstancedFurnitureData>();
        public RoomData RoomData = new RoomData(); // Data for the room
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
        //Saving Room Data
        sceneData.RoomData.Materials[0] = floor.GetComponent<Renderer>().material.name.Replace(" (Instance)", "");
        sceneData.RoomData.Materials[1] = ceiling.GetComponent<Renderer>().material.name.Replace(" (Instance)", "");
        sceneData.RoomData.Materials[2] = wallL.GetComponent<Renderer>().material.name.Replace(" (Instance)", "");
        sceneData.RoomData.Materials[3] = wallR.GetComponent<Renderer>().material.name.Replace(" (Instance)", "");
        sceneData.RoomData.Materials[4] = wallF.GetComponent<Renderer>().material.name.Replace(" (Instance)", "");
        sceneData.RoomData.Materials[5] = wallB.GetComponent<Renderer>().material.name.Replace(" (Instance)", "");
        // Saving Gizmo Position
        sceneData.RoomData.GizmosPosition = GetComponent<RoomReshaper>().GetGizmoPosition();


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

            // Load Room Data
            GetComponent<RoomReshaper>().MoveGizmos(sceneData.RoomData.GizmosPosition);
            // Apply Materials to Walls and Floor
            var materialsPathDict = GetComponent<MaterialsPathDict>();
    
            floor.GetComponent<Renderer>().material = Resources.Load<Material>(materialsPathDict.GetPathByName(sceneData.RoomData.Materials[0]));
            ceiling.GetComponent<Renderer>().material = Resources.Load<Material>(materialsPathDict.GetPathByName(sceneData.RoomData.Materials[1]));
            wallL.GetComponent<Renderer>().material = Resources.Load<Material>(materialsPathDict.GetPathByName(sceneData.RoomData.Materials[2]));
            wallR.GetComponent<Renderer>().material = Resources.Load<Material>(materialsPathDict.GetPathByName(sceneData.RoomData.Materials[3]));
            wallF.GetComponent<Renderer>().material = Resources.Load<Material>(materialsPathDict.GetPathByName(sceneData.RoomData.Materials[4]));
            wallB.GetComponent<Renderer>().material = Resources.Load<Material>(materialsPathDict.GetPathByName(sceneData.RoomData.Materials[5]));

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
