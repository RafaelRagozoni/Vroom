using UnityEngine;
using UnityEngine.UI;

public class ObjectEditorUI : MonoBehaviour
{
    public static ObjectEditorUI Instance;

    public GameObject canvas;
    public Slider scaleSlider;

    private GameObject currentTarget;
    private Vector3 originalScale;
    private bool ignoreSliderChange = false;
    public Transform cameraRig;

    void Awake()
    {
        Instance = this;
        canvas.SetActive(false);

        // Certifique-se de definir um valor mínimo pro slider!
        scaleSlider.minValue = 0.1f;
        scaleSlider.maxValue = 2f;
        scaleSlider.onValueChanged.AddListener(OnScaleChanged);
    }

    public void OpenEditor(GameObject target, Vector3 position)
    {
        Debug.Log("OpenEditor chamado por: " + target.name);

        currentTarget = target;
        originalScale = Vector3.one;

        ignoreSliderChange = true;

        float currentScale = target.transform.localScale.x;
        float sliderValue = Mathf.InverseLerp(0.5f, 2f, currentScale);
        scaleSlider.value = Mathf.Lerp(scaleSlider.minValue, scaleSlider.maxValue, sliderValue);

        // Define a posição do canvas
        canvas.transform.position = position;

        // Faz o canvas olhar para a câmera
        canvas.transform.LookAt(cameraRig);

        // Corrige a rotação para não ficar de cabeça pra baixo
        canvas.transform.rotation = Quaternion.LookRotation(canvas.transform.position - cameraRig.position);

        canvas.SetActive(true);
        ignoreSliderChange = false;
    }


    private void AtualizarPosicaoUI()
    {
        if (currentTarget == null) return;

        Collider collider = currentTarget.GetComponent<Collider>();
        if (collider != null)
        {
            float altura = collider.bounds.size.y;
            Vector3 novaPosicao = currentTarget.transform.position + new Vector3(0, altura + 0.1f, 0);
            canvas.transform.position = novaPosicao;
        }
    }

    public void OnScaleChanged(float value)
    {
        if (ignoreSliderChange || currentTarget == null) return;

        // Aqui o valor do slider já é o multiplicador direto
        float scale = Mathf.Clamp(value, 0.5f, 2f);
        currentTarget.transform.localScale = Vector3.one * scale;

        AtualizarPosicaoUI(); // <- reposiciona a UI com base na nova escala

        Debug.Log($"Escala alterada para: {currentTarget.transform.localScale}");
    }

    public void CloseEditor()
    {
        canvas.SetActive(false);
        currentTarget = null;
    }
}
