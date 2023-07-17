using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering.UI;

public class Player : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtCam;
    
    bool facingRight;
    internal bool canFire;
    bool canMove;

    internal bool isSprinting;
    internal bool isDead;
    internal bool isPaused;
    internal bool shouldFire;
    internal bool playerHasIFrames;

    Vector3 firePoint;

    public GameObject shotPoint;
    public GameObject rGunPoint, lGunPoint;

    public GameObject weapon;

    [Header("References")]
    public AudioSource sfx;
    public AudioClip fire, footstep, hit, die, heal, energy, exp;

    internal PlayerStats stats;
    internal PlayerInput input;

    internal CircleCollider2D pickupRangeCollider;
    internal SpriteRenderer renderer;
    internal Animator animator;

    public Rigidbody2D firePrefab;

    public float iFrames;
    float iFrameTimer;

    
    Rigidbody2D rb2D;

    void Awake() { 
        InitializeVariables();
    }

    void InitializeVariables() {
        pickupRangeCollider = GetComponentInChildren<CircleCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        stats = GetComponent<PlayerStats>();
        input = GetComponent<PlayerInput>();
        renderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        if (isDead || isPaused) return;

        PowerUnderTenPercent();
        UpdatePickupRange();
        FlipSprite();
        Recharge();
        Dead();
    }


    public void PlayEXPUpSound() {
        sfx.PlayOneShot(exp, 1);
    }

    void FixedUpdate() {
        if (isDead) return;
        if (isPaused) return;

        MovePlayer();
    }

    void UpdatePickupRange() {
        pickupRangeCollider.radius = stats.v_PickupRange;
    }



    void MovePlayer() {
        if (isSprinting && stats.powerAvail > stats.maxPower * 0.15f) {
            Move(stats.m_SprintSpeed);
            EnergyDraw(stats.v_SprintDraw);
        } else if (isSprinting && stats.powerAvail == stats.maxPower * 0.15f) {
            Move(stats.m_Speed);
            EnergyDraw(stats.v_SprintDraw * 0.2f);
        } else {
            Move(stats.m_Speed);
            EnergyDraw(stats.v_SprintDraw * 0.2f);
        }

        if (input.moveInput != Vector2.zero)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

    }

    void Move(float speed) {
        Vector3 m_Input = new Vector3(input.moveInput.x, input.moveInput.y, 0);
        rb2D.MovePosition(transform.position + m_Input * Time.deltaTime * speed);
    }

    void EnergyDraw(float power){
        stats.powerAvail -= power;
    }

    public void FireWeapon() {
        RotateGunTowardMouse gun = weapon.GetComponent<RotateGunTowardMouse>();
        gun.PlayFireAnim();
        sfx.PlayOneShot(fire, 1);
        StartCoroutine(_ProcessShake(15f, 0.25f));
        EnergyDraw(stats.v_ShootDraw);
        firePoint = shotPoint.transform.position;
        Rigidbody2D clone = Instantiate(firePrefab, firePoint, Quaternion.identity);
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        clone.velocity = transform.TransformDirection((mousePosition - firePoint) * stats.m_BulletSpeed);
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
    void Recharge() {
        if (input.moveInput == Vector2.zero) {
            stats.powerAvail += (stats.v_AmbientRecharge * 7) * Time.deltaTime;
        } else {
            stats.powerAvail += stats.v_AmbientRecharge * Time.deltaTime;
        }
        
    }

    public void UpdateHP(int HPUpdateValue) {
        if (HPUpdateValue > 0) {
            sfx.PlayOneShot(heal, 1);
        } else if (HPUpdateValue < 0) {
            sfx.PlayOneShot(hit, 1);
        }
       stats.playerHP += HPUpdateValue;
    }

    public void UpdateEnergy(float energyUpdateValue) {
        stats.powerAvail += energyUpdateValue;
        sfx.PlayOneShot(energy,1);
    }

    void FlipSprite() {
        if (isPaused || isDead) return;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePosition.x < transform.position.x ) {
            weapon.transform.position = lGunPoint.transform.position;
            if (!facingRight) {
                facingRight = true;
                renderer.flipX = false;
                weapon.GetComponent<SpriteRenderer>().flipY = true;
            }

        } else if (mousePosition.x > transform.position.x && facingRight) {
            weapon.transform.position = rGunPoint.transform.position;
            if (facingRight) {
                facingRight = false;
                renderer.flipX = true;
                weapon.GetComponent<SpriteRenderer>().flipY = false;
            }

        }

    }
    

    void PowerUnderTenPercent() {
        if (stats.powerAvail > stats.maxPower * 0.1f) {
            canFire = true;
            canMove = true;
        } else {
            canFire = false;
            canMove = false;
        }
    }

    void Dead() {
        if (stats.playerHP <= 0) {
            animator.SetBool("hasDied", true);
            sfx.PlayOneShot(die, 1);
            isDead = true;
        }
    }
}
