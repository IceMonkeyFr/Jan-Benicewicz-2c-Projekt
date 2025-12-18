using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    [Header("Ustawienia czasu")]
    public float startTime = 60f;

    [Header("UI")]
    public TMP_Text timerText;
    public TMP_Text rotationText;
    public TMP_Text highScoreText;
    public GameObject endScreen;        // Twój End Screen w UI

    [Header("Obiekt obracający się")]
    public Transform rotatingObject;
    public float degreesPerRotation = 360f;

    [Header("Reset braku ruchu")]
    public float idleTimeToReset = 7f;
    public float minRotationToAvoidReset = 180f;

    [Header("Obiekt gracza do high score")]
    public Transform player;

    private float currentTime;
    private bool timerActive = true;

    private float previousRotation;
    private float totalRotation;
    private float accumulatedRotation;
    private float rotationSinceLastCheck;
    private float timeSinceLastMovement;

    private float highScore = 0f;
    private float startPlayerY = 0f;

    void Start()
    {
        currentTime = startTime;
        UpdateTimerText();
        UpdateRotationText(0f);

        if (player != null)
            startPlayerY = player.position.y;

        highScore = 0f;
        UpdateHighScoreText();

        if (rotatingObject == null)
            rotatingObject = transform;

        previousRotation = rotatingObject.eulerAngles.z;

        // Ukryj end screen na start
        if (endScreen != null)
            endScreen.SetActive(false);
    }

    void Update()
    {
        if (timerActive)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                currentTime = 0;
                timerActive = false;
                ShowEndScreen(); // zamiast Time.timeScale = 0
            }

            UpdateTimerText();
        }

        TrackRotation();
        CheckIdleReset();
        UpdateHighScore();
    }

    private void TrackRotation()
    {
        float currentRotation = rotatingObject.eulerAngles.z;
        float deltaRotation = Mathf.DeltaAngle(previousRotation, currentRotation);
        previousRotation = currentRotation;

        if (Mathf.Abs(deltaRotation) > 0.01f)
        {
            totalRotation += Mathf.Abs(deltaRotation);
            accumulatedRotation += Mathf.Abs(deltaRotation);
            rotationSinceLastCheck += Mathf.Abs(deltaRotation);
            timeSinceLastMovement = 0f;

            if (accumulatedRotation >= degreesPerRotation)
            {
                int fullRotations = Mathf.FloorToInt(accumulatedRotation / degreesPerRotation);
                accumulatedRotation -= fullRotations * degreesPerRotation;
                AddTime(fullRotations);
            }
        }
        else
        {
            timeSinceLastMovement += Time.deltaTime;
        }

        UpdateRotationText(totalRotation);
    }

    private void CheckIdleReset()
    {
        if (timeSinceLastMovement >= idleTimeToReset)
        {
            if (rotationSinceLastCheck < minRotationToAvoidReset)
            {
                totalRotation = 0f;
                accumulatedRotation = 0f;
                UpdateRotationText(totalRotation);
            }

            timeSinceLastMovement = 0f;
            rotationSinceLastCheck = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Stop"))
        {
            timerActive = false;
            ShowEndScreen(); // end screen też przy kolizji ze śmiercią
        }
        else if (collision.CompareTag("Start"))
        {
            if (currentTime > 0)
                timerActive = true;
        }
    }

    public void AddTime(int seconds)
    {
        currentTime += seconds;
        UpdateTimerText();
    }

    void UpdateTimerText()
    {
        int seconds = Mathf.CeilToInt(currentTime);
        timerText.text = seconds.ToString();
    }

    void UpdateRotationText(float degrees)
    {
        if (rotationText != null)
        {
            int roundedDegrees = Mathf.RoundToInt(degrees);
            rotationText.text = $"{roundedDegrees}°"; // pełne stopnie z symbolem °
        }
    }

    private void UpdateHighScore()
    {
        if (player == null) return;

        float relativeY = player.position.y - startPlayerY;
        if (relativeY > highScore && relativeY > 0f)
        {
            highScore = relativeY;
            UpdateHighScoreText();
        }
    }

    private void UpdateHighScoreText()
    {
        if (highScoreText != null)
            highScoreText.text = Mathf.FloorToInt(highScore).ToString(); // tylko liczba
    }

    private void ShowEndScreen()
    {
        if (endScreen != null)
            endScreen.SetActive(true);
    }
}