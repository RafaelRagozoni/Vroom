using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InstantiatePrefabUI : MonoBehaviour
{
    public static InstantiatePrefabUI Instance;

    public Camera sceneCamera;
    public List<GameObject> FurnitureCategories;
    private List<GameObject> spawnedPrefabs = new List<GameObject>();
    public List<GameObject> ChairList;
    private float r = 1.0f;
    private float x_position;
    private float z_position;

    public bool AddFurnitureMode = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private IEnumerator AnimateAndDestroy(GameObject obj, float duration = 0.3f)
    {
        Vector3 originalScale = obj.transform.localScale;
        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;
            obj.transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, t);
            time += Time.deltaTime;
            yield return null;
        }

        Destroy(obj);
    }


    public void DeactivateAddFurnitureMode()
    {
        AddFurnitureMode = false;
        foreach (GameObject obj in spawnedPrefabs)
        {
            if (obj != null)
            {
                StartCoroutine(AnimateAndDestroy(obj));
            }
        }
        spawnedPrefabs.Clear();
    }

    public void InstantiateFurnitureUI()
    {
        if (AddFurnitureMode==false)
        {
            for (int i = 0; i < FurnitureCategories.Count; ++i)
            {
                x_position = sceneCamera.transform.position.x + r * Mathf.Cos(i * 2 * Mathf.PI / FurnitureCategories.Count);
                z_position = sceneCamera.transform.position.z + r * Mathf.Sin(i * 2 * Mathf.PI / FurnitureCategories.Count);
                Vector3 position = new Vector3(x_position, sceneCamera.transform.position.y, z_position);
                Vector3 directionToCamera = sceneCamera.transform.position - position;
                Quaternion rotation = Quaternion.LookRotation(directionToCamera);
                GameObject prefab = Instantiate(FurnitureCategories[i], position, rotation);
                spawnedPrefabs.Add(prefab);

                var grabbable = prefab.GetComponentInChildren<Grabbable>();
                if (grabbable != null)
                {
                    grabbable.enabled = false; // Impede de ser agarrado
                    Debug.Log($"Desativado Grabbable no filho de {prefab.name}");

                    // Adiciona o CategoriesObjectClickManager se ainda não tiver
                    if (grabbable.GetComponent<CategoriesObjectClickManager>() == null)
                    {
                        grabbable.gameObject.AddComponent<CategoriesObjectClickManager>();
                    }

                }
                else
                {
                    Debug.LogWarning($"Nenhum Grabbable encontrado em {prefab.name} ou filhos.");
                }
            }
        }
        AddFurnitureMode = true;
    }

    //public void InstantiateListUI(List<GameObject> List)
    //{
    //    for (int i = 0; i < List.Count; ++i)
    //    {
    //        x_position = sceneCamera.transform.position.x + r * Mathf.Cos(i * 2 * Mathf.PI / List.Count);
    //        z_position = sceneCamera.transform.position.z + r * Mathf.Sin(i * 2 * Mathf.PI / List.Count);
    //        Vector3 position = new Vector3(x_position, sceneCamera.transform.position.y, z_position);
    //        Vector3 directionToCamera = sceneCamera.transform.position - position;
    //        Quaternion rotation = Quaternion.LookRotation(directionToCamera);
    //        GameObject prefab = Instantiate(List[i], position, rotation);
    //        spawnedPrefabs.Add(prefab);

    //        var grabbable = prefab.GetComponentInChildren<Grabbable>();
    //        if (grabbable != null)
    //        {
    //            grabbable.enabled = false; // Impede de ser agarrado
    //            Debug.Log($"Desativado Grabbable no filho de {prefab.name}");
    //            // Adiciona o InstantiateObjectClickManager se ainda não tiver
    //            if (grabbable.GetComponent<InstantiateObjectClickManager>() == null)
    //            {
    //                grabbable.gameObject.AddComponent<InstantiateObjectClickManager>();
    //            }
    //        }
    //        else
    //        {
    //            Debug.LogWarning($"Nenhum Grabbable encontrado em {prefab.name} ou filhos.");
    //        }
    //    }
    //}

    public void InstantiateListUI(List<GameObject> List)
    {
        for (int i = 0; i < List.Count; ++i)
        {
            x_position = sceneCamera.transform.position.x + r * Mathf.Cos(i * 2 * Mathf.PI / List.Count);
            z_position = sceneCamera.transform.position.z + r * Mathf.Sin(i * 2 * Mathf.PI / List.Count);
            Vector3 position = new Vector3(x_position, sceneCamera.transform.position.y, z_position);
            Vector3 directionToCamera = sceneCamera.transform.position - position;
            Quaternion rotation = Quaternion.LookRotation(directionToCamera);

            GameObject prefab = Instantiate(List[i], position, rotation);
            spawnedPrefabs.Add(prefab);

            // Remove o componente CategoriesObjectClickManager, se existir
            var categoryClickManager = prefab.GetComponentInChildren<CategoriesObjectClickManager>();
            if (categoryClickManager != null)
            {
                Destroy(categoryClickManager);
                Debug.Log($"Removido CategoriesObjectClickManager de {prefab.name}");
            }

            var grabbable = prefab.GetComponentInChildren<Grabbable>();
            if (grabbable != null)
            {
                grabbable.enabled = false; // Impede de ser agarrado
                Debug.Log($"Desativado Grabbable no filho de {prefab.name}");
                if (grabbable.GetComponent<InstantiateObjectClickManager>() == null)
                {
                    grabbable.gameObject.AddComponent<InstantiateObjectClickManager>();
                }
            }
            else
            {
                Debug.LogWarning($"Nenhum Grabbable encontrado em {prefab.name} ou filhos.");
            }
        }
    }



}
