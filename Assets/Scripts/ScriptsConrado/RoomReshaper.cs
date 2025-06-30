using Meta.Voice;
using Oculus.Interaction;
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

    public void MoveGizmos(Vector3 Positions)
    {
        gizmoX.transform.position = new Vector3(Positions.x, gizmoX.transform.position.y, gizmoX.transform.position.z);
        gizmoY.transform.position = new Vector3(gizmoY.transform.position.x, Positions.y, gizmoY.transform.position.z);
        gizmoZ.transform.position = new Vector3(gizmoZ.transform.position.x, gizmoZ.transform.position.y, Positions.z);
    }

    public Vector3 GetGizmoPosition()
    {
        return new Vector3(gizmoX.transform.position.x, gizmoY.transform.position.y, gizmoZ.transform.position.z);
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
        xText.text = (originalGizmoX - 7.6).ToString("F2") + "m";
        yText.text = (originalGizmoY - 10.1).ToString("F2") + "m";
        zText.text = (originalGizmoZ - 7.6).ToString("F2") + "m";

        var oneGrabTranslateTransformerX = gizmoX.transform.GetComponentInChildren<OneGrabTranslateTransformer>();
        if (oneGrabTranslateTransformerX != null)
        {
            oneGrabTranslateTransformerX.Constraints.MinY.Constrain = true;
            oneGrabTranslateTransformerX.Constraints.MinY.Value = gizmoX.transform.position.y;
            oneGrabTranslateTransformerX.Constraints.MinZ.Constrain = true;
            oneGrabTranslateTransformerX.Constraints.MinZ.Value = gizmoX.transform.position.z;
            oneGrabTranslateTransformerX.Constraints.MaxY.Constrain = true;
            oneGrabTranslateTransformerX.Constraints.MaxY.Value = gizmoX.transform.position.y;
            oneGrabTranslateTransformerX.Constraints.MaxZ.Constrain = true;
            oneGrabTranslateTransformerX.Constraints.MaxZ.Value = gizmoX.transform.position.z;
        }

        var oneGrabTranslateTransformerY = gizmoY.transform.GetComponentInChildren<OneGrabTranslateTransformer>();
        if (oneGrabTranslateTransformerY != null)
        {
            oneGrabTranslateTransformerY.Constraints.MinX.Constrain = true;
            oneGrabTranslateTransformerY.Constraints.MinX.Value = gizmoY.transform.position.x;
            oneGrabTranslateTransformerY.Constraints.MinZ.Constrain = true;
            oneGrabTranslateTransformerY.Constraints.MinZ.Value = gizmoY.transform.position.z;
            oneGrabTranslateTransformerY.Constraints.MaxX.Constrain = true;
            oneGrabTranslateTransformerY.Constraints.MaxX.Value = gizmoY.transform.position.x;
            oneGrabTranslateTransformerY.Constraints.MaxZ.Constrain = true;
            oneGrabTranslateTransformerY.Constraints.MaxZ.Value = gizmoY.transform.position.z;
        }

        var oneGrabTranslateTransformerZ = gizmoZ.transform.GetComponentInChildren<OneGrabTranslateTransformer>();
        if (oneGrabTranslateTransformerZ != null)
        {
            oneGrabTranslateTransformerZ.Constraints.MinX.Constrain = true;
            oneGrabTranslateTransformerZ.Constraints.MinX.Value = gizmoZ.transform.position.x;
            oneGrabTranslateTransformerZ.Constraints.MinY.Constrain = true;
            oneGrabTranslateTransformerZ.Constraints.MinY.Value = gizmoZ.transform.position.y;
            oneGrabTranslateTransformerZ.Constraints.MaxX.Constrain = true;
            oneGrabTranslateTransformerZ.Constraints.MaxX.Value = gizmoZ.transform.position.x;
            oneGrabTranslateTransformerZ.Constraints.MaxY.Constrain = true;
            oneGrabTranslateTransformerZ.Constraints.MaxY.Value = gizmoZ.transform.position.y;
        }

    }


}