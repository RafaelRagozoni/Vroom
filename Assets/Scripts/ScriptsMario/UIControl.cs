using UnityEngine;

public class UIControl : MonoBehaviour
{
    public GameObject Capsula;
    public Camera sceneCamera;
    public Transform canvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvas.gameObject.SetActive(false);
        canvas.position=sceneCamera.transform.position + sceneCamera.transform.forward * 5.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void capsulaUIController()
    {
        Renderer rend = Capsula.GetComponent<Renderer>();
        if (rend != null)
        {
            rend.materials[0].color = Color.red;
        }
        else
        {
            //Desativa o objeto Capsula
            Capsula.SetActive(false);
        }
    }
}
