using Oculus.Interaction;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CategoriesObjectClickManager : MonoBehaviour
{
    private Grabbable grabbable;
    private int ChunkSize;
    public int index = 0;
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
        InstantiatePrefabUI.Instance.InstantiateFurnitureMode = true;
        InstantiatePrefabUI.Instance.CategoriesAddFurnitureMode = false;
        Debug.Log("Objeto clicado: " + target.name);
        if(target.CompareTag("ChairListTag"))
        {
            Debug.Log("Entrou na tag");
            ChunkSize = InstantiatePrefabUI.Instance.ChunkSize;
            InstantiatePrefabUI.Instance.currentCategoryList = InstantiatePrefabUI.Instance.ChairList;
            InstantiatePrefabUI.Instance.UpdateAuxList(InstantiatePrefabUI.Instance.currentCategoryList,ChunkSize, 0);
            InstantiatePrefabUI.Instance.InstantiateListUI(InstantiatePrefabUI.Instance.AuxPrefabList);
        }

        if (target.CompareTag("BedListTag"))
        {
            Debug.Log("Entrou na tag");
            ChunkSize = InstantiatePrefabUI.Instance.ChunkSize;
            InstantiatePrefabUI.Instance.currentCategoryList = InstantiatePrefabUI.Instance.BedList;
            InstantiatePrefabUI.Instance.UpdateAuxList(InstantiatePrefabUI.Instance.currentCategoryList, ChunkSize, 0);
            InstantiatePrefabUI.Instance.InstantiateListUI(InstantiatePrefabUI.Instance.AuxPrefabList);
        }



    }

}
