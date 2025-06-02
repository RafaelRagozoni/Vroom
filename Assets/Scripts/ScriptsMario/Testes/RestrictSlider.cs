using UnityEngine;
using Oculus.Interaction;
using JetBrains.Annotations;
public class RestrictSlider : MonoBehaviour
{   
    private GrabFreeTransformer grabFreeTransformer;
    public Transform P_i;
    public Transform P_f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grabFreeTransformer = GetComponent<GrabFreeTransformer>();
    
        if (grabFreeTransformer != null)
        {
            var constraints = new TransformerUtils.PositionConstraints();
            constraints.ConstraintsAreRelative = true; // ABSOLUTO
            constraints.ZAxis.ConstrainAxis = true;
            constraints.ZAxis.AxisRange.Min = Mathf.Min(P_i.position.z, P_f.position.z);
            constraints.ZAxis.AxisRange.Max = Mathf.Max(P_i.position.z, P_f.position.z);
            // Aqui restringe o eixo X (horizontal)
            constraints.XAxis.ConstrainAxis = true;                                         
            constraints.XAxis.AxisRange.Min = 0f;
            constraints.XAxis.AxisRange.Max = 0f;
            // Aqui restringe o eixo Y (altura)
            constraints.YAxis.ConstrainAxis = true;
            constraints.YAxis.AxisRange.Min = 0f;
            constraints.YAxis.AxisRange.Max = 0f;

            grabFreeTransformer.InjectOptionalPositionConstraints(constraints);
            Debug.Log("GrabFreeTransformer configurado com limites de posição.");
        }
        else
        {
            Debug.LogError("GrabFreeTransformer não encontrado no objeto " + gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
