using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public sealed class BottomBarTab : MonoBehaviour, IPointerClickHandler
{
    private static class Anim
    {
        public const string IsActive = "IsActive";
        public const string IsLocked = "IsLocked";
        public const string LockedPulse = "LockedPulse";
    }

    [Header("Wiring")]
    [SerializeField] private string tabId = "Tab";
    [SerializeField] private Toggle toggle;
    [SerializeField] private Animator animator;
    [SerializeField] private CanvasGroup lockGroup;

    [Header("State")]
    [SerializeField] private bool isLocked;

    [Header("Events")]
    public UnityEvent<BottomBarTab> ToggledOn;
    public UnityEvent<BottomBarTab> ToggledOff;
    public UnityEvent<BottomBarTab> LockedPressed;

    public string TabId => tabId;
    public bool IsLocked => isLocked;
    public Toggle Toggle => toggle;

    private void Reset()
    {
        toggle = GetComponent<Toggle>();
        animator = GetComponent<Animator>();
        lockGroup = GetComponentInChildren<CanvasGroup>();
    }

    private void Awake()
    {
        ApplyLockVisual();
        PushAnimatorState(toggle != null && toggle.isOn, isLocked);
    }

    private void OnEnable()
    {
        if (toggle != null) toggle.onValueChanged.AddListener(OnToggleChanged);
    }

    private void OnDisable()
    {
        if (toggle != null) toggle.onValueChanged.RemoveListener(OnToggleChanged);
    }

    public void SetLocked(bool locked)
    {
        if (isLocked == locked) return;
        isLocked = locked;
        ApplyLockVisual();
        if (toggle != null && isLocked && toggle.isOn) toggle.SetIsOnWithoutNotify(false);
        PushAnimatorState(toggle != null && toggle.isOn, isLocked);
    }

    private void ApplyLockVisual()
    {
        if (lockGroup == null) return;
        lockGroup.alpha = isLocked ? 1f : 0f;
        lockGroup.blocksRaycasts = isLocked;
        lockGroup.interactable = isLocked;
    }

    private void OnToggleChanged(bool on)
    {
        if (isLocked)
        {
            if (on && toggle != null) toggle.SetIsOnWithoutNotify(false);
            return;
        }

        PushAnimatorState(on, isLocked);
        if (on) ToggledOn?.Invoke(this);
        else ToggledOff?.Invoke(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isLocked)
        {
            if (animator != null) animator.SetTrigger(Anim.LockedPulse);
            LockedPressed?.Invoke(this);
            return;
        }

        if (toggle == null) return;

        if (!toggle.isOn) toggle.isOn = true;
        else if (toggle.group != null && toggle.group.allowSwitchOff) toggle.isOn = false;
    }

    private void PushAnimatorState(bool isActive, bool locked)
    {
        if (animator == null) return;
        animator.SetBool(Anim.IsActive, isActive);
        animator.SetBool(Anim.IsLocked, locked);
    }
}
