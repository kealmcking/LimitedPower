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

    public Image healthBar;
    public List<Sprite> healthBarSprites;
    GameManager gameManager;

    public TMP_Text scoreText;
    public TMP_Text highScoreText, currentScoreText;

    public GameObject pauseMenu, gameOverMenu, tutorialMenu;
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
        slider.maxValue = player.maxPower;
        slider.value = player.powerAvail;
        sliderImage.color = Color.Lerp(Color.red, Color.green, slider.value / 20);
        healthBar.sprite = healthBarSprites[player.playerHP];
        scoreText.text = "Score: " + gameManager.playerScore;
    }
}
