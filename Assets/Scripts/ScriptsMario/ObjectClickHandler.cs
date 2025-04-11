using UnityEngine;
using Oculus.Interaction;

public class ObjectClickHandler : MonoBehaviour
{
    private Renderer parentRenderer;
    int i;
    private void Start()
    {
        i = 0;
        // Obtém o componente Renderer do objeto pai
        parentRenderer = transform.parent.GetComponent<Renderer>();

        // Verifica se o componente RayInteractable está presente no filho
        RayInteractable interactable = GetComponent<RayInteractable>();

        if (interactable != null)
        {
            // Monitora mudanças de estado para detectar quando o cubo (filho) é selecionado
            interactable.WhenStateChanged += (args) =>
            {
                if (args.NewState == InteractableState.Select)
                {
                    OnClick();
                }
            };
        }
        else
        {
            Debug.LogError("RayInteractable não encontrado!");
        }
    }
    private void OnClick()
    {
        ObjectEditorUI.Instance.CloseEditor();
    }
    //private void OnClick()
    //{
    //    i = i + 1;
    //    i = i % 2;
    //    // Verifica se o objeto pai tem materiais e altera a cor do primeiro material
    //    if (parentRenderer != null && parentRenderer.materials.Length > 0)
    //    {
    //        if (i == 0)
    //        {
    //            parentRenderer.materials[0].color = Color.red; // Muda a cor do primeiro material
    //        }
    //        if (i == 1)
    //        {
    //            parentRenderer.materials[0].color = Color.blue;
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogWarning("O objeto pai não tem materiais ou o Renderer não foi encontrado!");
    //    }
    //}
}
