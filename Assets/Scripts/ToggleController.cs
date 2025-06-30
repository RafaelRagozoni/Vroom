using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour
{
    public ReshapeRoom dataHandler; // Referência ao objeto com os dados
    public enum Operation { Add, Subtract, Multiply, Divide }
    public Operation operation;
    public enum Variable { X, Y, Z }
    public TMPro.TMP_Text variableText;
    public Variable targetVariable;
    public float value;

    private void Start()
    {
        // Configura o clique do Toggle (mesmo que não mude o estado)
        EventTrigger trigger = GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = gameObject.AddComponent<EventTrigger>();
        }
    }

    public void OnToggleClicked()
    {
        if (dataHandler == null)
        {
            Debug.LogError("DataHandler não atribuído!");
            return;
        }
        if (!GetComponent<Toggle>().isOn)
        {
            Debug.LogError("ToogleDesativado");
            return;
        }

        // Aplica a operação conforme configurado no Inspector
        switch (targetVariable)
        {
            case Variable.X:
                ApplyOperation(ref dataHandler.roomSizeTarget.x);
                UpdateTexts("X",dataHandler.roomSizeTarget.x);
                break;
            case Variable.Y:
                ApplyOperation(ref dataHandler.roomSizeTarget.y);
                UpdateTexts("Y",dataHandler.roomSizeTarget.y);
                break;
            case Variable.Z:
                ApplyOperation(ref dataHandler.roomSizeTarget.z);
                UpdateTexts("Z",dataHandler.roomSizeTarget.z);
                break;
        }
    }

    private void ApplyOperation(ref float variable)
    {
        switch (operation)
        {
            case Operation.Add:
                variable += value;
                break;
            case Operation.Subtract:
                variable -= value;
                break;
            case Operation.Multiply:
                variable *= value;
                break;
            case Operation.Divide:
                if (value != 0) variable /= value;
                else Debug.LogError("Divisão por zero!");
                break;
        }

        Debug.Log($"{targetVariable} atualizado: {variable}");
    }

    private void UpdateTexts(string dimension, float value)
    {
        if (variableText != null) variableText.text = $"{dimension}: {value}";
    }
    
}