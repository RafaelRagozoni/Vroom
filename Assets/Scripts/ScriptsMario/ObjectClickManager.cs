using UnityEngine;
using Oculus.Interaction;

public class ObjectClickManager : MonoBehaviour
{
    private Renderer parentRenderer;
    private float lastClickTime = 0f;
    private float clickThreshold = 0.3f; // Tempo entre cliques para considerar duplo clique
    private bool oneClickPending = false;

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
        float currentTime = Time.time;

        if (currentTime - lastClickTime < clickThreshold)
        {
            // Clique duplo
            oneClickPending = false;
            lastClickTime = 0f;
            CancelInvoke(nameof(TriggerSingleClick)); // Cancela o clique único, se estava agendado
            TriggerDoubleClick();
        }
        else
        {
            // Possível clique único, espera um pouco pra ver se vem outro
            oneClickPending = true;
            lastClickTime = currentTime;
            Invoke(nameof(TriggerSingleClick), clickThreshold);
        }
    }

    private void TriggerSingleClick()
    {
        if (oneClickPending)
        {
            oneClickPending = false;
            ObjectEditorUI.Instance.CloseEditor();
        }
    }

    private void TriggerDoubleClick()
    {
        Debug.Log("Duplo clique detectado!");
        ObjectEditorUI.Instance.OpenEditor(transform.parent.gameObject);
    }
}
