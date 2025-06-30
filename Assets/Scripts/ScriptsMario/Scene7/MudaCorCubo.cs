using UnityEngine;

public class MudaCorCubo : MonoBehaviour
{
    public GameObject cubo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public void ChangeColor()
    //{
    //    cubo.GetComponent<MeshRenderer>().materials[0].color = Color.red;
    //}

    public void ChangeColor()
    {
        Color randomColor = new Color(
            Random.Range(0f, 1f), // Red
            Random.Range(0f, 1f), // Green
            Random.Range(0f, 1f)  // Blue
        );

        cubo.GetComponent<MeshRenderer>().materials[0].color = randomColor;
    }



}
