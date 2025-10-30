using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class OverlayEditModeToggle : MonoBehaviour
{
    [DllImport("user32.dll")]
    private static extern short GetAsyncKeyState(int vKey);

    [Header("Key Combination")]
    public KeyCode modifierKey = KeyCode.LeftControl;
    public KeyCode altModifierKey = KeyCode.LeftAlt;
    public KeyCode toggleKey = KeyCode.BackQuote; // the ` key

    public static bool editMode = false;
    private bool prevTogglePressed = false;

    void Start()
    {
        // optional: start in normal mode
        SetEditMode(false);
    }

    void Update() {
        // Press: Ctrl + Alt + `
        if (Input.GetKey(modifierKey) && Input.GetKey(altModifierKey) && Input.GetKeyDown(toggleKey))
            {
                ToggleEditMode();
            }
        }

    private void ToggleEditMode()
    {
        editMode = !editMode;
        SetEditMode(editMode);
    }

    private void SetEditMode(bool enabled)
    {
        // here you can toggle UI or call your EditModeManager
        Debug.Log("Edit Mode: " + (enabled ? "ON" : "OFF"));
        // Example:
        // EditModeManager.Instance.SetActive(enabled);
    }
}
