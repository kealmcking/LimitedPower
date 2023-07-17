using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserInterface : MonoBehaviour
{
    public Player player;
    public Slider slider;
    public Image sliderImage;
    public Slider expSlider;

    public Image healthBar;
    public List<Sprite> healthBarSprites;
    GameManager gameManager;

    public TMP_Text scoreText;
    public TMP_Text highScoreText, currentScoreText;

    public GameObject pauseMenu, gameOverMenu, tutorialMenu, levelUpMenu;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        slider.minValue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        expSlider.value = gameManager.playerExp;
        expSlider.maxValue = gameManager.expToNextLevel;
        slider.maxValue = player.stats.maxPower;
        slider.value = player.stats.powerAvail;
        sliderImage.color = Color.Lerp(Color.blue, Color.cyan, slider.value / 10);
        if (player.stats.playerHP >= 0) {
            healthBar.sprite = healthBarSprites[player.stats.playerHP];
        }

        scoreText.text = "Score: " + gameManager.playerScore;
    }
}
