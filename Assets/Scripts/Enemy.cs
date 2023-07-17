using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public int enemyHP;
    Animator animator;

    [SerializeField] AudioSource sfx;
    [SerializeField] AudioClip hit, spawn, die;

    SpriteRenderer renderer;

    public float m_Speed;

    public int enemyScoreValue;

    public int m_Damage;

    public Rigidbody2D rb2D;

    public Player player;
    bool soundPlayed;

    bool facingRight;

    public GameObject healthDrop, expDrop, massExpPickup, energyDrop;
    bool hasSpawnedPickup;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        sfx = GameObject.Find("SFX").GetComponent<AudioSource>();
        player = GameObject.Find("Player").GetComponent<Player>();
        sfx.PlayOneShot(spawn, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isPaused) return;
        FlipSprite();
        FollowPlayer();
        Die();
    }

    void Die() {
        if (enemyHP < 1) {
            animator.SetBool("hasDied", true);
            StartCoroutine(TimeTillDestroy());
            
            if (!soundPlayed) {
                int randValue = Random.Range(0, 100);
                if (randValue > 70 && randValue <= 80 && player.stats.playerHP == 5) {
                    randValue = 0;
                }
                switch (randValue) {
                    case <= 70:
                        SpawnPickups(expDrop);
                        break;
                    case > 70 and <= 80:
                        SpawnPickups(healthDrop);
                        break;
                    case > 80 and <= 90:
                        SpawnPickups(energyDrop);
                        break;
                    case > 90 and <= 95:
                        SpawnPickups(massExpPickup);
                        break;
                    case > 95 and <= 100:
                        Debug.Log("Account for one more pickup type here (>95 and <= 100). Temporarily spawns exp");
                        SpawnPickups(expDrop);
                        break;
                    default:
                        Debug.Log("You did something impossible here, figure it out");
                        break;
                      
                }

                player.stats.playerScore += enemyScoreValue;
                sfx.PlayOneShot(die, 0.25f);
                soundPlayed = true;
            }
            
        }
    }

    private void SpawnPickups(GameObject objectToInstantiate) { 
        GameObject spawnedObject = Instantiate(objectToInstantiate, transform.position, Quaternion.identity);
        hasSpawnedPickup = true;
    }

    private IEnumerator TimeTillDestroy() {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        
        Destroy(collider);

        yield return new WaitForSeconds(0.75f);

        Destroy(this.gameObject);
    }

    void FollowPlayer() {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, Time.deltaTime * m_Speed);
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Bullet") {
            sfx.PlayOneShot(hit, 1);
            enemyHP -= 1;
            Destroy(collision.gameObject);
        }

        if (collision.tag == "Player") {
            player.UpdateHP(-1);
        }
    }

    void FlipSprite() {
        if (player.transform.position.x > transform.position.x && !facingRight) {
            facingRight = true;
            renderer.flipX = false;
        } else if (player.transform.position.x < transform.position.x && facingRight) {
            facingRight = false;
            renderer.flipX = true;
        }
    }
}
