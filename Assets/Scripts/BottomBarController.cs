using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class BottomBarController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private string isShownParam = "IsShown";
    [SerializeField] private float showDelay = 0f;
    [SerializeField] private float hideDelay = 0f;
    [SerializeField] private bool useUnscaledTime = true;

    private Coroutine routine;
    private bool requestedVisible = true;

    void Reset()
    {
        animator = GetComponent<Animator>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void OnEnable()
    {
        ApplyImmediate(requestedVisible);
    }

    public void Show() => Request(true, showDelay);
    public void Hide() => Request(false, hideDelay);

    private void Request(bool visible, float delay)
    {
        if (requestedVisible == visible && (routine != null || GetApproxVisible() == visible))
            return;

        requestedVisible = visible;

        if (!isActiveAndEnabled)
            return;

        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(Animate(visible, delay));
    }

    private bool GetApproxVisible()
    {
        if (animator) return animator.GetBool(isShownParam);
        if (canvasGroup) return canvasGroup.alpha > 0.5f;
        return true;
    }

    private IEnumerator Animate(bool visible, float delay)
    {
        if (delay > 0f)
        {
            float t = 0f;
            while (t < delay)
            {
                t += useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                yield return null;
            }
        }

        if (!visible && canvasGroup)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        if (animator)
        {
            if (canvasGroup && visible)
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
            animator.SetBool(isShownParam, visible);
        }
        else if (canvasGroup)
        {
            canvasGroup.alpha = visible ? 1f : 0f;
            canvasGroup.interactable = visible;
            canvasGroup.blocksRaycasts = visible;
        }

        routine = null;
    }

    private void ApplyImmediate(bool visible)
    {
        if (routine != null) { StopCoroutine(routine); routine = null; }

        if (animator) animator.SetBool(isShownParam, visible);

        if (canvasGroup)
        {
            canvasGroup.alpha = visible ? 1f : 0f;
            canvasGroup.blocksRaycasts = visible;
            canvasGroup.interactable = visible;
        }
    }
}
