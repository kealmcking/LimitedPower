using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExpDrop : MonoBehaviour
{

    public bool moveTowardPlayer;

    GameObject player;

    public int expValue;
    public float pickupDistance;

    public float m_Speed;

    float distanceToPlayer;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (moveTowardPlayer) {
            distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, Time.deltaTime * m_Speed);
            if (distanceToPlayer <= pickupDistance) {
                Player playerRef = player.GetComponent<Player>();
                playerRef.PlayEXPUpSound();
                playerRef.stats.playerExp += expValue;
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerPickupRange") {
            player = collision.gameObject.transform.parent.gameObject;
            moveTowardPlayer = true;
        }
    }
}
