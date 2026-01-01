using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Util;

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

    Dictionary<KeyCode, int> keyCodeToVK;

    [SerializeField] bool isTransparentyOverlay;

    [DllImport("user32.dll")]
    static extern short GetAsyncKeyState(int vKey);

    void Awake()
    {
        GetReferences();
        SetMapping();

        SetColor();
        SetText();
    }

    void Update()
    {
        bool isPressed = isTransparentyOverlay
            ? IsPressedWin(key)
            : Input.GetKey(key);

        targetColor = isPressed ? pressedColor : idleColor;
        image.color = Color.Lerp(image.color, targetColor, Time.deltaTime * colorLerpSpeed);
    }
    
    bool IsPressedWin(KeyCode key)
    {
        return keyCodeToVK.TryGetValue(key, out int vk) && (GetAsyncKeyState(vk) & 0x8000) != 0;
    }

    void SetColor()
    {
        image.color = idleColor;
        targetColor = idleColor;
    }

    void SetText()
    {
        if (keyLabel != null)
            keyLabel.text = key.ToString().ToUpper();
    }

    void GetReferences()
    {
        image = GetComponent<Image>();
        keyLabel = GetComponentInChildren<TextMeshProUGUI>();
    }

    void SetMapping()
    {
        Dictionary<int, KeyCode> vkToKeyCode = new Dictionary<int, KeyCode>();
        KeyCodes.SetMappings(vkToKeyCode);
        keyCodeToVK = DictionaryUtils.Invert(vkToKeyCode);
    }
}
