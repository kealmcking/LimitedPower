using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public bool hasBlastRadius;

    [SerializeField] AudioSource sfx;

    public AudioClip hit;

    public GameObject blastRadius;
    public GameObject hitAnim;
    public int bulletDamage;

    IEnumerator destroySelf;

    // Start is called before the first frame update
    void Start()
    {
        sfx = GameObject.Find("SFX").GetComponent<AudioSource>();
        StartCoroutine("DestroySelf");
    }

    public void Destruction() {
        if (hasBlastRadius)
        {
            GameObject clone = Instantiate(blastRadius, transform.position, Quaternion.identity);
        }
        else {
            Vector3 randomRot = new Vector3(0, 0, Random.Range(0, 360));
            Quaternion quaternion = Quaternion.Euler(randomRot);
            GameObject clone = Instantiate(hitAnim, transform.position, quaternion);
        }
        Destroy(this.gameObject);
    }

    private IEnumerator DestroySelf() {
        yield return new WaitForSeconds(4);
        Destruction();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player" && collision.tag != "PlayerPickupRange" && collision.tag != "Ignore" && collision.tag != "ExpDrop")
        {
            Destruction();
        }
        
        if (collision.GetComponent<Health>()) {
            sfx.PlayOneShot(hit, 1);
            collision.GetComponent<Health>().health -= bulletDamage;
        }
        
    }

}
