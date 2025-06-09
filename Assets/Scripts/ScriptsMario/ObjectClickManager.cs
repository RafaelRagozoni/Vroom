using UnityEngine;
using Oculus.Interaction;
using Bhaptics.SDK2;
public class ObjectClickManager : MonoBehaviour
{
    private Renderer parentRenderer;
    private float lastClickTime = 0f;
    private float clickThreshold = 0.3f; // Tempo entre cliques para considerar duplo clique
    private bool oneClickPending = false;

    private Grabbable grabbable;
    public Camera sceneCamera;
    public GameObject Editprefab;
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
                // Modo de dele��o
                Debug.Log("Deletando objeto: " + transform.parent.name);
                Destroy(transform.parent.gameObject);
                Debug.Log("Tentando tocar evento Bhaptics: teste_aula");
                BhapticsLibrary.Play("teste_aula",0,      // Delay Time (millisecond)
            1.0f,   // Haptic intensity
            1.0f,   // Haptic duration
            20.0f,  // Rotate haptic around global Vector3.up (0f - 360f)
            0.3f    // Move haptic up or down (-0.5f - 0.5f)
        );
            }
            EditFurnitureManager.Instance.DeactivateEditFurnitureMode();

        }
    }

    private void TriggerDoubleClick()
    {
        Debug.Log("Duplo clique detectado no objeto!");
        InstantiateTexturesUI.Instance.DeactivateTextureEditMode();
        InstantiatePrefabUI.Instance.DeactivateAddFurnitureMode();
        DeleteManager.Instance.DeactivateDeletionMode();
        EditFurnitureManager.Instance.selectedTarget = null;///////////////
        if (EditFurnitureManager.Instance.EditFunitureMode == false)
        {

            if (Editprefab != null)
            {
                Transform target = transform.parent;
                Transform model= target.Find("raw_model(Clone)");
                if (model == null)
                {
                   Debug.LogError("Modelo filho não encontrado!");
                }

                EditFurnitureManager.Instance.selectedTarget = model;
                // Tenta obter a altura do objeto via Collider
                Collider collider = target.GetComponent<Collider>();
                Vector3 posicaoAcima;
                if (collider != null)
                {
                    float altura = collider.bounds.size.y;
                    posicaoAcima = target.position + new Vector3(0, altura + 0.3f, 0); // 0.1f para evitar sobreposi��o
                }
                else
                {
                    Debug.LogWarning("Nenhum Collider encontrado para calcular a altura. Usando valor padr�o.");
                    posicaoAcima = target.position + new Vector3(0, 0.5f, 0);
                }

                Quaternion rotacao = Quaternion.identity; // Sem rota��o espec�fica, ajuste se necess�rio
                EditFurnitureManager.Instance.ActivateEditFurnitureMode(posicaoAcima, rotacao);
                //Instantiate(Editprefab, posicaoAcima, rotacao);
                Debug.Log("Prefab instanciado acima do m�vel.");
            }
            else
            {
                Debug.LogWarning("Prefab para instanciar n�o est� atribu�do!");
            }
        }
    }

}
