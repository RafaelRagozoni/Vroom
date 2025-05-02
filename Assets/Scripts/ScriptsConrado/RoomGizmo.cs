using UnityEngine;

public class RoomGizmo : MonoBehaviour
{

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 localForward = transform.forward;
        Vector3 delta = controllerPosition - initialControllerPosition;
        float pullAmount = Vector3.Dot(delta, localForward); // Project movement onto forward
        transform.position = initialGizmoPosition + localForward * pullAmount;
    }
}
