using Oculus.Interaction;
using UnityEngine;

public class OpenUI : MonoBehaviour
{
    public GameObject Canvas;
    public Camera sceneCamera;

    private RayInteractable interactable;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void OpenUIFunction()
    {
        // Define a distância da UI à frente da câmera
        float distanceFromCamera = 1.0f;

        // Posiciona a UI à frente da câmera
        Canvas.transform.position = sceneCamera.transform.position + sceneCamera.transform.forward * distanceFromCamera;

        // Faz o canvas olhar para a câmera
        Canvas.transform.LookAt(sceneCamera.transform);

        // Corrige a rotação para que a UI não fique espelhada
        Canvas.transform.rotation = Quaternion.Euler(0, Canvas.transform.rotation.eulerAngles.y + 180, 0);

        // Ativa a UI
        Canvas.SetActive(true);
    }
}
