using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InstantiateTexturesUI : MonoBehaviour
{

    public static InstantiateTexturesUI Instance;
    public Transform selectedTarget;
    public int ChunkSize;
    public Camera sceneCamera;
    public float r = 1.5f; // Raio do c�rculo
    private float x_position;
    private float z_position;

    private List<GameObject> spawnedTexturePrefabs = new List<GameObject>();
    public List<string> TextureAuxPrefabList = new List<string>();
    public List<string> currentTextureList;
    public List<string> WallTextureList;
    public List<string> FloorTextureList;
    public List<string> CeilTextureList;

    public bool TextureEditMode = false;
    private int currentStartIndex;

    void Awake()
    {
        Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChunkSize = 5;
        currentStartIndex = 0;
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


    public void DeactivateTextureEditMode()
    {
        TextureEditMode = false;
        foreach (GameObject obj in spawnedTexturePrefabs)
        {
            if (obj != null)
            {
                StartCoroutine(AnimateAndDestroy(obj));
            }
        }
        spawnedTexturePrefabs.Clear();
    }
    public void InstantiateTextureListUI(List<string> List)
    {   GetComponent<ActivateSaveAndLoadUI>().DeactivateAllUI();
        for (int i = 0; i < List.Count; ++i)
        {
            x_position = sceneCamera.transform.position.x + r * Mathf.Cos(i * 2 * Mathf.PI / List.Count);
            z_position = sceneCamera.transform.position.z + r * Mathf.Sin(i * 2 * Mathf.PI / List.Count);

            Vector3 position = new Vector3(x_position, sceneCamera.transform.position.y - 0.3f, z_position);

            // Calcula direção apenas no plano XZ
            Vector3 cameraPosXZ = new Vector3(sceneCamera.transform.position.x, position.y, sceneCamera.transform.position.z);
            Vector3 directionToCamera = cameraPosXZ - position;
            Quaternion rotation = Quaternion.LookRotation(directionToCamera);

            // Instancia
            GameObject prefab = GetComponent<FurnitureSpawner>().SpawnPrefab(List[i], position, rotation);
            prefab.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            spawnedTexturePrefabs.Add(prefab);

            var grabbable = prefab.GetComponentInChildren<Grabbable>();
            if (grabbable != null)
            {
                // Adiciona script para instanciar original ao clicar
                if (grabbable.GetComponent<ChangeTextureClickManager>() == null)
                {
                    grabbable.gameObject.AddComponent<ChangeTextureClickManager>();
                }
                
            }
            else
            {
                Debug.LogWarning($"Nenhum Grabbable encontrado em {prefab.name} ou filhos.");
            }
        }
    }


    public void UpdateTextureAuxList(List<string> sourceList, int chunkSize, int startIndex)
    {
        TextureAuxPrefabList.Clear();

        if (sourceList == null || sourceList.Count == 0 || chunkSize <= 0)
        {
            Debug.LogWarning("Lista inv�lida ou chunkSize <= 0.");
            return;
        }

        // Garantir startIndex v�lido dentro do intervalo da lista
        startIndex = startIndex % sourceList.Count;

        for (int i = 0; i < chunkSize; i++)
        {
            int index = (startIndex + i) % sourceList.Count; // �ndice circular
            TextureAuxPrefabList.Add(sourceList[index]);
        }

        Debug.Log($"Adicionados {chunkSize} elementos � AuxPrefabList a partir do �ndice {startIndex} (circular).");
    }

    private bool canClick = true;
    public void CycleInstantiatedTextureChunk()
    {
        if (TextureEditMode == true)
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

            DeactivateTextureEditMode();
            currentStartIndex = (currentStartIndex + ChunkSize) % currentTextureList.Count;
            UpdateTextureAuxList(currentTextureList, ChunkSize, currentStartIndex);
            InstantiateTextureListUI(TextureAuxPrefabList);
            TextureEditMode = true;
        }
    }

    public void CycleInstantiatedTextureChunkBackwards()
    {
        if (TextureEditMode == true)
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

            DeactivateTextureEditMode();

            // Atualiza o �ndice para tr�s, garantindo que continue no intervalo correto
            currentStartIndex = (currentStartIndex - ChunkSize + currentTextureList.Count) % currentTextureList.Count;

            UpdateTextureAuxList(currentTextureList, ChunkSize, currentStartIndex);
            InstantiateTextureListUI(TextureAuxPrefabList);
            TextureEditMode = true;
        }
    }

    private IEnumerator EnableClickAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canClick = true;
    }
}
