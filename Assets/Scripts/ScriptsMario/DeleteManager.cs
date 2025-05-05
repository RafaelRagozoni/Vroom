using UnityEngine;

public class DeleteManager : MonoBehaviour
{
    public static DeleteManager Instance;
    public bool DeleteMode = false;
    public GameObject Lixeira;
    public Camera sceneCamera;
    private float distanceFromCamera = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (DeleteMode == true)
        {
            Lixeira.transform.position = sceneCamera.transform.position + sceneCamera.transform.forward * distanceFromCamera;
            Lixeira.transform.LookAt(sceneCamera.transform);
        }     
   }

    public void DeletionMode()
    {
        if (DeleteMode)
        {
            DeleteMode = false;
            Lixeira.SetActive(false);
        }
        else
        {
            DeleteMode = true;
            Lixeira.SetActive(true);
        }

    }
}
