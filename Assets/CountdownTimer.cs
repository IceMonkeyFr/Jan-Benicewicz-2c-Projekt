using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    [Header("Ustawienia czasu")]
    public float startTime = 60f;

    [Header("UI")]
    public TMP_Text timerText;          // Tekst pokazujący czas
    public TMP_Text rotationText;       // Tekst pokazujący obrót w stopniach

    [Header("Obiekt obracający się")]
    public Transform rotatingObject;    // Obiekt, którego obrót śledzimy
    public float degreesPerRotation = 360f; // 1 pełny obrót = 360 stopni

    [Header("Reset braku ruchu")]
    public float idleTimeToReset = 7f;  // czas bez ruchu po którym reset
    public float minRotationToAvoidReset = 180f; // minimalny obrót, by nie zresetować

    private float currentTime;
    private bool timerActive = true;

    
    private float previousRotation;
    private float totalRotation;
    private float accumulatedRotation; 
    private float rotationSinceLastCheck; 
    private float timeSinceLastMovement;

    void Start()
    {
        currentTime = startTime;
        UpdateTimerText();
        UpdateRotationText(0f);
        Time.timeScale = 1f;

        if (rotatingObject == null)
            rotatingObject = transform;

        previousRotation = rotatingObject.eulerAngles.z;
    }

    void Update()
    {
        if (timerActive && Time.timeScale > 0)
        {
            
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                currentTime = 0;
                timerActive = false;
                Time.timeScale = 0f;
            }

            UpdateTimerText();
        }

        TrackRotation();

      
        CheckIdleReset();
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
                Debug.Log($"Dodano {fullRotations} sekund za {fullRotations} pełny obrót!");
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
                Debug.Log($"Reset liczby obrotów — brak ruchu przez {idleTimeToReset} s.");
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
            Debug.Log("Dotknięto obiektu z tagiem STOP ");
            timerActive = false;
        }
        else if (collision.CompareTag("Start"))
        {
            if (currentTime > 0)
            {
                Debug.Log("Dotknięto obiektu z tagiem START ");
                timerActive = true;
            }
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
            rotationText.text = $"Obrót: {degrees:F1}°";
        }
    }
}
