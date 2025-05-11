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

    private float lastCallTime = 0f;
    private float callCooldown = 0.3f; // 300 ms de intervalo m�nimo

    public void DeletionMode()
    {
        if (Time.time - lastCallTime < callCooldown)
        {
            Debug.Log("Ignorado: chamada muito r�pida");
            return; // Ignora se a fun��o foi chamada h� menos de 0.3s
        }

        lastCallTime = Time.time;

        if (DeleteMode)
        {
            Debug.Log("Modo de Dele��o Desativado");
            DeleteMode = false;
            Lixeira.SetActive(false);
            Debug.Log("Lixeira Desativada");
        }
        else
        {
            Debug.Log("Modo de Dele��o Ativado");
            DeleteMode = true;
            Lixeira.SetActive(true);
            Debug.Log("Lixeira Ativada");
        }
    }
}
