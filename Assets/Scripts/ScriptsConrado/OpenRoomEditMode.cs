using UnityEngine;
using Oculus.Interaction;
using System.Collections;


public class OpenRoomEditMode : MonoBehaviour
{
    public GameObject sceneCamera;
    public bool dentro = true;
    private bool canRun = true;
    public RayInteractor rayInteractorRight;
    public RayInteractor rayInteractorLeft;

    public Transform oculusCursorRight;
    public Transform oculusCursorLeft;
    public Transform selectionCircleRight;
    public Transform selectionCircleLeft;

    public RoomReshaper roomReshaper;


    public void OpenEditRoomModeFunction()
    {
        InstantiateTexturesUI.Instance.DeactivateTextureEditMode();
        InstantiatePrefabUI.Instance.DeactivateAddFurnitureMode();

        sceneCamera.transform.position = new Vector3(20, 25, 20);
        rayInteractorRight.MaxRayLength = 100;
        rayInteractorLeft.MaxRayLength = 100;
        oculusCursorRight.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        oculusCursorLeft.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        selectionCircleRight.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        selectionCircleLeft.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        //roomReshaper.enabled = true;
    }

    public void CloseEditRoomModeFunction()
    {
        InstantiateTexturesUI.Instance.DeactivateTextureEditMode();
        InstantiatePrefabUI.Instance.DeactivateAddFurnitureMode();
        
        sceneCamera.transform.position = new Vector3(0, 0, 0);
        sceneCamera.transform.rotation = Quaternion.Euler(0, 0, 0);
        rayInteractorRight.MaxRayLength = 5;
        rayInteractorLeft.MaxRayLength = 5;
        oculusCursorRight.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
        oculusCursorLeft.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
        selectionCircleRight.transform.localScale = new Vector3(0.016f, 0.016f, 1f);
        selectionCircleLeft.transform.localScale = new Vector3(0.016f, 0.016f, 1f);
        //roomReshaper.enabled = false;
    }

    // public void OpenRoomEditModeFunction()
    // {
    //     if (!canRun) return;

    //     InstantiateTexturesUI.Instance.DeactivateTextureEditMode();
    //     InstantiatePrefabUI.Instance.DeactivateAddFurnitureMode();
    //     canRun = false;
    //     // Faz a camera fica na posição de cima 
    //     if (dentro)
    //     {
    //         sceneCamera.transform.position = new Vector3(20, 25, 20);
    //         rayInteractorRight.MaxRayLength = 100;
    //         rayInteractorLeft.MaxRayLength = 100;
    //         oculusCursorRight.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
    //         oculusCursorLeft.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
    //         selectionCircleRight.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
    //         selectionCircleLeft.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
    //         //roomReshaper.enabled = true;
    //         dentro = false;
    //     }
    //     else
    //     {
    //         sceneCamera.transform.position = new Vector3(0, 0, 0);
    //         sceneCamera.transform.rotation = Quaternion.Euler(0, 0, 0);
    //         rayInteractorRight.MaxRayLength = 5;
    //         rayInteractorLeft.MaxRayLength = 5;
    //         oculusCursorRight.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
    //         oculusCursorLeft.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
    //         selectionCircleRight.transform.localScale = new Vector3(0.016f, 0.016f, 1f);
    //         selectionCircleLeft.transform.localScale = new Vector3(0.016f, 0.016f, 1f);
    //         //roomReshaper.enabled = false;
    //         dentro = true;
    //     }

    //     StartCoroutine(ResetCanRun());
    // }

    private IEnumerator ResetCanRun()
    {
        yield return new WaitForSeconds(1);
        canRun = true;
    }
}
