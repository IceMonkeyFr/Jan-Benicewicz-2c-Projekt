using UnityEngine;
using UnityEngine.UI;   // potrzebne do Image

public class PauseMenu : MonoBehaviour
{
    [Header("UI Pause")]
    public GameObject pauseMenuUI;  // Panel z menu pauzy
    public Image pauseImage;        // Obrazek wyœwietlany po zapauzowaniu

    [Header("Ustawienia przezroczystoœci")]
    [Range(0f, 1f)]
    public float pausedImageAlpha = 1f;   // przezroczystoœæ, gdy pauza aktywna

    private float originalAlpha;          // zapamiêtuje oryginaln¹ przezroczystoœæ obrazka

    public static bool GameIsPaused = false;

    void Start()
    {
        // Pobieramy oryginaln¹ przezroczystoœæ obrazka
        if (pauseImage != null)
            originalAlpha = pauseImage.color.a;

        // Ukrywamy UI na starcie
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        // Naciœniêcie ESC w³¹cza/wy³¹cza pauzê
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);  // wy³¹czamy panel
        SetImageAlpha(originalAlpha);  // przywracamy oryginaln¹ przezroczystoœæ

        Time.timeScale = 1f;           // przywracamy czas
        GameIsPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);   // w³¹czamy panel
        SetImageAlpha(pausedImageAlpha); // ustawiamy przezroczystoœæ obrazka

        Time.timeScale = 0f;           // zatrzymujemy grê
        GameIsPaused = true;
    }

    // Funkcja ustawiaj¹ca przezroczystoœæ obrazka
    private void SetImageAlpha(float alpha)
    {
        if (pauseImage != null)
        {
            Color c = pauseImage.color;
            c.a = alpha;
            pauseImage.color = c;
        }
    }

    public void QuitGame()
    {
        Debug.Log("Wyjœcie z gry...");
        Application.Quit();
    }
}
