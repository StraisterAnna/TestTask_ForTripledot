using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public sealed class SafeArea : MonoBehaviour
{
    [SerializeField] private bool updateEveryFrame = true;

    private RectTransform _rt;
    private Rect _lastSafe;
    private ScreenOrientation _lastOrientation;
    private Vector2Int _lastResolution;

    private void Awake()
    {
        _rt = GetComponent<RectTransform>();
        ApplySafeArea();
    }

    private void Update()
    {
        if (!updateEveryFrame) return;

        if (Screen.safeArea != _lastSafe ||
            Screen.orientation != _lastOrientation ||
            _lastResolution.x != Screen.width ||
            _lastResolution.y != Screen.height)
        {
            ApplySafeArea();
        }
    }

    public void Refresh() => ApplySafeArea();

    private void ApplySafeArea()
    {
        var safe = Screen.safeArea;
        _lastSafe = safe;
        _lastOrientation = Screen.orientation;
        _lastResolution = new Vector2Int(Screen.width, Screen.height);

        Vector2 min = safe.position;
        Vector2 max = safe.position + safe.size;

        float w = Mathf.Max(1f, Screen.width);
        float h = Mathf.Max(1f, Screen.height);

        min.x /= w; min.y /= h;
        max.x /= w; max.y /= h;

        _rt.anchorMin = min;
        _rt.anchorMax = max;
    }
}
