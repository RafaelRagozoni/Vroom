using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using Oculus.Interaction;

public class InstantiatePrefab : MonoBehaviour
{
    public Camera sceneCamera;
    public List<GameObject> Prefabs;
    public GameObject Container;
    public GameObject buttonPrefab;

    void Start()
    {
        GenerateButtons();
    }

    void GenerateButtons()
    {
        // Remove bot�es antigos, se houver
        foreach (Transform child in Container.transform)
        {
            Destroy(child.gameObject);
        }

        // Gera bot�es dinamicamente para cada prefab na lista
        for (int i = 0; i < Prefabs.Count; i++)
        {
            //AddFurnitureBehaviour(Prefabs[i]); // Adiciona o comportamento de mob�lia ao prefab
            Debug.Log($"Criando bot�o para {Prefabs[i].name}"); // Loga o nome do prefab

            GameObject button = Instantiate(buttonPrefab, Container.transform, false); // Instancia bot�o corretamente
            button.name = "Button_" + Prefabs[i].name; // Nomeia o bot�o corretamente

            // Configura o texto do bot�o
            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>(true); // O 'true' garante que busca at� em objetos inativos
            if (buttonText != null)
            {
                buttonText.text = Prefabs[i].name; // Define o nome correto do prefab no bot�o
            }
            else
            {
                Debug.LogError($"O bot�o {button.name} n�o tem um componente TMP_Text!");
            }

            // Corrige refer�ncia para evitar closures
            GameObject prefabCopy = Prefabs[i];

            // Adiciona a fun��o ao bot�o
            button.GetComponent<Button>().onClick.AddListener(() => InstantiateObject(prefabCopy));
        }
    }

    void AddFurnitureBehaviour(GameObject prefab)
    {
        var child = prefab.transform.Find("ISDK_RayGrabInteraction").gameObject;

        var transformer = child.GetComponent<GrabFreeTransformer>();
        if (transformer == null)
        {
            transformer = child.AddComponent<GrabFreeTransformer>();

        }
        transformer.gridSize = gridSize;
        transformer.righHandInteractor = rayInteractor;
        transformer.rayStart = rayStart;
        
        

        if (child.GetComponent<ObjectClickManager>() == null)
        {
            var clickHandler = child.AddComponent<ObjectClickManager>();
        }

        var grabable = child.GetComponent<Grabbable>();

        grabable.InjectOptionalOneGrabTransformer(transformer);

        GameObject markerObj = Instantiate(marker);
        markerObj.transform.SetParent(prefab.transform);
        markerObj.transform.localPosition = Vector3.zero;
        //markerObj.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        markerObj.GetComponent<Collider>().enabled = false;
        //Material mat = Resources.Load<Material>("Assets/Materials/Blue Glow.mat");
        //markerObj.GetComponent<Renderer>().material = mat;
        transformer.marker = markerObj;
    }

    private bool canClick = true;
    private float clickCooldown = 0.1f;
    public float gridSize = 1.0f;
    public RayInteractor rayInteractor;
    public GameObject marker;
    public float rayStart = 1.0f;

    //void InstantiateObject(GameObject prefab)
    //{
    //    if (!canClick) return; // Se o clique j� foi registrado, ignora.

    //    canClick = false; // Bloqueia cliques adicionais por um curto tempo.
    //    Invoke(nameof(ResetClick), clickCooldown); // Reativa o clique ap�s cooldown.

    //    Debug.Log("Clique detectado!");

    //    Vector3 position = sceneCamera.transform.position + sceneCamera.transform.forward * 1.0f;
    //    //Quaternion rotation = Quaternion.LookRotation(sceneCamera.transform.forward, Vector3.up);
    //    //rotation *= Quaternion.Euler(0, 90, 0); // Ajusta rota��o
    //    var rotation = Quaternion.identity; // Mant�m a rota��o original
    //    var Object=Instantiate(prefab, position, rotation);
    //    AddFurnitureBehaviour(Object);
    //}

    void InstantiateObject(GameObject prefab)
    {
        if (!canClick) return;

        canClick = false;
        Invoke(nameof(ResetClick), clickCooldown);

        Debug.Log("Clique detectado!");

        Vector3 position = sceneCamera.transform.position + sceneCamera.transform.forward * 1.0f;
        Quaternion rotation = Quaternion.identity;
        var obj = Instantiate(prefab, position, rotation);

        // Adiciona Rigidbody se n�o tiver
        if (!obj.TryGetComponent<Rigidbody>(out var rb))
        {
            rb = obj.AddComponent<Rigidbody>();
        }
        // Desativa o isKinematic para garantir que a f�sica vai funcionar
        rb.isKinematic = false;

        // Ativa a gravidade
        rb.useGravity = true;

        // Desmarca o IsTrigger do Collider (caso o prefab tenha um)
        Collider collider = obj.GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = false; // Desativa o "Is Trigger"
        }

        // Adiciona o script que desativa a gravidade ap�s colis�o
        obj.AddComponent<DisableGravityOnCollision>();

        AddFurnitureBehaviour(obj);

        var child = obj.transform.Find("ISDK_RayGrabInteraction");
        if (child != null)
        {
            child.gameObject.SetActive(false);
        }

    }

    void ResetClick()
    {
        canClick = true;
    }
}
