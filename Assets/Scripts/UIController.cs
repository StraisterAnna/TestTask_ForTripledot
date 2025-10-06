using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button btnSettings;
    [SerializeField] private Button btnWin;

    [Header("Windows")]
    [SerializeField] private GameObject settingsWindow;
    [SerializeField] private GameObject levelCompletedWindow;

    [Header("UI Components")]
    [SerializeField] private BottomBarController bottomBar;

    private void Awake()
    {
        if (btnSettings != null) btnSettings.onClick.AddListener(OpenSettings);
        if (btnWin != null) btnWin.onClick.AddListener(OpenLevelCompleted);
        if (settingsWindow != null) settingsWindow.SetActive(false);
        if (levelCompletedWindow != null) levelCompletedWindow.SetActive(false);
        if (bottomBar == null) bottomBar = FindObjectOfType<BottomBarController>();
    }

    private void OpenSettings()
    {
        if (levelCompletedWindow) levelCompletedWindow.SetActive(false);
        if (settingsWindow) settingsWindow.SetActive(true);
        if (bottomBar) bottomBar.Hide();
    }

    private void OpenLevelCompleted()
    {
        if (settingsWindow) settingsWindow.SetActive(false);
        if (levelCompletedWindow) levelCompletedWindow.SetActive(true);
        if (bottomBar) bottomBar.Hide();
    }

    public void CloseSettings()
    {
        if (settingsWindow) settingsWindow.SetActive(false);
        if (bottomBar) bottomBar.Show();
    }

    public void CloseLevelCompleted()
    {
        if (levelCompletedWindow) levelCompletedWindow.SetActive(false);
        if (bottomBar) bottomBar.Show();
    }
}
