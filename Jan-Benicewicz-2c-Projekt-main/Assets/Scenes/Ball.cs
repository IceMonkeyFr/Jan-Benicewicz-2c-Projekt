using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    public Rigidbody2D rb;
    public float startingSpeed;
    public Text
        ScoreP1,
        ScoreP2;

   
    public int player1Score = 0;
    public int player2Score = 0;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;

        bool isRight = UnityEngine.Random.value >= 0.5;

        float xVelocity = -1f;

        if (isRight == true)
        {
            xVelocity = 1f;
        }

        float yVelocity = UnityEngine.Random.Range(-1, 1);

        rb.velocity = new Vector2(xVelocity * startingSpeed, yVelocity * startingSpeed);
    }

    void Update()
    {
        CheckIfOutOfBounds();
    }


    void CheckIfOutOfBounds()
    {
        Vector3 pos = transform.position;

        float leftLimit = cam.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
        float rightLimit = cam.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;

        if (pos.x > rightLimit)
        {
            ScoreP1.text = "" + player1Score++;
            Debug.Log("Gracz 1 +1 | Wynik: " + player1Score);
            ResetBall();
        }

        
        if (pos.x < leftLimit)
        {
            ScoreP2.text = "" + player2Score++;
            Debug.Log("Gracz 2 +1 | Wynik: " + player2Score);
            ResetBall();
        }
    }

    
    void ResetBall()
    {
        transform.position = Vector3.zero;

        bool isRight = UnityEngine.Random.value >= 0.5;
        float xVelocity = isRight ? 1f : -1f;
        float yVelocity = UnityEngine.Random.Range(-1f, 1f);

        rb.velocity = new Vector2(
            xVelocity * startingSpeed,
            yVelocity * startingSpeed
        );
    }
}
