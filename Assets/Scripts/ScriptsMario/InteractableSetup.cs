using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.Surfaces;
using System.Reflection;

public class InteractableSetup : MonoBehaviour
{
    public void AddRayGrabInteraction(GameObject targetObject)
    {
        // Garante que o objeto principal tenha collider e rigidbody
        Collider collider = targetObject.GetComponent<Collider>() ?? targetObject.AddComponent<BoxCollider>();
        Rigidbody rb = targetObject.GetComponent<Rigidbody>() ?? targetObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;

        // Cria o objeto filho
        GameObject rayGrabObj = new GameObject("ISDK_RayGrabInteraction");
        rayGrabObj.transform.SetParent(targetObject.transform, false);

        // Adiciona Grabbable
        var grabbable = rayGrabObj.AddComponent<Grabbable>();
        SetPrivate(grabbable, "_rigidbody", rb);
        SetPrivate(grabbable, "_kinematicWhileSelected", true);
        SetPrivate(grabbable, "_throwWhenUnselected", true);
        SetPrivate(grabbable, "_targetTransform", targetObject.transform);

        // Adiciona MoveFromTargetProvider
        var moveProvider = rayGrabObj.AddComponent<MoveFromTargetProvider>();

        // Adiciona ColliderSurface
        var colliderSurface = rayGrabObj.AddComponent<ColliderSurface>();
        SetPrivate(colliderSurface, "_collider", collider);

        // Adiciona RayInteractable e desativa temporariamente
        var rayInteractable = rayGrabObj.AddComponent<RayInteractable>();
        rayInteractable.enabled = false;

        SetPrivate(rayInteractable, "_pointableElement", grabbable);
        SetPrivate(rayInteractable, "_selectSurface", colliderSurface);
        SetPrivate(rayInteractable, "_movementProvider", moveProvider);

        rayInteractable.enabled = true;

        Debug.Log("ISDK RayGrabInteraction configurado com sucesso!");
    }

    private void SetPrivate<T>(T obj, string fieldName, object value)
    {
        var type = typeof(T);
        var field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        if (field != null)
        {
            field.SetValue(obj, value);
        }
        else
        {
            Debug.LogWarning($"Campo privado '{fieldName}' não encontrado em {type}");
        }
    }
}
