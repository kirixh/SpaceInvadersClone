using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    int score = 0;
    Text scoreText;

    public static GameController instance;
    Jet jet;

    public GameObject[] jetPrefabs;
    public GameObject[] enemyPrefabs;
    public GameObject[] powerUpPrefabs;
    int jetTier = 0;

    public Vector2 minPosition;
    public Vector2 maxPosition;

    public float enemySpawnInterval = 5f;
    float enemySpawnTimer = 0f;
    float difficultyInterval = 30f;
    float difficultyTimer = 0f;

    public float PowerUpSpawnChance = 30;

    public float shakeRange = 45;
    public float MaximumStress = 0.6f;

    private void Awake()
    {
        instance = this;
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        jet = GameObject.FindGameObjectWithTag("Jet").GetComponent<Jet>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemySpawnTimer >= enemySpawnInterval)
        {
            spawnEnemy();
            enemySpawnTimer = 0f;
        }
        else
        {
            enemySpawnTimer += Time.deltaTime;
        }

        if (difficultyTimer >= difficultyInterval)
        {
            difficultyTimer = 0f;
            if (enemySpawnInterval > 1f)
                enemySpawnInterval -= 0.5f;
        }
        else
        {
            difficultyTimer += Time.deltaTime;
        }
    }

    public void AddScore(int points)
    {
        score += points;
        scoreText.text = score.ToString();
    }

    public void DecreaseHealPoints()
    {
        if (jet)
            jet.DecreaseHealPoints();
    }

    private void spawnEnemy()
    {
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject enemy = Instantiate(enemyPrefabs[randomIndex]);
        Vector2 randomPosition = new Vector2(Random.Range(minPosition.x, maxPosition.x), Random.Range(minPosition.y, maxPosition.y));
        enemy.transform.position = randomPosition;
    }

    public void spawnPowerUp(Vector2 pos)
    {
        int randomValue = Random.Range(0, 100);
        if (0 <= randomValue && randomValue < PowerUpSpawnChance)
        {
            int randomIndex = Random.Range(0, powerUpPrefabs.Length);
            GameObject powerUp = Instantiate(powerUpPrefabs[randomIndex]);
            powerUp.transform.position = pos;
        }
    }

    public void camShake()
    {
        var target = GameObject.FindGameObjectWithTag("MainCamera");
        var receiver = target.GetComponent<StressReceiver>();
        if (receiver == null) return;
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance > shakeRange) return;
        float distance01 = Mathf.Clamp01(distance / shakeRange);
        float stress = (1 - Mathf.Pow(distance01, 2)) * MaximumStress;
        receiver.InduceStress(stress);
    }
    public void upgradeJet()
    {
        ++jetTier;
        if (jetTier < jetPrefabs.Length)
        {
            Jet newJet = Instantiate(jetPrefabs[jetTier], jet.transform.position, jet.getQuaternionIdentity()).GetComponent<Jet>();
            jet.DestroyJet();
            jet = newJet;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
