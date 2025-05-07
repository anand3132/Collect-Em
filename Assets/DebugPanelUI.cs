using UnityEngine;
using UnityEngine.UI; 

public class DebugPanelUI : MonoBehaviour
{
    public GameObject DebugPanel;
    public Button ToggleButton; 
    void Start()
    {
        // Null check for safety
        if (DebugPanel != null)
        {
            DebugPanel.SetActive(false);
        }

        if (ToggleButton != null)
        {
            // For standard Unity UI Button, use onClick not clicked
            ToggleButton.onClick.AddListener(TriggerDebugPanel);
        }
        else
        {
            Debug.LogError("ToggleButton reference not set in DebugPanelUI");
        }
    }

    private void TriggerDebugPanel()
    {
        if (DebugPanel != null)
        {
            DebugPanel.SetActive(!DebugPanel.activeSelf);
        }
    }

    private void OnDestroy()
    {
        if (ToggleButton != null)
        {
            ToggleButton.onClick.RemoveListener(TriggerDebugPanel);
        }
    }
}