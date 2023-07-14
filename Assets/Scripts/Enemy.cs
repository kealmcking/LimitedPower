using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public int enemyHP;

    [SerializeField] AudioSource sfx;
    [SerializeField] AudioClip hit, spawn, die;

    SpriteRenderer renderer;

    public float m_Speed;

    public int enemyScoreValue;

    public int m_Damage;

    public Rigidbody2D rb2D;

    public Player player;

    bool facingRight;

    // Start is called before the first frame update
    void Start()
    {
        
        renderer = gameObject.GetComponent<SpriteRenderer>();
        sfx = GameObject.Find("SFX").GetComponent<AudioSource>();
        player = GameObject.Find("Player").GetComponent<Player>();
        sfx.PlayOneShot(spawn, 1);
        FlipSprite();
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
        if (enemyHP == 0) {
            sfx.PlayOneShot(die, 0.25f);
            player.playerScore += enemyScoreValue;
            Destroy(this.gameObject);
        }
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

        if (collision.tag == "Player" && !player.playerHasIFrames) {
            player.playerHP -= 1;
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
