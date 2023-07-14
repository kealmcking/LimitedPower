using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour
{

    float horiz;
    float vert;
    bool shift;
    bool hasClickedLMouse;
    bool hasClickedRMouse;

    bool facingRight;

    bool canFire;
    bool canMove;

    public bool isDead;
    public bool isPaused;

    public float m_Speed;
    public float m_SprintSpeed;
    public float m_BulletSpeed;

    public float v_SprintDraw;
    public float v_ShootDraw;
    public float v_BigShootDraw;
    public float v_AmbientRecharge;

    public float powerAvail;
    public float maxPower;
    public float timer;

    public float timeBetweenRegularShots;
    public float timeBetweenBigShots;

    public int playerHP;
    public int playerScore;

    public AudioClip fire, footstep, hit, die;
    public AudioSource sfx;

    SpriteRenderer renderer;
    internal Animator animator;

    public Rigidbody2D firePrefab;

    public float iFrames;
    float iFrameTimer;
    public bool playerHasIFrames;

    float playerHPLastFrame;

    [SerializeField] CinemachineVirtualCamera virtCam;
    

    Rigidbody2D rb2D;
    // Start is called before the first frame update
    void Start()
    {
        playerHPLastFrame = playerHP;
        rb2D = GetComponent<Rigidbody2D>();
        powerAvail = maxPower;
        renderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;
        if (isPaused) return;
        CheckIfPlayerLostHealth();
        PowerUnderTenPercent();

        if (canMove) {
            horiz = Input.GetAxis("Horizontal");
            vert = Input.GetAxis("Vertical");
            shift = Input.GetKey(KeyCode.LeftShift);
        } else {
            horiz = 0;
            vert = 0;
            shift = false;
        }

        
        hasClickedLMouse = Input.GetMouseButton(0);
        hasClickedRMouse = Input.GetMouseButton(1);

        if (horiz != 0 || vert != 0) {
            animator.SetBool("isMoving", true);
        } else {
            animator.SetBool("isMoving", false);
        }

        
        FlipSprite();
        FireWeapon();
        Recharge();

        powerAvail = Mathf.Clamp(powerAvail, 0, maxPower);

        Dead();
        
    }

    private IEnumerator _ProcessShake(float shakeIntensity = 5f, float shakeTiming = 0.5f) {
            Noise(1, shakeIntensity);
            yield return new WaitForSeconds (shakeTiming);
            Noise(0,0);
    }

    public void Noise(float amplitudeGain, float frequencyGain) {

        virtCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitudeGain;
        virtCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = frequencyGain;
    }

    void CheckIfPlayerLostHealth() {
        if (playerHP != playerHPLastFrame) {
            sfx.PlayOneShot(hit, 1);
            iFrameTimer = iFrames;
            iFrameTimer -= Time.deltaTime;
            playerHasIFrames = true;
        } else {
            iFrameTimer = 0;
            playerHasIFrames = false;
        }
        
    }

    void FixedUpdate() {
        if (isDead) return;
        if (isPaused) return;

        MovePlayer();
    }

    void LateUpdate() {
        playerHPLastFrame = playerHP;
    }

    void MovePlayer() {
        if (shift && powerAvail > maxPower * 0.15f) {
            Debug.Log("Sprint");
            Vector3 m_Input = new Vector3(horiz, vert, 0);
            rb2D.MovePosition(transform.position + m_Input * Time.deltaTime * m_SprintSpeed);
            powerAvail -= v_SprintDraw;
        } else if (shift && powerAvail == maxPower * 0.15f) {
            Debug.Log("Walk");
            Vector3 m_Input = new Vector3(horiz, vert, 0);
            rb2D.MovePosition(transform.position + m_Input * Time.deltaTime * m_Speed);
        } else {
            Debug.Log("Walk");
            Vector3 m_Input = new Vector3(horiz, vert, 0);
            rb2D.MovePosition(transform.position + m_Input * Time.deltaTime * m_Speed);
        }

    }

    void FireWeapon() {
        if (!canFire) return;
        if (timer > 0) {
            timer -= Time.deltaTime;
        } else {
            if (hasClickedLMouse) {
                sfx.PlayOneShot(fire, 1);
                StartCoroutine(_ProcessShake(15f, 0.25f));
                powerAvail -= v_ShootDraw;
                timer = timeBetweenRegularShots;
                Rigidbody2D clone = Instantiate(firePrefab, transform.position, Quaternion.identity);
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                clone.velocity = transform.TransformDirection((mousePosition - transform.position) * m_BulletSpeed);
            }
        }
    }

    void Recharge() {
        if (vert == 0 && horiz == 0 && hasClickedLMouse == false) {
            powerAvail += (v_AmbientRecharge * 4) * Time.deltaTime;
        } else {
            powerAvail += v_AmbientRecharge * Time.deltaTime;
        }
        
    }

    void FlipSprite() {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePosition.x > transform.position.x && !facingRight) {
            facingRight = true;
            renderer.flipX = false;
        } else if (mousePosition.x < transform.position.x && facingRight) {
            facingRight = false;
            renderer.flipX = true;
        }
    }

    void PowerUnderTenPercent() {
        if (powerAvail > maxPower * 0.1f) {
            canFire = true;
            canMove = true;
        } else {
            canFire = false;
            canMove = false;
        }
    }

    void Dead() {
        if (playerHP <= 0) {
            sfx.PlayOneShot(die, 1);
            isDead = true;
        }
    }
}
