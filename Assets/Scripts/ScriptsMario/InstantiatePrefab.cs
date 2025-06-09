using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using Oculus.Interaction;
using Unity.VisualScripting;

public class InstantiatePrefab : MonoBehaviour
{   public static InstantiatePrefab Instance;
    public Camera sceneCamera;
    public List<GameObject> Prefabs;

    void Start()
    {
        Instance = this;
    }

    

    void AddFurnitureBehaviour(GameObject prefab)
    {
        var child = prefab.transform.Find("ISDK_RayGrabInteraction").gameObject;

        var transformer = child.GetComponent<FurnitureGrabTransformer>();
        if (transformer == null)
        {
            transformer = child.AddComponent<FurnitureGrabTransformer>();
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

    //public void InstantiateObject(GameObject prefab)
    //{
    //    if (!canClick) return;

    //    canClick = false;
    //    Invoke(nameof(ResetClick), clickCooldown);

    //    Debug.Log("Clique detectado!");

    //    Vector3 position = sceneCamera.transform.position + sceneCamera.transform.forward * 1.0f;
    //    //Quaternion rotation = Quaternion.identity;
    //    var obj = Instantiate(prefab, position, prefab.gameObject.transform.rotation);

    //    // Adiciona Rigidbody se n�o tiver
    //    if (!obj.TryGetComponent<Rigidbody>(out var rb))
    //    {
    //        rb = obj.AddComponent<Rigidbody>();
    //    }
    //    // Desativa o isKinematic para garantir que a f�sica vai funcionar
    //    rb.isKinematic = false;

    //    // Ativa a gravidade
    //    rb.useGravity = true;
    //    rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
    //    // Desmarca o IsTrigger do Collider (caso o prefab tenha um)
    //    Collider collider = obj.GetComponent<Collider>();
    //    if (collider != null)
    //    {
    //        collider.isTrigger = false; // Desativa o "Is Trigger"
    //    }
    //    AddFurnitureBehaviour(obj);

    //}

    public GameObject InstantiateObject(string pathPrefab)
    {
        if (!canClick) return null;

        canClick = false;
        Invoke(nameof(ResetClick), clickCooldown);

        Debug.Log("Clique detectado!");

        Vector3 position = sceneCamera.transform.position + sceneCamera.transform.forward * 1.0f;
    
        //var obj = Instantiate(prefab, position, prefab.transform.rotation);
        var obj = GetComponent<FurnitureSpawner>().SpawnPrefab(pathPrefab, position, Quaternion.identity);

        if (!obj.TryGetComponent<Rigidbody>(out var rb))
            rb = obj.AddComponent<Rigidbody>();

        

        rb.isKinematic = false;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezePositionX |
                         RigidbodyConstraints.FreezePositionZ |
                         RigidbodyConstraints.FreezeRotationZ |
                         RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationY;

        Collider collider = obj.GetComponent<Collider>();
        if (collider != null)
            collider.isTrigger = false;

        //AddFurnitureBehaviour(obj);

        return obj; // <-- agora retorna o objeto instanciado
    }

    void ResetClick()
    {
        canClick = true;
    }
}
