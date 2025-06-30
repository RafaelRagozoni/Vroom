using Oculus.Interaction;
using Oculus.Interaction.Surfaces;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class FurnitureSpawner : MonoBehaviour
{
    public class FurnitureData
    {
        public int FurnitureBaseId;
        public int ModelId;
    }

    public List<FurnitureData> InstantiatedFurnitureIds = new List<FurnitureData>();
    public GameObject furniturePrefab;

    public RayInteractor righHandInteractor;
    public RayInteractor leftHandInteractor;

    public float scale = 1.0f;
    [SerializeField] private GameObject prefabParaInstanciar;
    [SerializeField] private Camera sceneCamera;

    public Dictionary<int,string> InstantiatedFurniturePaths = new Dictionary<int,string>();

    public GameObject SpawnPrefab(string pathPrefab, Vector3 position, Quaternion rotation, FurnitureType type=FurnitureType.Floor)
    {
        // Load the prefab from the specified path (runtime safe)
        GameObject model = Resources.Load<GameObject>(pathPrefab);
        if (model == null)
        {
            Debug.LogError($"Prefab not found at Resources path: {pathPrefab}");
            throw new Exception($"Prefab not found at Resources path: {pathPrefab}");
        }

        if (furniturePrefab != null)
        { 
            var furniturePrefabInstance = Instantiate(furniturePrefab, position, rotation);
            var modelInstance = Instantiate(model, position, rotation);

            InstantiatedFurniturePaths[furniturePrefabInstance.GetInstanceID()] = pathPrefab;

            SetupCollider(furniturePrefabInstance, modelInstance);

            modelInstance.transform.SetParent(furniturePrefabInstance.transform);

            SetupInteractors(furniturePrefabInstance,type);
            SetupDoubleClick(furniturePrefabInstance);
            furniturePrefabInstance.transform.localScale = new Vector3(scale, scale, scale);

            var modelCollider = modelInstance.GetComponent<BoxCollider>();

            if (modelCollider != null)
            {
                modelCollider.enabled = false;
            }

            if (model.tag != "Untagged")
            {
                furniturePrefabInstance.tag = model.tag;
            }
            else
            {
                furniturePrefabInstance.tag = "Furniture";
            }

            // Add the instantiated furniture ID to the list
            FurnitureData data = new FurnitureData
            {
                FurnitureBaseId = furniturePrefabInstance.GetInstanceID(),
                ModelId = model.GetInstanceID()
            };
            InstantiatedFurnitureIds.Add(data);

            return furniturePrefabInstance;
        }
        else
        {
            Debug.LogError("Prefab is not assigned in the Inspector!");
            return null;
        }
    }


    private static void SetupCollider(GameObject furniturePrefabInstance, GameObject modelInstance)
    {
        var collider = furniturePrefabInstance.AddComponent<BoxCollider>();
        var childrenBounds = GetChildrenBounds(modelInstance);

        collider.center = furniturePrefabInstance.transform.InverseTransformPoint(childrenBounds.transform.TransformPoint(childrenBounds.center));
        collider.size = furniturePrefabInstance.transform.InverseTransformVector(childrenBounds.transform.TransformVector(childrenBounds.size));
    }


    private void SetupInteractors(GameObject furniturePrefabInstance, FurnitureType type)
    {
        var rayGrabInteractor = furniturePrefabInstance.transform.Find("ISDK_RayGrabInteraction");

        if (rayGrabInteractor != null)
        {
            var transformer = rayGrabInteractor.GetComponent<FurnitureGrabTransformer>();

            transformer.leftHandInteractor = leftHandInteractor;
            transformer.righHandInteractor = righHandInteractor;
            transformer.type = type;

            var colliderSurface = rayGrabInteractor.GetComponent<ColliderSurface>();

            colliderSurface.InjectCollider(furniturePrefabInstance.GetComponent<Collider>());
        }
        else
        {
            Debug.LogError("ISDK_RayGrabInteraction not found!");
        }
    }

    private void SetupDoubleClick(GameObject furniturePrefabInstance)
    {
        var rayGrabInteractor = furniturePrefabInstance.transform.Find("ISDK_RayGrabInteraction");

        if (rayGrabInteractor != null)
        {
            var ObjectClickManager = rayGrabInteractor.GetComponent<ObjectClickManager>();
            ObjectClickManager.Editprefab = prefabParaInstanciar;
            ObjectClickManager.sceneCamera = sceneCamera;
        }
        else
        {
            Debug.LogError("ISDK_RayGrabInteraction not found!");
        }

    }
    public static BoxCollider GetChildrenBounds(GameObject target)
    {
        if (target == null)
        {
            throw new Exception("Target GameObject is null.");
        }

        Renderer[] allRenderers = target.GetComponentsInChildren<Renderer>();
        if (allRenderers.Length == 0)
        {
            throw new Exception("No Renderers found in children of " + target.name);
        }

        Bounds totalBounds = new Bounds();
        bool hasBounds = false;

        foreach (Renderer rend in allRenderers)
        {
            // Skip the target object�s own renderer (if needed)
            //if (rend.transform == target.transform)
            //    continue;

            if (!hasBounds)
            {
                totalBounds = rend.bounds;
                hasBounds = true;
            }
            else
            {
                totalBounds.Encapsulate(rend.bounds);
            }
        }

        if (!hasBounds)
            throw new Exception("No bounds found!");

        // Add or get existing BoxCollider
        BoxCollider box = new();
        if (box == null)
            box = target.AddComponent<BoxCollider>();

        // Convert world bounds to local space
        box.center = target.transform.InverseTransformPoint(totalBounds.center);
        box.size = target.transform.InverseTransformVector(totalBounds.size);

        return box;
    }
}
