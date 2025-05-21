using Oculus.Interaction;
using UnityEngine;

public class ChangeTextureClickManager : MonoBehaviour
{
    private Transform ObjectCLickedTarget; //Parede ou chao que foi clicado

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        Transform objectClickedTarget = InstantiateTexturesUI.Instance.selectedTarget;
        Debug.Log("Objeto recebido: " + objectClickedTarget.name);
        //InstantiateTexturesUI.Instance.DeactivateTextureEditMode();
        if (objectClickedTarget == null)
        {
            Debug.LogWarning("Nenhum alvo selecionado para aplicar a textura!");
            return;
        }
        
        //Renderer targetRenderer = transform.parent.GetComponent<Renderer>();
        Renderer targetRenderer = transform.parent.GetComponentInChildren<Renderer>();

        Renderer clickedTargetRenderer = objectClickedTarget.GetComponent<Renderer>();

        if (targetRenderer == null)
            Debug.LogWarning("targetRenderer é NULL! Objeto: " + transform.parent.name);
        if (clickedTargetRenderer == null)
            Debug.LogWarning("clickedTargetRenderer é NULL! Objeto: " + objectClickedTarget.name);
        
        Debug.Log("Antes");
        if (targetRenderer != null && clickedTargetRenderer != null)
        {
            Debug.Log("Tentou trocar o material");
            clickedTargetRenderer.material = targetRenderer.material;
        }
        else
        {
            Debug.LogWarning("Renderer não encontrado em um dos objetos.");
        }
        Debug.Log("Depois");
        InstantiateTexturesUI.Instance.DeactivateTextureEditMode();
    }
}
