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
        interactable = GetComponent<RayInteractable>();

        if (interactable != null)
        {
            // Monitora mudanças de estado para detectar quando o cubo (filho) é selecionado
            interactable.WhenStateChanged += (args) =>
            {
                if (args.NewState == InteractableState.Select)
                {
                    OpenUIFunction();
                }
            };
        }
        else
        {
            Debug.LogError("RayInteractable não encontrado!");
        }
    }

    //public void OpenUIFunction()
    //{
    //    // Posiciona a UI 5 unidades à frente da câmera
    //    Canvas.transform.position = sceneCamera.transform.position + sceneCamera.transform.forward * 1.0f;

    //    // Garante que a UI esteja sempre voltada para a câmera
    //    //Canvas.transform.LookAt(sceneCamera.transform);

    //    // Roda a UI em torno do eixo Y para ficar a 90º da câmera
    //    //Canvas.transform.rotation = Quaternion.Euler(0, 180, 0);

    //    Canvas.SetActive(true);
    //}

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
