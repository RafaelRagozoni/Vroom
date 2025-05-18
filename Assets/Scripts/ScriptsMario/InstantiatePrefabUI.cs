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

    //Object List
    public List<GameObject> ChairList;
    public List<GameObject> BedList;


    private float r = 1.0f;
    private float x_position;
    private float z_position;
    
    public List<GameObject> AuxPrefabList;
    public List<GameObject> currentCategoryList;
    private int currentStartIndex;
    public int ChunkSize;

    public bool CategoriesAddFurnitureMode = false;
    public bool InstantiateFurnitureMode = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentStartIndex = 0;
        ChunkSize = 3;
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
        CategoriesAddFurnitureMode = false;
        InstantiateFurnitureMode = false;
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
        DeleteManager.Instance.DeactivateDeletionMode();
        if (CategoriesAddFurnitureMode==false && InstantiateFurnitureMode==false)
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
            CategoriesAddFurnitureMode = true;
        }
        
    }

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


    public void UpdateAuxList(List<GameObject> sourceList, int chunkSize, int startIndex)
    {
        AuxPrefabList.Clear();

        if (sourceList == null || sourceList.Count == 0 || chunkSize <= 0)
        {
            Debug.LogWarning("Lista inválida ou chunkSize <= 0.");
            return;
        }

        // Garantir startIndex válido dentro do intervalo da lista
        startIndex = startIndex % sourceList.Count;

        for (int i = 0; i < chunkSize; i++)
        {
            int index = (startIndex + i) % sourceList.Count; // índice circular
            AuxPrefabList.Add(sourceList[index]);
        }

        Debug.Log($"Adicionados {chunkSize} elementos à AuxPrefabList a partir do índice {startIndex} (circular).");
    }



    private bool canClick = true;

    public void CycleInstantiatedChunk()
    {
        if(InstantiateFurnitureMode==true)
        {
            if (!canClick)
            {
                Debug.Log("Aguarde antes de clicar novamente.");
                return;
            }

            canClick = false;  // Bloqueia novos cliques
            StartCoroutine(EnableClickAfterDelay(0.3f));  // Reativa após delay

            Debug.Log("Valor de ChunkSize dentro da função: " + ChunkSize);
            Debug.Log("Valor de currentStartIndex dentro da função: " + currentStartIndex);

            DeactivateAddFurnitureMode();
            currentStartIndex = (currentStartIndex + ChunkSize) % currentCategoryList.Count;
            UpdateAuxList(currentCategoryList, ChunkSize, currentStartIndex);
            InstantiateListUI(AuxPrefabList);
            InstantiateFurnitureMode = true;
        }    
    }

    public void CycleInstantiatedChunkBackwards()
    {
        if(InstantiateFurnitureMode==true)
        {
            if (!canClick)
            {
                Debug.Log("Aguarde antes de clicar novamente.");
                return;
            }

            canClick = false;
            StartCoroutine(EnableClickAfterDelay(0.3f));

            Debug.Log("Valor de ChunkSize dentro da função: " + ChunkSize);
            Debug.Log("Valor de currentStartIndex dentro da função: " + currentStartIndex);

            DeactivateAddFurnitureMode();

            // Atualiza o índice para trás, garantindo que continue no intervalo correto
            currentStartIndex = (currentStartIndex - ChunkSize + currentCategoryList.Count) % currentCategoryList.Count;

            UpdateAuxList(currentCategoryList, ChunkSize, currentStartIndex);
            InstantiateListUI(AuxPrefabList);
            InstantiateFurnitureMode = true;
        }
    }

    private IEnumerator EnableClickAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canClick = true;
    }

}
