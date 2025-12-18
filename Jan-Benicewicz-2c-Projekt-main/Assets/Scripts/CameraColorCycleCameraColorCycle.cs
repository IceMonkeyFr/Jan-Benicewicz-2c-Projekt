using UnityEngine;

public class CameraColorCycle : MonoBehaviour
{
    [Header("Kolory t³a kamery")]
    public Color[] colors;           // lista kolorów, przez które ma przechodziæ
    public float transitionSpeed = 1f; // szybkoœæ zmiany kolorów

    private Camera cam;
    private int currentIndex = 0;
    private int nextIndex = 1;
    private float t = 0f;

    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogError("CameraColorCycle musi byæ przypiêty do obiektu z Camera!");
        }

        if (colors.Length < 2)
        {
            Debug.LogWarning("Dodaj przynajmniej 2 kolory do CameraColorCycle!");
        }
    }

    void Update()
    {
        if (colors.Length < 2 || cam == null) return;

        t += Time.deltaTime * transitionSpeed;
        cam.backgroundColor = Color.Lerp(colors[currentIndex], colors[nextIndex], t);

        if (t >= 1f)
        {
            t = 0f;
            currentIndex = nextIndex;
            nextIndex = (nextIndex + 1) % colors.Length;
        }
    }
}
