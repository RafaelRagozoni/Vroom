using Oculus.Interaction;
using UnityEngine;

public class ScaleFurnitureClickManager : MonoBehaviour
{
    public GameObject gizmoPrefab; // Prefab do gizmo de escala
    public Camera sceneCamera; // Câmera da cena, se necessário
    private void Start()
    {
        gizmoPrefab = EditFurnitureManager.Instance.FurnitureGizmo;
        sceneCamera = EditFurnitureManager.Instance.sceneCamera;
        RayInteractable interactable = GetComponent<RayInteractable>();

        if (interactable != null)
        {
            interactable.WhenStateChanged += (args) =>
            {
                if (args.NewState == InteractableState.Select)
                {
                    HandleClick();
                }
            };
        }
        else
        {
            Debug.LogError("RayInteractable não encontrado!");
        }
    }

    private void HandleClick()
    {
        Transform target = transform.parent;
        Transform ObjectClicked = EditFurnitureManager.Instance.selectedTarget;
        if (ObjectClicked == null)
        {
            Debug.LogWarning("Nenhum objeto selecionado para escalar.");
            return;
        }

        // Desativa o modo de edição de móveis
        EditFurnitureManager.Instance.DeactivateEditFurnitureMode();
        EditFurnitureManager.Instance.EditFunitureMode = true;
        // Log para verificar o objeto a ser escalado
        Debug.Log("Objeto a ser Escalado: " + ObjectClicked.name);

        // Supondo que você já tem o prefab do gizmo
        //EditFurnitureManager.Instance.InstantiateGizmoPrefab(ObjectClicked.position + Vector3.up, Quaternion.Euler(-90, 0, 0));
        // Distância desejada à frente da câmera
        float distancia = 2.0f;

        // Posição na frente da câmera
        Vector3 posicaoNaFrente = sceneCamera.transform.position + sceneCamera.transform.forward * distancia;

        // Instancia o gizmo na frente da câmera
        EditFurnitureManager.Instance.InstantiateGizmoPrefab(posicaoNaFrente, Quaternion.Euler(-90, 0, 0));
    }
    
}
