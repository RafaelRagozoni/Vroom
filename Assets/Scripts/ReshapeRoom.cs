using System.Collections;
using UnityEngine;

public class ReshapeRoom : MonoBehaviour
{
    public Transform paredeEsq;
    public Transform paredeDir;
    public Transform paredeFront;
    public Transform paredeBack;
    public Transform teto;
    public Transform chao;
    public Transform pointLight;
    public Vector3 roomSizeNow;
    public Vector3 roomSizeTarget;
    public Vector3 roomSizeBase;
    public TMPro.TMP_Text xText;
    public TMPro.TMP_Text yText;
    public TMPro.TMP_Text zText;


    void change(Vector3 roomSizeNow){
        float room_x = roomSizeNow.x;
        float room_y = roomSizeNow.y;
        float room_z = roomSizeNow.z;
        float wall_h = 0.5f*room_y;
        float wall_wz = -0.5f*room_z;
        float wall_wx = -0.5f*room_x;

        Vector3 chaoScale = new Vector3(room_x, 0.01f, room_z);
        Vector3 esqScale = new Vector3(0.01f, room_y, room_z);
        Vector3 frontScale = new Vector3(room_x, room_y, 0.01f);

        Vector3 tetoPos = new Vector3(0f, room_y, 0f);
        Vector3 backPos = new Vector3(0f, wall_h, wall_wz);
        Vector3 frontPos = new Vector3(0f, wall_h, wall_wz+room_z);
        Vector3 esqPos = new Vector3(wall_wx, wall_h, 0f);
        Vector3 dirPos = new Vector3(wall_wx+room_x, wall_h, 0f);

        chao.localScale = chaoScale;
        teto.localScale = chaoScale;
        paredeEsq.localScale = esqScale;
        paredeDir.localScale = esqScale;
        paredeFront.localScale = frontScale;
        paredeBack.localScale = frontScale;

        teto.position = tetoPos;
        pointLight.position = tetoPos;
        paredeEsq.position = esqPos;
        paredeDir.position = dirPos;
        paredeFront.position = frontPos;
        paredeBack.position = backPos;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        change(roomSizeBase);
        xText.text = $"X: {roomSizeBase.x}";
        yText.text = $"Y: {roomSizeBase.y}";
        zText.text = $"Z: {roomSizeBase.z}";
        roomSizeNow = roomSizeBase;
    }

    public void RemodelateRoom()
    {
        float duracao = 10f;
        StartCoroutine(InterpolateVector(roomSizeNow, roomSizeTarget, duracao));
        roomSizeNow = roomSizeTarget;
    }

    public IEnumerator InterpolateVector(Vector3 inicio, Vector3 fim, float duracao)
    {
        float tempo = 0f;
        while (tempo < duracao)
        {
            tempo += Time.deltaTime;
            Vector3 valorAtual = Vector3.Lerp(inicio, fim, tempo / duracao);
            change(valorAtual);
            yield return null;
        }
    }

    public void OnBase()
    {
        roomSizeTarget = roomSizeBase;
        RemodelateRoom();
        xText.text = $"X: {roomSizeTarget.x}";
        yText.text = $"Y: {roomSizeTarget.y}";
        zText.text = $"Z: {roomSizeTarget.z}";
    }
}
