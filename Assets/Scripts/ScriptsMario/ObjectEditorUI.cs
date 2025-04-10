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

    void Awake()
    {
        Instance = this;
        canvas.SetActive(false);

        // Certifique-se de definir um valor mínimo pro slider!
        scaleSlider.minValue = 0.1f;
        scaleSlider.maxValue = 2f;
        scaleSlider.onValueChanged.AddListener(OnScaleChanged);
    }

    public void OpenEditor(GameObject target)
    {
        Debug.Log("OpenEditor chamado por: " + target.name);

        currentTarget = target;
        originalScale = target.transform.localScale;

        ignoreSliderChange = true;

        // Calcula o valor atual do slider com base na escala atual
        float currentMultiplier = target.transform.localScale.x / originalScale.x;

        scaleSlider.value = currentMultiplier;

        canvas.SetActive(true);
        ignoreSliderChange = false;
    }


    public void OnScaleChanged(float value)
    {
        if (ignoreSliderChange || currentTarget == null) return;

        float safeValue = Mathf.Max(value, 0.1f); // Evita escala zero
        currentTarget.transform.localScale = originalScale * safeValue;

        Debug.Log($"Escala alterada para: {originalScale * safeValue}");
    }

    public void CloseEditor()
    {
        canvas.SetActive(false);
        currentTarget = null;
    }
}
