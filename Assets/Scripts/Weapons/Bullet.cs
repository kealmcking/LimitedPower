using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public bool hasBlastRadius;


    public GameObject blastRadius;

    IEnumerator destroySelf;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("DestroySelf");
    }

    public void Destruction() {
        if (hasBlastRadius) {
            GameObject clone = Instantiate(blastRadius, transform.position, Quaternion.identity);
        }
        Destroy(this.gameObject);
    }

    private IEnumerator DestroySelf() {
        yield return new WaitForSeconds(4);
        Destruction();
    }
    
}
