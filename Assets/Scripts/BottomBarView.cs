using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public sealed class BottomBarView : MonoBehaviour
{
    [System.Serializable]
    public sealed class ContentActivatedEvent : UnityEvent<int, string> {}

    [Header("Tabs")]
    [SerializeField] private List<BottomBarTab> tabs = new List<BottomBarTab>();
    [SerializeField] private int initialActiveIndex = 0;
    [SerializeField] private bool allowEmptySelection = false;
    [SerializeField] private ToggleGroup toggleGroup;

    [Header("Events")]
    public ContentActivatedEvent ContentActivated;
    public UnityEvent Closed;

    private Animator _animator;

    private void Reset()
    {
        toggleGroup = GetComponent<ToggleGroup>();
        tabs.Clear();
        GetComponentsInChildren(tabs);
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        if (toggleGroup != null) toggleGroup.allowSwitchOff = allowEmptySelection;

        for (int i = 0; i < tabs.Count; i++)
        {
            var t = tabs[i];
            if (t == null) continue;
            if (t.Toggle != null) t.Toggle.group = toggleGroup;

            int idx = i;
            t.ToggledOn.AddListener(_ => OnTabOn(idx));
            t.ToggledOff.AddListener(_ => OnTabOff(idx));
        }
    }

    private void Start()
    {
        SelectInitial();
    }

    private void SelectInitial()
    {
        if (tabs.Count == 0) return;
        int idx = Mathf.Clamp(initialActiveIndex, 0, tabs.Count - 1);

        for (int i = 0; i < tabs.Count; i++)
        {
            var t = tabs[i];
            if (t == null || t.Toggle == null) continue;
            bool shouldBeOn = (i == idx) && !t.IsLocked;
            t.Toggle.SetIsOnWithoutNotify(shouldBeOn);
        }

        EvaluateClosed();
    }

    private void OnTabOn(int index)
    {
        if (index < 0 || index >= tabs.Count) return;
        var t = tabs[index];
        ContentActivated?.Invoke(index, t != null ? t.TabId : string.Empty);
    }

    private void OnTabOff(int _)
    {
        EvaluateClosed();
    }

    private void EvaluateClosed()
    {
        bool anyOn = false;
        for (int i = 0; i < tabs.Count; i++)
        {
            var t = tabs[i];
            if (t != null && t.Toggle != null && t.Toggle.isOn) { anyOn = true; break; }
        }
        if (!anyOn) Closed?.Invoke();
    }

    public void SetLocked(string tabId, bool locked)
    {
        for (int i = 0; i < tabs.Count; i++)
        {
            var t = tabs[i];
            if (t != null && t.TabId == tabId)
            {
                t.SetLocked(locked);
                break;
            }
        }
        EvaluateClosed();
    }

    public void ShowBar(bool visible)
    {
        if (_animator != null) _animator.SetBool("IsVisible", visible);
    }
}
