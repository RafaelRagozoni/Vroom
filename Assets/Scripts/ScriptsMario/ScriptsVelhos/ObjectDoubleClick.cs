using UnityEngine;
using Oculus.Interaction;

public class ObjectDoubleClick : MonoBehaviour
{
    private Renderer parentRenderer;
    private float lastClickTime = 0f;
    private float doubleClickThreshold = 0.3f; // Tempo máximo entre cliques (em segundos) para ser considerado duplo clique

    private void Start()
    {
        parentRenderer = transform.parent.GetComponent<Renderer>();

        RayInteractable interactable = GetComponent<RayInteractable>();

        if (interactable != null)
        {
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
        float currentTime = Time.time;

        if (currentTime - lastClickTime <= doubleClickThreshold)
        {
            //OnDoubleClick();
            lastClickTime = 0f; // reseta para evitar múltiplos duplos cliques em sequência
        }
        else
        {
            lastClickTime = currentTime;
        }
    }

    //private void OnDoubleClick()
    //{
    //    Debug.Log("Antes: " + transform.parent.position);
    //    ObjectEditorUI.Instance.OpenEditor(transform.parent.gameObject);
    //    Debug.Log("Depois: " + transform.parent.position);
    //}


    //private void OnDoubleClick()
    //{
    //    Debug.Log("Duplo clique detectado!");

    //    if (parentRenderer != null && parentRenderer.materials.Length > 0)
    //    {
    //        // Alterna entre vermelho e azul, por exemplo
    //        Color currentColor = parentRenderer.materials[0].color;
    //        if (currentColor == Color.red)
    //        {
    //            parentRenderer.materials[0].color = Color.blue;
    //        }
    //        else
    //        {
    //            parentRenderer.materials[0].color = Color.red;
    //        }
    //    }
    //}
}
