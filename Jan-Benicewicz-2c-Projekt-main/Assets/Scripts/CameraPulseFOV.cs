using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPulseFOV : MonoBehaviour
{
    [Header("Ustawienia pulsowania")]
    public float minSize = 4.5f;      // minimalny rozmiar kamery (Orthographic Size)
    public float maxSize = 5.5f;      // maksymalny rozmiar kamery
    public float speed = 1f;          // szybkość pulsowania

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogError("CameraPulseFOV musi być przypięty do obiektu z Camera!");
            enabled = false;
        }

        if (!cam.orthographic)
        {
            Debug.LogWarning("Kamery nieortograficzne używają FOV zamiast orthographicSize.");
        }
    }

    void Update()
    {
        float pulse = (Mathf.Sin(Time.time * speed) + 1f) / 2f; // 0 → 1
        cam.orthographicSize = Mathf.Lerp(minSize, maxSize, pulse);
    }
}
