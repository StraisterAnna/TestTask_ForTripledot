using UnityEngine;

public class RotateUI : MonoBehaviour
{
    public float speed = 10f;

    void Update()
    {
        transform.Rotate(0f, 0f, speed * Time.deltaTime);
    }
}
