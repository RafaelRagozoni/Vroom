using Oculus.Interaction;
using UnityEngine;

public class TextureClickManager : MonoBehaviour
{
    public Transform target;
    private int ChunkSize;

    private float lastClickTime = 0f;
    private float clickThreshold = 0.3f; // Tempo entre cliques para considerar duplo clique
    private bool oneClickPending = false;

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
            Debug.LogError("RayInteractable n�o encontrado!");
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
            CancelInvoke(nameof(TriggerSingleClick)); // Cancela o clique �nico, se estava agendado
            TriggerDoubleClick();
        }
        else
        {
            // Poss�vel clique �nico, espera um pouco pra ver se vem outro
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
            if (DeleteManager.Instance.DeleteMode)
            {
               
            }
        }
    }
    private void TriggerDoubleClick()
    {
        target = transform.parent;
        InstantiateTexturesUI.Instance.selectedTarget = transform.parent;
        Debug.Log("Clicou no objeto: " + target.name);
        InstantiateTexturesUI.Instance.DeactivateTextureEditMode();
        InstantiatePrefabUI.Instance.DeactivateAddFurnitureMode();
        DeleteManager.Instance.DeactivateDeletionMode();
        EditFurnitureManager.Instance.DeactivateEditFurnitureMode();

        InstantiateTexturesUI.Instance.TextureEditMode = true;
        if (target.CompareTag("wall"))
        {
            Debug.Log("Entrou na tag Wall");
            ChunkSize =InstantiateTexturesUI.Instance.ChunkSize;
            InstantiateTexturesUI.Instance.currentTextureList = InstantiateTexturesUI.Instance.WallTextureList;
            InstantiateTexturesUI.Instance.UpdateTextureAuxList(InstantiateTexturesUI.Instance.currentTextureList, ChunkSize, 0);
            InstantiateTexturesUI.Instance.InstantiateTextureListUI(InstantiateTexturesUI.Instance.TextureAuxPrefabList);
        }

        if(target.CompareTag("floor"))
        {
            Debug.Log("Entrou na tag floor");
            ChunkSize = InstantiateTexturesUI.Instance.ChunkSize;
            InstantiateTexturesUI.Instance.currentTextureList = InstantiateTexturesUI.Instance.FloorTextureList;
            InstantiateTexturesUI.Instance.UpdateTextureAuxList(InstantiateTexturesUI.Instance.currentTextureList, ChunkSize, 0);
            InstantiateTexturesUI.Instance.InstantiateTextureListUI(InstantiateTexturesUI.Instance.TextureAuxPrefabList);
        }

        if (target.CompareTag("celling"))
        {
            Debug.Log("Entrou na tag celling");
            ChunkSize = InstantiateTexturesUI.Instance.ChunkSize;
            InstantiateTexturesUI.Instance.currentTextureList = InstantiateTexturesUI.Instance.CeilTextureList;
            InstantiateTexturesUI.Instance.UpdateTextureAuxList(InstantiateTexturesUI.Instance.currentTextureList, ChunkSize, 0);
            InstantiateTexturesUI.Instance.InstantiateTextureListUI(InstantiateTexturesUI.Instance.TextureAuxPrefabList);
        }
    }

}
