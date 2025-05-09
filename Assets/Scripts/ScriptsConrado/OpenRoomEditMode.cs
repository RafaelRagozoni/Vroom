using UnityEngine;
using Oculus.Interaction;
using System.Collections;


public class OpenRoomEditMode : MonoBehaviour
{
    public GameObject sceneCamera;
    public bool dentro = true;

    private bool canRun = true;

    public void OpenRoomEditModeFunction()
    {
        if (!canRun) return;

        canRun = false;
        // Faz a camera fica na posição de cima 
        if (dentro)
        {
            sceneCamera.transform.position = new Vector3(25, 35, -25);
            sceneCamera.transform.rotation = Quaternion.Euler(45, -45, 0);
            dentro = false;
        }
        else
        {
            sceneCamera.transform.position = new Vector3(0, 0, 0);
            sceneCamera.transform.rotation = Quaternion.Euler(0, 0, 0);
            dentro = true;
        }

        StartCoroutine(ResetCanRun());
    }

    private IEnumerator ResetCanRun()
    {
        yield return new WaitForSeconds(5);
        canRun = true;
    }
}
