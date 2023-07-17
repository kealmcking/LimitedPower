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
    [SerializeField] internal int playerExp;

    [SerializeField] internal Upgrade selectedUpgrade;

    [SerializeField] UpgradeButton button01, button02, button03;


    public float expToNextLevel;

    internal bool isLevelingUp;

    bool hasSetUpgrades;

    [SerializeField] int highScore;
    [SerializeField] int playerLevel;

    public bool isPaused;

    bool canOpenPauseMenu;

    public float enemyCircleRadius;

    float timeTillNextEnemy;
    public float enemyTime;

    public static GameManager Instance { get; private set; }

    void Awake() { 
        Time.timeScale = 1.0f;
        InstanceCheck();
    }

    private void InstanceCheck() {
        if (Instance != null && Instance != this) {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        ScoreLoad();
        player = GameObject.Find("Player").GetComponent<Player>();
        canOpenPauseMenu = true;
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        PauseMenu();
        playerScore = player.stats.playerScore;
        playerExp = player.stats.playerExp;
        IncreaseSpawnRate();
        CheckIfPlayerDead();
        LevelUp();
        if (!isPaused)
        {
            SpawnEnemies();
        }
        enemyTime = Mathf.Clamp(enemyTime, 0.5f, enemyTime);
    }

    void CheckIfPlayerDead() {
        if (player.isDead) {
            GameOver();
        }
    }

    void LevelUp()
    {
        if (playerExp >= expToNextLevel && isLevelingUp == false)
        {
            Cursor.visible = true;
            expToNextLevel *= 1.2f;
            player.stats.playerLevel += 1;
            playerLevel = player.stats.playerLevel;
            player.stats.powerAvail = player.stats.maxPower;
            if (!hasSetUpgrades) {
                Debug.Log("Is In Loop");
                button01.SetRandomUpgrade();
                button02.SetRandomUpgrade();
                button03.SetRandomUpgrade();
                hasSetUpgrades = true;
            }

            userInterface.levelUpMenu.SetActive(true);
            canOpenPauseMenu = false;
            playerExp = 0;
            player.stats.playerExp = playerExp;
            Time.timeScale = 0;


        }
    }

    public void CloseLevelUpMenu() {
        DistributeUpgradeStats(selectedUpgrade);

        Time.timeScale = 1;
        userInterface.levelUpMenu.SetActive(false);
        canOpenPauseMenu = true;
        isLevelingUp = false;
        hasSetUpgrades = false;
        Cursor.visible = true;
    }

    public void DistributeUpgradeStats(Upgrade upgrade) {
        player.stats.playerHP += upgrade.m_healthUpgrade;
        player.stats.maxPower += upgrade.m_maxPowerUpgrade;
        player.stats.v_AmbientRecharge += upgrade.m_ambientRechargeUpgrade;
        player.stats.m_Speed += upgrade.m_speedUpgrade;
        player.stats.m_SprintSpeed += upgrade.m_sprintSpeedUpgrade;
        player.stats.timeBetweenRegularShots -= upgrade.m_shotSpeedUpgrade;
        player.stats.v_PickupRange += upgrade.m_pickupRangeUpgrade;
    }

    void SpawnEnemies() {
        if (!player.isDead) {
            timeTillNextEnemy -= Time.deltaTime;
            if (timeTillNextEnemy <= 0) {
                Instantiate(enemy, player.transform.position - RandomPointOnCircleEdge(enemyCircleRadius), Quaternion.identity);
                timeTillNextEnemy = enemyTime;
            }
        }
    }

    void IncreaseSpawnRate() {
        enemyTime -= 0.005f * Time.deltaTime;
    }

    private Vector3 RandomPointOnCircleEdge(float radius) {
        Vector2 vector2 = Random.insideUnitCircle.normalized * radius;
        return new Vector3(vector2.x, vector2.y, 0);
    }

    void PauseMenu() {
        if (!canOpenPauseMenu) return;
        if (!player.isDead) {
            if (isPaused) {
                Time.timeScale = 0;
                userInterface.pauseMenu.SetActive(true);
            } else {
                Time.timeScale = 1;
                userInterface.pauseMenu.SetActive(false);
            }
        }
    }

    void GameOver() {
        if (PlayerPrefs.HasKey("highScore")) {
            userInterface.highScoreText.text = "High Score: " + PlayerPrefs.GetInt("highScore");
        } else {
            userInterface.highScoreText.text = "High Score: " + playerScore;
        }
        
        userInterface.currentScoreText.text = "Current Score: " + playerScore;
        StartCoroutine(WaitToShowGameOver());

        ScoreSave();
    }

    private IEnumerator WaitToShowGameOver() {

        yield return new WaitForSeconds(1);
        Time.timeScale = 0;
        userInterface.gameOverMenu.SetActive(true);
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
