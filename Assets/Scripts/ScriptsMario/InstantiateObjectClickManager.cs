using Oculus.Interaction;
using UnityEngine;

public class InstantiateObjectClickManager : MonoBehaviour
{
    private Grabbable grabbable;
    public string FurnitureModelPrefabPath { get; set; }
    public FurnitureType FurnitureType { get; set; }

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
            Debug.LogError("RayInteractable n�o encontrado!");
        }
    }

    private void HandleClick()
    {
        Transform target = transform.parent;

        InstantiatePrefabUI.Instance.DeactivateAddFurnitureMode();
        EditFurnitureManager.Instance.DeactivateEditFurnitureMode();
        InstantiatePrefabUI.Instance.InstantiateFurnitureMode = false;

        Debug.Log("Objeto clicado: " + target.name);

            Debug.Log("Entrou na tag");

            // Aqui pegamos o prefab original
            OriginalPrefabHolder holder = target.GetComponent<OriginalPrefabHolder>();
            if (holder == null || holder.originalPrefab == null)
            {
                Debug.LogError("OriginalPrefabHolder n�o encontrado ou prefab n�o atribu�do!");
                return;
            }

            GameObject newObject = InstantiatePrefab.Instance.InstantiateObject(FurnitureModelPrefabPath,FurnitureType);

            // Remove managers indesejados
            var componentsToRemove = newObject.GetComponentsInChildren<InstantiateObjectClickManager>(true);
            foreach (var comp in componentsToRemove)
            {
                Destroy(comp);
            }

            // Ativa Grabbable
            var grabbable = newObject.GetComponentInChildren<Grabbable>(true);
            if (grabbable != null)
            {
                grabbable.enabled = true;
            }
        
    }


}
