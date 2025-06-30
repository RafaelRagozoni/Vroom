using UnityEngine;

public class CloseUI : MonoBehaviour
{
    public GameObject Canvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CloseUIFunction()
    {
        Canvas.SetActive(false);
    }

}
