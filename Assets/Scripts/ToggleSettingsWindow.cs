using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle), typeof(Animator))]
public class ToggleAnimatorBinder : MonoBehaviour
{
    public string paramName = "On";
    Toggle t; Animator a;

    void Awake() { t = GetComponent<Toggle>(); a = GetComponent<Animator>();
                   a.SetBool(paramName, t.isOn);
                   t.onValueChanged.AddListener(v => a.SetBool(paramName, v)); }
    void OnEnable() => a?.SetBool(paramName, GetComponent<Toggle>().isOn);
}
