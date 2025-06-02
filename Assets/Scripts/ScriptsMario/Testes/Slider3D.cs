using UnityEngine;

public class Slider3D : MonoBehaviour
{
    public Transform pontoInicial;
    public Transform pontoFinal;

    public float minValue;
    public float maxValue;

    public Transform objetoParaEscalar;
    private bool isMirror;
    void Start()
    {
        minValue=0.75f;
        maxValue=1.5f;
        objetoParaEscalar =EditFurnitureManager.Instance.selectedTarget;
        if (objetoParaEscalar != null)
        {
            // Pegue a escala atual (uniforme, supondo que x=y=z)
            float escalaAtual = objetoParaEscalar.localScale.x; // ou y ou z, se só um eixo importa
            if (escalaAtual < 0)
            {
                isMirror = true;
                escalaAtual = Mathf.Abs(escalaAtual); // Converte para positivo se for negativo
            }
            else
            {
                isMirror = false;
            }
            // Calcula t correspondente à escala
            float t = Mathf.InverseLerp(minValue, maxValue, escalaAtual);
            t = Mathf.Clamp01(t);

            // Define posição do slider
            transform.position = Vector3.Lerp(pontoInicial.position, pontoFinal.position, t);
        }
    }

    void Update()
    {
        // Calcula o valor t baseado na posição do slider
        Vector3 eixo = pontoFinal.position - pontoInicial.position;
        Vector3 posRelativa = transform.position - pontoInicial.position;
        float t = Vector3.Dot(posRelativa, eixo.normalized) / eixo.magnitude;
        t = Mathf.Clamp01(t);

        // Mapeia t para o intervalo desejado
        float valorMapeado = Mathf.Lerp(minValue, maxValue, t);

        if (objetoParaEscalar != null)
        {
            if (isMirror)
            {
                Vector3 escala = new Vector3(-1, 1, 1);
                objetoParaEscalar.localScale = escala * valorMapeado; // Inverte o valor se for um objeto espelhado
            }
            else
            {
                objetoParaEscalar.localScale = Vector3.one * valorMapeado;
            }
            // --- AJUSTE DE ALTURA ---
            Renderer rend = objetoParaEscalar.GetComponentInChildren<Renderer>();
            if (rend != null)
            {
                float novaAltura = rend.bounds.size.y;
                Vector3 pos = objetoParaEscalar.position;
                pos.y = novaAltura / 2f;
                objetoParaEscalar.position = pos;
            }
        }
    }
}