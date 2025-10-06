using UnityEngine;
using UnityEngine.UI;

public class LevelCompletedWindowUI : MonoBehaviour
{
    [SerializeField] private Button btnClose;
    private UIController uiController;

    private void Awake()
    {
        if (btnClose) btnClose.onClick.AddListener(Close);
        uiController = FindObjectOfType<UIController>();
    }

    private void Close()
    {
        if (uiController != null)
            uiController.CloseLevelCompleted();
        else
            gameObject.SetActive(false);
    }
}
