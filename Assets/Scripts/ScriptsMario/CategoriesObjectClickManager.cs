using Oculus.Interaction;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CategoriesObjectClickManager : MonoBehaviour
{
    private Grabbable grabbable;
    private void Start()
    {
        grabbable = GetComponent<Grabbable>();
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

        //Adicione aqui o que deve acontecer ao clicar no objeto
        // Exemplo:
        InstantiatePrefabUI.Instance.DeactivateAddFurnitureMode();
        Debug.Log("Objeto clicado: " + target.name);
        if(target.CompareTag("ChairListTag"))
        {
            Debug.Log("Entrou na tag");
            InstantiatePrefabUI.Instance.InstantiateListUI(InstantiatePrefabUI.Instance.ChairList);
        }
    }

}
