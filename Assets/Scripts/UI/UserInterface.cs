using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class UserInterface : MonoBehaviour
{
    public Player player;
    public Slider slider;
    public Image sliderImage;
    public Slider expSlider;

    public Image healthBar;
    public List<GameObject> healthBarSprites;
    GameManager gameManager;

    public TMP_Text scoreText;
    public TMP_Text powerPercentText;
    public TMP_Text highScoreText, currentScoreText;
    public TMP_Text levelText;

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
        MovePowerTextToTenPercentOfSliderWidth();
        expSlider.value = gameManager.playerExp;
        levelText.text = "LEVEL: " + player.stats.playerLevel;
        expSlider.maxValue = gameManager.expToNextLevel;
        slider.maxValue = player.stats.maxPower;
        slider.value = player.stats.powerAvail;
        sliderImage.color = Color.Lerp(Color.red, Color.yellow, slider.value / 100);
        for (int i = 0; i < healthBarSprites.Count; i++) {
            EnableHealthObject(i);
        }

        scoreText.text = "Score: " + gameManager.playerScore;
    }

    void EnableHealthObject(int health) {
        if (player.stats.playerHP > health) {
            healthBarSprites[health].SetActive(true);
        } else if (player.stats.playerHP <= health) {
            healthBarSprites[health].SetActive(false);
        } else if (health < 0) {
            Debug.Log("HEALTH IS OUT OF RANGE");
        }

        
    }

    void MovePowerTextToTenPercentOfSliderWidth() {
        RectTransform myRectTransform = powerPercentText.GetComponent<RectTransform>();
        float sliderHeight = slider.fillRect.rect.width;
        float sliderHeightCorrected = sliderHeight / 10;
        float tenPercentOfSliderHeight = sliderHeightCorrected;
        Vector3 newTextPos = new Vector3(tenPercentOfSliderHeight, myRectTransform.localPosition.y,myRectTransform.localPosition.z);
        myRectTransform.localPosition = newTextPos;
    }
}
