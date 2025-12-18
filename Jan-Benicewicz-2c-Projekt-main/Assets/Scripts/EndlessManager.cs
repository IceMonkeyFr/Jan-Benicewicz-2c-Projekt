using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class EndlessManager : MonoBehaviour
{
    #region LEVEL_SETTINGS
    public List<GameObject> platformPrefabs;

    [Header("Odleg³oœæ miêdzy platformami (min-max)")]
    public float minDistanceBetween = 2f;
    public float maxDistanceBetween = 4f;

    [Header("Wychylenie platform w lewo/prawo")]
    public float maxXOffset = 2f;

    public int preloadCount = 10;
    #endregion

    #region PLAYER_SETTINGS
    public Transform player;
    #endregion

    #region DEATH_SETTINGS
    public Transform deathZone;
    public float deathOffset = 6f;
    #endregion

    #region UI_SETTINGS
    public GameObject deathScreen;
    #endregion

    private float highestPointY;
    private bool isDead = false;

    void Start()
    {
        Time.timeScale = 1f;

        if (deathScreen != null)
            deathScreen.SetActive(false);

        highestPointY = transform.position.y;

        for (int i = 0; i < preloadCount; i++)
            SpawnPlatform();
    }

    void Update()
    {
        if (isDead) return;

        GeneratePlatformsIfNeeded();
        MoveDeathZone();
        CheckPlayerDeath();
    }

    // ------------------------------------------------------------
    //          GENEROWANIE PLATFORM
    // ------------------------------------------------------------
    void GeneratePlatformsIfNeeded()
    {
        if (player.position.y + 10f > highestPointY)
            SpawnPlatform();
    }

    void SpawnPlatform()
    {
        if (platformPrefabs.Count == 0) return;

        int index = Random.Range(0, platformPrefabs.Count);

        float randomX = transform.position.x + Random.Range(-maxXOffset, maxXOffset);
        float randomYDistance = Random.Range(minDistanceBetween, maxDistanceBetween);

        Vector3 pos = new Vector3(
            randomX,
            highestPointY,
            transform.position.z
        );

        Instantiate(platformPrefabs[index], pos, Quaternion.identity);

        highestPointY += randomYDistance;
    }

    // ------------------------------------------------------------
    //              RUCHOME DNO
    // ------------------------------------------------------------
    void MoveDeathZone()
    {
        float targetY = player.position.y - deathOffset;

        if (targetY > deathZone.position.y)
        {
            deathZone.position = new Vector3(
                deathZone.position.x,
                targetY,
                deathZone.position.z
            );
        }
    }

    // ------------------------------------------------------------
    //              DETEKCJA ŒMIERCI
    // ------------------------------------------------------------
    void CheckPlayerDeath()
    {
        if (player.position.y < deathZone.position.y)
            KillPlayer();
    }

    void KillPlayer()
    {
        isDead = true;
        Time.timeScale = 0f;

        if (deathScreen != null)
            deathScreen.SetActive(true);
    }

    // ------------------------------------------------------------
    //                   RESTART
    // ------------------------------------------------------------
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
