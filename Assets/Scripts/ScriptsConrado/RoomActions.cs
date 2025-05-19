using UnityEngine;
using UnityEngine.UI;

public class RoomActions : MonoBehaviour
{
    public ReshapeRoom dataHandler; // Referência ao objeto com os dados

    public void OnTransform()
    {
        if (!GetComponent<Toggle>().isOn)
        {
            return;
        }
        dataHandler.RemodelateRoom();
    }

    public void OnBase()
    {
        if (!GetComponent<Toggle>().isOn)
        {
            return;
        }
        dataHandler.roomSizeTarget = dataHandler.roomSizeBase;
    }

    public void OnRandom(){
        if (!GetComponent<Toggle>().isOn)
        {
            return;
        }
        dataHandler.roomSizeTarget = dataHandler.roomSizeBase;
    }
}
