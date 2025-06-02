using UnityEngine;
using Oculus.Interaction; // Add this if Grabbable is from Oculus Integration, otherwise use the correct namespace

public class MirrorClickManager : MonoBehaviour
{
    private void Start()
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
        Transform target = transform.parent;
        Transform ObjectClicked=EditFurnitureManager.Instance.selectedTarget;
        if (ObjectClicked == null)
        {
            Debug.LogWarning("Nenhum objeto selecionado para espelhar.");
            return;
        }
        
        //Invertendo a escala do objeto selecionado
        Vector3 localScale = ObjectClicked.localScale;
        localScale.x *= -1; // Inverte a escala X
        ObjectClicked.localScale = localScale;

        // Desativa o modo de edição de móveis
        EditFurnitureManager.Instance.DeactivateEditFurnitureMode();
        // Log para verificar o objeto espelhado
        Debug.Log("Objeto Espelhado: " + ObjectClicked.name);

    }
    
}
