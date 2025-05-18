using Oculus.Interaction;
using UnityEngine;

public class InstantiateObjectClickManager : MonoBehaviour
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

        // Adicione aqui o que deve acontecer ao clicar no objeto
        InstantiatePrefabUI.Instance.DeactivateAddFurnitureMode();
        InstantiatePrefabUI.Instance.InstantiateFurnitureMode = false;
        Debug.Log("Objeto clicado: " + target.name);

        if (target.CompareTag("ChairListTag"))
        {
            Debug.Log("Entrou na tag");

            // Instancia o objeto e guarda a referência
            GameObject newObject = InstantiatePrefab.Instance.InstantiateObject(target.gameObject);

            // Remove todos os InstantiateObjectClickManager do objeto instanciado e seus filhos
            var componentsToRemove = newObject.GetComponentsInChildren<InstantiateObjectClickManager>(true);
            foreach (var comp in componentsToRemove)
            {
                Destroy(comp);
                Debug.Log("Removido InstantiateObjectClickManager de " + comp.gameObject.name);
            }

            // Ativa o componente Grabbable no objeto instanciado
            var grabbable = newObject.GetComponentInChildren<Grabbable>(true);
            if (grabbable != null)
            {
                grabbable.enabled = true; // Habilita o componente Grabbable
                Debug.Log($"Ativado Grabbable no filho de {newObject.name}");
            }
        }
    }

}
