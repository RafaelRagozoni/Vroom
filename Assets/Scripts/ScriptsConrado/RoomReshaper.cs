using Meta.Voice;
using TMPro;
using UnityEngine;

public class RoomReshaper : MonoBehaviour
{
    public GameObject wallR;
    public GameObject wallL;
    public GameObject wallB;
    public GameObject wallF;
    public GameObject celling;
    public GameObject floor;
    public GameObject gizmoX;
    public GameObject gizmoY;
    public GameObject gizmoZ;
    public TextMeshPro xText;
    public TextMeshPro yText;
    public TextMeshPro zText;
    private float originalGizmoX;
    private float originalGizmoY;
    private float originalGizmoZ;

    public void Start()
    {
        originalGizmoX = gizmoX.transform.position.x;
        originalGizmoY = gizmoY.transform.position.y;
        originalGizmoZ = gizmoZ.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Remodelate();
    }

    void Remodelate()
    {
        float deltaGizmoY = gizmoY.transform.position.y - originalGizmoY;
        float deltaGizmoX = gizmoX.transform.position.x - originalGizmoX;
        float deltaGizmoZ = gizmoZ.transform.position.z - originalGizmoZ;

        // puxando o Y
        wallB.transform.position = new Vector3(
            wallB.transform.position.x + deltaGizmoX / 2,
            wallB.transform.position.y + deltaGizmoY / 2,
            wallB.transform.position.z
        );
        wallF.transform.position = new Vector3(
            wallF.transform.position.x + deltaGizmoX / 2,
            wallF.transform.position.y + deltaGizmoY / 2,
            wallF.transform.position.z + deltaGizmoZ
        );
        wallL.transform.position = new Vector3(
            wallL.transform.position.x,
            wallL.transform.position.y + deltaGizmoY / 2,
            wallL.transform.position.z + deltaGizmoZ / 2
        );
        wallR.transform.position = new Vector3(
            wallR.transform.position.x + deltaGizmoX,
            wallR.transform.position.y + deltaGizmoY / 2,
            wallR.transform.position.z + deltaGizmoZ / 2
        );
        celling.transform.position = new Vector3(
            celling.transform.position.x + deltaGizmoX / 2,
            celling.transform.position.y + deltaGizmoY,
            celling.transform.position.z + deltaGizmoZ / 2
        );
        floor.transform.position = new Vector3(
            floor.transform.position.x + deltaGizmoX / 2,
            floor.transform.position.y,
            floor.transform.position.z + deltaGizmoZ / 2
        );
        gizmoX.transform.position = new Vector3(
            gizmoX.transform.position.x,
            gizmoX.transform.position.y + deltaGizmoY / 2,
            gizmoX.transform.position.z + deltaGizmoZ / 2
        );
        gizmoZ.transform.position = new Vector3(
            gizmoZ.transform.position.x + deltaGizmoX / 2,
            gizmoZ.transform.position.y + deltaGizmoY / 2,
            gizmoZ.transform.position.z
        );
        gizmoY.transform.position = new Vector3(
            gizmoY.transform.position.x + deltaGizmoX / 2,
            gizmoY.transform.position.y,
            gizmoY.transform.position.z + deltaGizmoZ / 2
        );

        wallB.transform.localScale = new Vector3(
            wallB.transform.localScale.x + deltaGizmoX / 10,
            wallB.transform.localScale.y + deltaGizmoY / 10,
            wallB.transform.localScale.z + deltaGizmoY / 10
        );
        wallF.transform.localScale = new Vector3(
            wallF.transform.localScale.x + deltaGizmoX / 10,
            wallF.transform.localScale.y + deltaGizmoY / 10,
            wallF.transform.localScale.z + deltaGizmoY / 10
        );
        wallL.transform.localScale = new Vector3(
            wallL.transform.localScale.x + deltaGizmoZ / 10,
            wallL.transform.localScale.y + deltaGizmoY / 10,
            wallL.transform.localScale.z + deltaGizmoY / 10
        );
        wallR.transform.localScale = new Vector3(
            wallR.transform.localScale.x + deltaGizmoZ / 10,
            wallR.transform.localScale.y + deltaGizmoY / 10,
            wallR.transform.localScale.z + deltaGizmoY / 10
        );
        celling.transform.localScale = new Vector3(
            celling.transform.localScale.x + deltaGizmoX / 10,
            celling.transform.localScale.y + deltaGizmoZ / 10,
            celling.transform.localScale.z + deltaGizmoZ / 10
        );
        floor.transform.localScale = new Vector3(
            floor.transform.localScale.x + deltaGizmoX / 10,
            floor.transform.localScale.y + deltaGizmoZ / 10,
            floor.transform.localScale.z + deltaGizmoZ / 10
        );
        originalGizmoX = gizmoX.transform.position.x;
        originalGizmoY = gizmoY.transform.position.y;
        originalGizmoZ = gizmoZ.transform.position.z;
        xText.text = (originalGizmoX - 11).ToString("F2");
        yText.text = (originalGizmoY - 15).ToString("F2");
        zText.text = (originalGizmoZ - 11).ToString("F2");
    }


}
