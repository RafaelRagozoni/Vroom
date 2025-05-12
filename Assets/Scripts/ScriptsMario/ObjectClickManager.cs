using UnityEngine;
using Oculus.Interaction;

public class ObjectClickManager : MonoBehaviour
{
    private Renderer parentRenderer;
    private float lastClickTime = 0f;
    private float clickThreshold = 0.3f; // Tempo entre cliques para considerar duplo clique
    private bool oneClickPending = false;

    private Grabbable grabbable;

    private void Start()
    {
        grabbable = GetComponent<Grabbable>();
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
            if(DeleteManager.Instance.DeleteMode)
            {
                // Modo de deleção
                Debug.Log("Deletando objeto: " + transform.parent.name);
                Destroy(transform.parent.gameObject);
            }
            
            //ObjectEditorUI.Instance.CloseEditor();
        }
    }

    private void TriggerDoubleClick()
    {
        Debug.Log("Duplo clique detectado!");

        Transform target = transform.parent;

        // Tenta obter a altura do objeto via Collider
        //Collider collider = target.GetComponent<Collider>();
        //if (collider != null)
        //{
        //    float altura = collider.bounds.size.y;
        //    Vector3 posicaoUI = target.position + new Vector3(0, altura + 0.1f, 0);
        //    ObjectEditorUI.Instance.OpenEditor(target.gameObject, posicaoUI);
        //}
        //else
        //{
        //    Debug.LogWarning("Nenhum Collider encontrado para calcular a altura.");
        //    Vector3 fallbackPosition = target.position + new Vector3(0, 0.5f, 0);
        //    ObjectEditorUI.Instance.OpenEditor(target.gameObject, fallbackPosition);
        //}
    }

}
