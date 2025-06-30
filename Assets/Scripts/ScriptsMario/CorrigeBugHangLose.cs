using UnityEngine;
using System.Collections;


public class CorrigeBugHangLose : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f); // tempo suficiente para estabilizar os estados
        gameObject.SetActive(true);
    }
}
