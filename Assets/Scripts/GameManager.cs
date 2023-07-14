using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] Player player;
    [SerializeField] UserInterface userInterface;

    [SerializeField] GameObject enemy;

    [SerializeField] internal int playerScore;
    [SerializeField] float playerTime;

    [SerializeField] int highScore;

    bool isPaused;

    public float enemyCircleRadius;

    float timeTillNextEnemy;
    public float enemyTime;

    // Start is called before the first frame update
    void Start()
    {
        ScoreLoad();
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        playerScore = player.playerScore;
        IncreaseSpawnRate();
        CheckIfPlayerDead();
        PauseMenu();
        SpawnEnemies();
        enemyTime = Mathf.Clamp(enemyTime, 0.5f, enemyTime);
    }

    void CheckIfPlayerDead() {
        if (player.isDead) {
            GameOver();
        }
    }

    void SpawnEnemies() {
        if (!player.isDead || !player.isPaused) {
            timeTillNextEnemy -= Time.deltaTime;
            if (timeTillNextEnemy <= 0) {
                Instantiate(enemy, player.transform.position - RandomPointOnCircleEdge(enemyCircleRadius), Quaternion.identity);
                timeTillNextEnemy = enemyTime;
            }

        }
    }

    void IncreaseSpawnRate() {
        enemyTime -= 0.01f * Time.deltaTime;
    }

    private Vector3 RandomPointOnCircleEdge(float radius) {
        Vector2 vector2 = Random.insideUnitCircle.normalized * radius;
        return new Vector3(vector2.x, vector2.y, 0);
    }

    void PauseMenu() {
        if (Input.GetKeyDown(KeyCode.Escape) && !player.isDead) {
            if (!isPaused) {
                player.animator.speed = 0;
                userInterface.pauseMenu.SetActive(true);
                isPaused = true;
            } else {
                player.animator.speed = 1;
                userInterface.pauseMenu.SetActive(false);
                isPaused = false;
            }
            player.isPaused = isPaused;
        }
    }

    void GameOver() {
        if (PlayerPrefs.HasKey("highScore")) {
            userInterface.highScoreText.text = "High Score: " + PlayerPrefs.GetInt("highScore");
        } else {
            userInterface.highScoreText.text = "High Score: " + playerScore;
        }
        
        userInterface.currentScoreText.text = "Current Score: " + playerScore;
        userInterface.gameOverMenu.SetActive(true);
        ScoreSave();
    }

    public void QuitGame() {
        Application.Quit();

    }

    void ScoreLoad() {
        if (PlayerPrefs.HasKey("highScore")) {
            highScore = PlayerPrefs.GetInt("highScore");
        }
    }

    void ScoreSave() {
        if (PlayerPrefs.HasKey("highScore")) {
            if (playerScore > PlayerPrefs.GetInt("highScore")) {
                highScore = playerScore;
                PlayerPrefs.SetInt("highScore", highScore);
                PlayerPrefs.Save();
            }
        } else {
            if (playerScore > highScore) {
                highScore = playerScore;
                PlayerPrefs.SetInt("highScore", highScore);
                PlayerPrefs.Save();
            }
        }
    }

    public void RestartLevel() {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
 }
