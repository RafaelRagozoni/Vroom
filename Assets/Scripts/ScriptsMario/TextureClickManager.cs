using Oculus.Interaction;
using UnityEngine;

public class TextureClickManager : MonoBehaviour
{
    public Transform target;
    private int ChunkSize;

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
            Debug.LogError("RayInteractable não encontrado!");
        }
    }

    private void HandleClick()
    {
        target = transform.parent;
        InstantiateTexturesUI.Instance.selectedTarget = transform.parent;
        Debug.Log("Clicou no objeto: " + target.name);
        InstantiateTexturesUI.Instance.DeactivateTextureEditMode();
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
