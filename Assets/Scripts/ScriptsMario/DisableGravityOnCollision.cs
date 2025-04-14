using Oculus.Interaction;
using UnityEngine;

public class DisableGravityOnCollision : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Aqui voc� pode checar se bateu especificamente no ch�o, se quiser
        // if (collision.gameObject.CompareTag("Chao")) { ... }

        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true; // congela no lugar
                               // Se n�o quiser congelar, remova essa linha
                               // Reativa o script de grab (se necess�rio)

        var child = this.transform.Find("ISDK_RayGrabInteraction");
        if (child != null)
        {
            child.gameObject.SetActive(true);
        }
        Destroy(this); // remove o script depois de executar
    }
}
