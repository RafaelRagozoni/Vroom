using UnityEngine;
using Oculus.Interaction;
using System.Collections;

public class FurnitureScaler : MonoBehaviour
{
    private float baseY;
    private float initialX;
    private float initialZ;
    private Transform target;
    private RayInteractable interactable;

    [Header("Limites de escala (relativos à escala base)")]
    private float minScale; // 0.5x da escala base
    private float maxScale;   // 2x da escala base

    [Header("Escala base de referência")]
    public Vector3 baseScale = Vector3.one; // Defina no Inspector se necessário

    private GrabFreeTransformer grabFreeTransformer;

    void Start()
    {
        minScale = EditFurnitureManager.Instance.minScale;
        maxScale = EditFurnitureManager.Instance.maxScale;
        target = EditFurnitureManager.Instance.selectedTarget;
        if (target != null)
        {
            initialX = target.position.x;
            initialZ = target.position.z;
            float currentScaleFactor = target.localScale.x / baseScale.x;
            baseY = target.position.y - (currentScaleFactor - 1);

            if (baseScale == Vector3.zero)
                baseScale = Vector3.one;
            // Posiciona o gizmo na altura proporcional à escala atual do móvel
            transform.position = new Vector3(
                initialX,
                baseY + (currentScaleFactor - 1),
                initialZ
            );
        }

        interactable = GetComponent<RayInteractable>();
        if (interactable != null)
        {
            interactable.WhenStateChanged += OnStateChanged;
        }

        // Configura os limites do GrabFreeTransformer via constraints
        grabFreeTransformer = GetComponent<GrabFreeTransformer>();
        if (grabFreeTransformer != null)
        {
            var constraints = new TransformerUtils.PositionConstraints();
            constraints.YAxis.ConstrainAxis = true;
            constraints.YAxis.AxisRange.Min = baseY + (minScale - 1);
            constraints.YAxis.AxisRange.Max = baseY + (maxScale - 1);


            grabFreeTransformer.InjectOptionalPositionConstraints(constraints);
            Debug.Log("GrabFreeTransformer configurado com limites de posição.");
        }
        else
        {
            Debug.LogError("GrabFreeTransformer não encontrado no objeto " + gameObject.name);
        }
    }

    private void OnStateChanged(InteractableStateChangeArgs args)
    {
        if (target == null) return;

        if (args.NewState == InteractableState.Select)
        {
            float currentScaleFactor = target.localScale.x / baseScale.x;
            // Alinha o Gizmo com o móvel
            transform.position = new Vector3(
                initialX,
                baseY + (currentScaleFactor - 1),
                initialZ
            );
        }
    }

    void Update()
    {
        if (target == null) return;

        if (interactable != null && interactable.State == InteractableState.Select)
        {
            float deltaY = transform.position.y - baseY;
            float scaleFactor = 1 + deltaY;
            scaleFactor = Mathf.Clamp(scaleFactor, minScale, maxScale);

            target.localScale = baseScale * scaleFactor;
        }
    }
}