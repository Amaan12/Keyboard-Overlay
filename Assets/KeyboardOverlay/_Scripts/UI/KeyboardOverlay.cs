using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Image))]
public class KeyboardOverlayKey : MonoBehaviour
{
    [Header("Key Settings")]
    [SerializeField] KeyCode key;

    [Header("Color Settings")]
    [SerializeField] Color pressedColor = new Color(0f, 1f, 0f, 0.5f);
    [SerializeField] Color idleColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    [SerializeField] float colorLerpSpeed = 10f;

    Image image;
    TextMeshProUGUI keyLabel;
    Color targetColor;

    [DllImport("user32.dll")]
    private static extern short GetAsyncKeyState(int vKey);

    void Awake()
    {
        image = GetComponent<Image>();
        keyLabel = GetComponentInChildren<TextMeshProUGUI>();
        if (keyLabel != null)
            keyLabel.text = key.ToString().ToUpper();

        image.color = idleColor;
        targetColor = idleColor;
    }

    void Update()
    {
        int vKey = (int)key;

        // Uppercase fix for letters only
        if (vKey >= (int)KeyCode.A && vKey <= (int)KeyCode.Z)
            vKey -= 32;

        bool isPressed = (GetAsyncKeyState(vKey) & 0x8000) != 0;

        targetColor = isPressed ? pressedColor : idleColor;
        image.color = Color.Lerp(image.color, targetColor, Time.deltaTime * colorLerpSpeed);
    }
}
