using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public sealed class UIRampGradient : MonoBehaviour
{
    public Gradient Gradient = new Gradient();
    [Range(16, 2048)] public int Resolution = 256;
    public float Angle = 90f;

    private Image _img;
    private Texture2D _ramp;
    private Material _runtimeMat;

    private static readonly int MainTexId = Shader.PropertyToID("_MainTex");
    private static readonly int RampTexId = Shader.PropertyToID("_RampTex");
    private static readonly int AngleId   = Shader.PropertyToID("_Angle");

    private void Awake()
    {
        _img = GetComponent<Image>();
        BakeAndApply();
    }

    private void OnEnable()
    {
        BakeAndApply();
    }

    private void OnValidate()
    {
        if (!isActiveAndEnabled) return;
        BakeAndApply();
    }

    private void OnDestroy()
    {
        if (Application.isPlaying)
        {
            if (_runtimeMat) Destroy(_runtimeMat);
            if (_ramp) Destroy(_ramp);
        }
        else
        {
            if (_runtimeMat) DestroyImmediate(_runtimeMat);
            if (_ramp) DestroyImmediate(_ramp);
        }
    }

    private void BakeAndApply()
    {
        if (_img == null) _img = GetComponent<Image>();

        int width = Mathf.Clamp(Resolution, 16, 2048);

        if (_ramp == null || _ramp.width != width)
        {
            if (_ramp != null)
            {
                if (Application.isPlaying) Destroy(_ramp);
                else DestroyImmediate(_ramp);
            }

            _ramp = new Texture2D(width, 1, TextureFormat.RGBA32, false, false)
            {
                wrapMode = TextureWrapMode.Clamp,
                filterMode = FilterMode.Bilinear,
                name = "UIRampGradient_Ramp"
            };
        }

        for (int x = 0; x < width; x++)
        {
            float t = width == 1 ? 0f : x / (float)(width - 1);
            _ramp.SetPixel(x, 0, Gradient.Evaluate(t));
        }
        _ramp.Apply(false, false);

        if (_runtimeMat == null)
        {
            var shader = Shader.Find("UI/Gradient Ramp");
            _runtimeMat = new Material(shader) { name = "UIRampGradient_Mat" };
        }

        Texture main = _img != null && _img.sprite != null ? _img.sprite.texture : Texture2D.whiteTexture;

        _runtimeMat.SetTexture(MainTexId, main);
        _runtimeMat.SetTexture(RampTexId, _ramp);
        _runtimeMat.SetFloat(AngleId, Angle);

        if (_img != null)
        {
            _img.material = _runtimeMat;
            _img.SetMaterialDirty();
        }
    }
}
