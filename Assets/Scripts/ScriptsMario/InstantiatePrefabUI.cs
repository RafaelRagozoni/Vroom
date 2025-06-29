using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InstantiatePrefabUI : MonoBehaviour
{
    public static InstantiatePrefabUI Instance;

    public Camera sceneCamera;
    public List<string> FurnitureCategories;
    private List<GameObject> spawnedPrefabs = new List<GameObject>();

    //Object List
    public List<string> ChairList;
    public List<string> BedList;
    public List<string> SofaList;
    public List<string> TableList;

    public List<string> WallDecorationList;


    private float r = 1.5f;
    private float x_position;
    private float z_position;

    public List<string> AuxPrefabList;
    public List<string> currentCategoryList;
    private int currentStartIndex;
    public int ChunkSize;

    public bool CategoriesAddFurnitureMode = false;
    public bool InstantiateFurnitureMode = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentStartIndex = 0;
        ChunkSize = 5;
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
        EditFurnitureManager.Instance.DeactivateEditFurnitureMode();
        InstantiateTexturesUI.Instance.DeactivateTextureEditMode();
        GetComponent<ActivateSaveAndLoadUI>().DeactivateAllUI();
        DeleteManager.Instance.DeactivateDeletionMode();
        if (CategoriesAddFurnitureMode == false && InstantiateFurnitureMode == false)
        {
            for (int i = 0; i < FurnitureCategories.Count; ++i)
            {
                x_position = sceneCamera.transform.position.x + r * Mathf.Cos(i * 2 * Mathf.PI / FurnitureCategories.Count);
                z_position = sceneCamera.transform.position.z + r * Mathf.Sin(i * 2 * Mathf.PI / FurnitureCategories.Count);
                //Vector3 position = new Vector3(x_position, sceneCamera.transform.position.y-0.3f, z_position);
                //Vector3 directionToCamera = sceneCamera.transform.position - position;
                //Quaternion rotation = Quaternion.LookRotation(directionToCamera);
                Vector3 position = new Vector3(x_position, sceneCamera.transform.position.y - 0.3f, z_position);

                // Calcula dire��o apenas no plano XZ
                Vector3 cameraPosXZ = new Vector3(sceneCamera.transform.position.x, position.y, sceneCamera.transform.position.z);
                Vector3 directionToCamera = cameraPosXZ - position;
                Quaternion rotation = Quaternion.LookRotation(directionToCamera);


                GameObject prefab = GetComponent<FurnitureSpawner>().SpawnPrefab(FurnitureCategories[i], position, rotation);//Instantiate(FurnitureCategories[i], position, rotation);

                spawnedPrefabs.Add(prefab);

                // Guarda o original para futura inst�ncia real
                var holder = prefab.AddComponent<OriginalPrefabHolder>();
                holder.originalPrefab = prefab;

                // NORMALIZA O TAMANHO para UI
                NormalizeScale(prefab);

                // Rotaciona objetos do tipo wall para facilitar a visualização
                if (FurnitureCategories[i].Contains("Wall"))
                {
                    prefab.transform.Rotate(-90, 0, 180);
                }

                var grabbable = prefab.GetComponentInChildren<Grabbable>();
                if (grabbable != null)
                {
                    //grabbable.enabled = false; // Impede de ser agarrado
                    Debug.Log($"Desativado Grabbable no filho de {prefab.name}");

                    // Adiciona o CategoriesObjectClickManager se ainda n�o tiver
                    if (grabbable.GetComponent<CategoriesObjectClickManager>() == null)
                    {
                        var clickManager = grabbable.gameObject.AddComponent<CategoriesObjectClickManager>();
                        clickManager.FurnitureModelPrefabPath = FurnitureCategories[i];
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

    public void InstantiateListUI(List<string> List, FurnitureType type = FurnitureType.Floor)
    {
        this.currentType = type;
        for (int i = 0; i < List.Count; ++i)
        {

            x_position = sceneCamera.transform.position.x + r * Mathf.Cos(i * 2 * Mathf.PI / List.Count);
            z_position = sceneCamera.transform.position.z + r * Mathf.Sin(i * 2 * Mathf.PI / List.Count);

            Vector3 position = new Vector3(x_position, sceneCamera.transform.position.y - 0.3f, z_position);

            // Calcula dire��o apenas no plano XZ
            Vector3 cameraPosXZ = new Vector3(sceneCamera.transform.position.x, position.y, sceneCamera.transform.position.z);
            Vector3 directionToCamera = cameraPosXZ - position;
            Quaternion rotation = Quaternion.LookRotation(directionToCamera);

            // Instancia a miniatura
            //GameObject prefab = Instantiate(List[i], position, rotation);//GetComponent<FurnitureSpawner>().SpawnPrefab(List[i], position, rotation);
            GameObject prefab = GetComponent<FurnitureSpawner>().SpawnPrefab(List[i], position, rotation);
            spawnedPrefabs.Add(prefab);

            // Guarda o original para futura inst�ncia real
            var holder = prefab.AddComponent<OriginalPrefabHolder>();
            holder.originalPrefab = prefab;

            // NORMALIZA O TAMANHO para UI
            NormalizeScale(prefab);


            // Rotaciona objetos do tipo wall para facilitar a visualização
            if (List[i].Contains("Wall"))
            {
                prefab.transform.Rotate(-90, 0, 180);
            }

            // Remove intera��es n�o desejadas na UI
            var categoryClickManager = prefab.GetComponentInChildren<CategoriesObjectClickManager>();
            if (categoryClickManager != null)
            {
                Destroy(categoryClickManager);
                Debug.Log($"Removido CategoriesObjectClickManager de {prefab.name}");
            }

            var grabbable = prefab.GetComponentInChildren<Grabbable>();
            if (grabbable != null)
            {
                //grabbable.enabled = false; // Impede de ser agarrado
                Debug.Log($"Desativado Grabbable no filho de {prefab.name}");

                // Adiciona script para instanciar original ao clicar
                if (grabbable.GetComponent<InstantiateObjectClickManager>() == null)
                {
                    var clickManager = grabbable.gameObject.AddComponent<InstantiateObjectClickManager>();
                    clickManager.FurnitureModelPrefabPath = List[i];
                    clickManager.FurnitureType = type;
                }
            }
            else
            {
                Debug.LogWarning($"Nenhum Grabbable encontrado em {prefab.name} ou filhos.");
            }
        }
    }


    private void NormalizeScale(GameObject obj)
    {
        // Define o tamanho alvo da maior dimens�o
        float targetSize = 0.6f;

        // Calcula o maior lado do bounds
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        Bounds bounds = new Bounds(obj.transform.position, Vector3.zero);
        foreach (var rend in renderers)
        {
            bounds.Encapsulate(rend.bounds);
        }

        Debug.Log($"Bounds size: {bounds.size}");

        float maxDimension = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);
        if (maxDimension > 0f)
        {
            float scaleFactor = targetSize / maxDimension;
            obj.transform.localScale *= scaleFactor;
        }
    }

    public void UpdateAuxList(List<string> sourceList, int chunkSize, int startIndex)
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
            int index = (startIndex + i) % sourceList.Count; // Índice circular
            AuxPrefabList.Add(sourceList[index]);
        }

        Debug.Log($"Adicionados {chunkSize} elementos à AuxPrefabList a partir do índice {startIndex} (circular).");
    }



    private bool canClick = true;
    private FurnitureType currentType;

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
            StartCoroutine(EnableClickAfterDelay(0.3f));  // Reativa ap�s delay

            Debug.Log("Valor de ChunkSize dentro da fun��o: " + ChunkSize);
            Debug.Log("Valor de currentStartIndex dentro da fun��o: " + currentStartIndex);

            DeactivateAddFurnitureMode();
            currentStartIndex = (currentStartIndex + ChunkSize) % currentCategoryList.Count;
            UpdateAuxList(currentCategoryList, ChunkSize, currentStartIndex);
            InstantiateListUI(AuxPrefabList, currentType);
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

            Debug.Log("Valor de ChunkSize dentro da fun��o: " + ChunkSize);
            Debug.Log("Valor de currentStartIndex dentro da fun��o: " + currentStartIndex);

            DeactivateAddFurnitureMode();

            // Atualiza o �ndice para tr�s, garantindo que continue no intervalo correto
            currentStartIndex = (currentStartIndex - ChunkSize + currentCategoryList.Count) % currentCategoryList.Count;

            UpdateAuxList(currentCategoryList, ChunkSize, currentStartIndex);
            InstantiateListUI(AuxPrefabList, currentType);
            InstantiateFurnitureMode = true;
        }
    }

    private IEnumerator EnableClickAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canClick = true;
    }

}
