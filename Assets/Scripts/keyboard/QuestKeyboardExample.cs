using UnityEngine;
using UnityEngine.UI;
#if META_KEYBOARD_OVERLAY
using Meta.XR.Keyboard;
#endif

public class QuestKeyboardExample : MonoBehaviour
{
    public InputField myInputField;

#if META_KEYBOARD_OVERLAY
    private void Start()
    {
        myInputField.onSelect.AddListener(OnInputFieldSelected);
    }

    private void OnInputFieldSelected(string _)
    {
        // Abre o teclado overlay
        KeyboardOverlay.Launch(
            myInputField.text,
            KeyboardOverlay.InputMode.Default,
            KeyboardOverlay.ReturnKeyType.Done,
            OnKeyboardTextCommitted
        );
    }

    private void OnKeyboardTextCommitted(string text)
    {
        myInputField.text = text;
    }
#endif
}