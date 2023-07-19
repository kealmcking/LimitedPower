using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAllInRange : MonoBehaviour {

    public int damage;

    void Start() {
        StartCoroutine(WaitToDestroy());   
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Enemy") {
            collision.GetComponent<Health>().health -= damage;
        }
    }


    IEnumerator WaitToDestroy() {
        yield return new WaitForSeconds(0.5f);
        Destroy(transform.parent.gameObject);
    }
}
