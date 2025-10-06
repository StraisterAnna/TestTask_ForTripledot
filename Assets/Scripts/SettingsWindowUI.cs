using UnityEngine;
using UnityEngine.UI;

public class SettingsWindowUI : MonoBehaviour
{
    [SerializeField] private Button btnClose;
    private UIController uiController;

    private void Awake()
    {
        if (btnClose) btnClose.onClick.AddListener(Close);
        uiController = FindObjectOfType<UIController>();
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        if (uiController != null)
            uiController.CloseSettings();
        else
            gameObject.SetActive(false);
    }
}
