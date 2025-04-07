using UnityEngine;
using UnityEngine.UI;

public class EscalaObjeto : MonoBehaviour
{
    public Transform objetoAlvo; // O objeto que ser� escalado
    public Slider sliderEscala;

    void Start()
    {
        if (objetoAlvo != null)
        {
            sliderEscala.value = objetoAlvo.localScale.x; // Assume que X, Y e Z come�am iguais
        }

        // Adiciona um Listener para chamar a fun��o quando o Slider mudar
        sliderEscala.onValueChanged.AddListener(AtualizarEscala);
    }

    public void AtualizarEscala(float valor)
    {
        objetoAlvo.localScale = new Vector3(valor, valor, valor);
    }
}
