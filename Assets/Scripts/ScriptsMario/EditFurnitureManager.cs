using UnityEngine;

public class EditFurnitureManager : MonoBehaviour
{
    // Acesso global � inst�ncia
    public static EditFurnitureManager Instance { get; private set; }
    public GameObject EditPrefab; // Prefab do objeto de edi��o
    public GameObject gizmoInstance; // Prefab do gizmo de escala
    public Camera sceneCamera;
    public bool EditFunitureMode;
    public float minScale = 0.75f;
    public float maxScale = 1.25f;

    public Vector3 Gizmo;
    public GameObject FurnitureGizmo;
    // Refer�ncia ao objeto instanciado na cena
    public Transform selectedTarget;
    private GameObject editInstance;
    void Awake()
    {
        minScale = 0.75f;
        maxScale = 1.25f;
        // Se j� existe uma inst�ncia e n�o � esta, destrua este objeto
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        // Sen�o, define esta como a inst�ncia �nica
        Instance = this;

        // Opcional: mant�m o objeto ao trocar de cena
        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EditFunitureMode = false;
    }

    // M�todo para ativar o modo de edi��o e instanciar o objeto
    public void ActivateEditFurnitureMode(Vector3 position, Quaternion rotation)
    {
        EditFunitureMode = true;
        if (EditPrefab != null && editInstance == null)
        {
            editInstance = Instantiate(EditPrefab, position, rotation);
            editInstance.transform.LookAt(sceneCamera.transform, Vector3.up);
            editInstance.transform.Rotate(0, 0, 0);
        }
    }

    public void DeactivateEditFurnitureMode()
    {
        EditFunitureMode = false;
        if (editInstance != null)
        {
            Destroy(editInstance); // Destroi apenas a inst�ncia na cena
            editInstance = null;
        }
        DestroyGizmoInstance();
        
    }

    public void InstantiateGizmoPrefab(Vector3 position, Quaternion rotation)
    {
        gizmoInstance = Instantiate(FurnitureGizmo, position, rotation);
    }

    public void DestroyGizmoInstance()
    {
        if (gizmoInstance != null)
        {
            Destroy(gizmoInstance);
            Debug.Log("Gizmo instance destroyed.");
            gizmoInstance = null;
        }
        else
        {
            Debug.Log("No gizmo instance to destroy.");
        }
    }
}
